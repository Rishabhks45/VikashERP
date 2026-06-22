using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Features.Brokers.Commands;
using VikashERP.Application.Features.Brokers.DTOs;
using VikashERP.Application.Features.Brokers.Queries;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Extensions;

namespace VikashERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrokersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public BrokersController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetBrokers()
    {
        var brokers = await _mediator.Send(new GetBrokersQuery());
        return Ok(brokers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBroker(Guid id)
    {
        var broker = await _mediator.Send(new GetBrokerByIdQuery(id));
        if (broker == null)
            return NotFound();

        return Ok(broker);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBroker([FromBody] CreateBrokerDto brokerDto)
    {
        try
        {
            var result = await _mediator.Send(new CreateBrokerCommand(brokerDto));
            return CreatedAtAction(nameof(GetBroker), new { id = result!.Id }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBroker(Guid id, [FromBody] UpdateBrokerDto brokerDto)
    {
        try
        {
            var result = await _mediator.Send(new UpdateBrokerCommand(id, brokerDto));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBroker(Guid id)
    {
        var result = await _mediator.Send(new DeleteBrokerCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("transactions")]
    public async Task<IActionResult> RecordTransaction([FromBody] CreateBrokerTransactionDto dto, CancellationToken cancellationToken)
    {
        if (dto.Amount <= 0)
            return BadRequest(new { Message = "Amount must be greater than zero." });

        if (dto.TransactionType != "Payment" && dto.TransactionType != "Commission")
            return BadRequest(new { Message = "Invalid transaction type." });

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var broker = await _context.Brokers.FindAsync(new object[] { dto.BrokerId }, cancellationToken);
            if (broker is null)
                return NotFound(new { Message = "Broker not found." });

            decimal debit = 0;
            decimal credit = 0;
            decimal newBalance = broker.CurrentBalance;

            if (dto.TransactionType == "Payment")
            {
                debit = dto.Amount;
                newBalance -= dto.Amount;
            }
            else // Commission
            {
                credit = dto.Amount;
                newBalance += dto.Amount;
            }

            var remarksPrefix = dto.TransactionType == "Payment" ? $"[PaymentMode: {dto.PaymentMode}] " : "";

            var ledgerEntry = new BrokerLedger
            {
                Id = Guid.NewGuid(),
                BrokerId = dto.BrokerId,
                TransactionDate = DateTime.SpecifyKind(dto.TransactionDate, DateTimeKind.Utc),
                TransactionType = dto.TransactionType,
                Debit = debit,
                Credit = credit,
                RunningBalance = newBalance,
                Remarks = $"{remarksPrefix}{dto.Remarks}".Trim(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = User.GetAuthenticatedUserId()
            };

            broker.CurrentBalance = newBalance;
            broker.UpdatedAt = DateTime.UtcNow;
            broker.UpdatedBy = User.GetAuthenticatedUserId();

            _context.BrokerLedgers.Add(ledgerEntry);
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Ok(new BrokerTransactionResponseDto
            {
                LedgerEntryId = ledgerEntry.Id,
                BrokerId = broker.Id,
                RemainingBalance = broker.CurrentBalance
            });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("transactions/recent")]
    public async Task<ActionResult<IEnumerable<RecentBrokerTransactionDto>>> GetRecentTransactions(CancellationToken cancellationToken)
    {
        try
        {
            var transactions = await _context.BrokerLedgers
                .Include(l => l.Broker)
                .Where(l => !l.IsDeleted && (l.TransactionType == "Payment" || l.TransactionType == "Commission"))
                .OrderByDescending(l => l.TransactionDate)
                .Take(100)
                .ToListAsync(cancellationToken);

            var dtos = transactions.Select(l =>
            {
                var remarks = l.Remarks ?? "";
                var paymentMode = "N/A";
                if (remarks.StartsWith("[PaymentMode: "))
                {
                    var idx = remarks.IndexOf("]");
                    if (idx > 0)
                    {
                        paymentMode = remarks.Substring(14, idx - 14);
                        remarks = remarks.Substring(idx + 1).Trim();
                    }
                }

                return new RecentBrokerTransactionDto
                {
                    Id = l.Id,
                    BrokerId = l.BrokerId,
                    BrokerName = l.Broker != null ? l.Broker.Name : "Unknown Broker",
                    TransactionDate = l.TransactionDate,
                    TransactionType = l.TransactionType,
                    Amount = l.TransactionType == "Payment" ? l.Debit : l.Credit,
                    PaymentMode = paymentMode,
                    Remarks = remarks
                };
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("{id:guid}/ledger")]
    public async Task<IActionResult> GetBrokerLedger(
        Guid id,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.BrokerLedgers
                .Where(l => l.BrokerId == id && !l.IsDeleted)
                .OrderBy(l => l.TransactionDate)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                var utcFrom = DateTime.SpecifyKind(fromDate.Value.Date, DateTimeKind.Utc);
                query = query.Where(l => l.TransactionDate >= utcFrom);
            }

            if (toDate.HasValue)
            {
                var utcTo = DateTime.SpecifyKind(toDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
                query = query.Where(l => l.TransactionDate <= utcTo);
            }

            var entries = await query.ToListAsync(cancellationToken);

            var result = entries.Select(l => new BrokerLedgerEntryDto
            {
                Id = l.Id,
                BrokerId = l.BrokerId,
                TransactionDate = l.TransactionDate,
                TransactionType = l.TransactionType,
                Debit = l.Debit,
                Credit = l.Credit,
                RunningBalance = l.RunningBalance,
                Remarks = l.Remarks
            }).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}

