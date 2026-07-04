using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using VikashERP.Application.Features.Sales.Commands;
using VikashERP.Application.Features.Sales.DTOs;
using VikashERP.Application.Features.Sales.Queries;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Enums;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Extensions;

namespace VikashERP.API.Controllers;

[Route("api/sales")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _context;

    public SalesController(IMediator mediator, ApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetInvoices(CancellationToken cancellationToken)
    {
        var query = new GetInvoicesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetInvoiceById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoiceByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id:guid}/pdf")]
    [AllowAnonymous] // Allow viewing PDF without auth for easy share links, or keep authenticated based on security requirements
    public async Task<IActionResult> GetInvoicePdf(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetInvoiceByIdQuery(id);
        var invoice = await _mediator.Send(query, cancellationToken);
        if (invoice == null) return NotFound("Invoice not found.");

        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == invoice.CustomerId, cancellationToken);
        
        var document = new VikashERP.Application.Features.Sales.Documents.InvoiceDocument(invoice, customer);
        var pdfBytes = document.GeneratePdf();

        var filename = $"Invoice_{invoice.InvoiceNumber}.pdf";
        return File(pdfBytes, "application/pdf", filename);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto, CancellationToken cancellationToken)
    {
        var command = new CreateInvoiceCommand(dto);
        var invoiceId = await _mediator.Send(command, cancellationToken);
        return Ok(new { Id = invoiceId });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] CreateInvoiceDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateInvoiceCommand(id, dto);
        var invoiceId = await _mediator.Send(command, cancellationToken);
        return Ok(new { Id = invoiceId });
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> ApproveInvoice(Guid id, CancellationToken cancellationToken)
    {
        var command = new ApproveInvoiceCommand(id);
        var success = await _mediator.Send(command, cancellationToken);
        if (!success) return BadRequest(new { Message = "Failed to approve invoice." });
        return Ok();
    }

    [HttpPut("{id:guid}/payment")]
    public async Task<IActionResult> UpdateInvoicePayment(Guid id, [FromBody] UpdateInvoicePaymentDto dto, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

            if (invoice == null)
                return NotFound(new { Message = "Invoice not found." });

            // Calculate differences for customer balance adjustment if invoice is already approved
            if (invoice.Status == SalesInvoiceStatus.Approved)
            {
                var oldPaidAmount = invoice.PaidAmount;
                var newPaidAmount = dto.CashAmount + dto.BankAmount;
                var difference = newPaidAmount - oldPaidAmount;

                if (difference != 0)
                {
                    // Adjust customer current balance (if they paid MORE, their outstanding dues DECREASE)
                    invoice.Customer.CurrentBalance -= difference;
                    invoice.Customer.UpdatedAt = DateTime.UtcNow;

                    // Update or insert ledger entry for the payment adjustment
                    var lastLedger = await _context.CustomerLedgers
                        .Where(x => x.CustomerId == invoice.CustomerId)
                        .OrderByDescending(x => x.CreatedAt)
                        .FirstOrDefaultAsync(cancellationToken);

                    var prevBalance = lastLedger?.RunningBalance ?? 0;

                    var ledgerEntry = new CustomerLedger
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = invoice.CustomerId,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = "Payment Adjustment",
                        Debit = difference < 0 ? -difference : 0,
                        Credit = difference > 0 ? difference : 0,
                        RunningBalance = prevBalance - difference,
                        Remarks = $"Payment adjustment for Invoice {invoice.InvoiceNumber}. Old: ₹{oldPaidAmount:N2}, New: ₹{newPaidAmount:N2}",
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = User.GetAuthenticatedUserId()
                    };
                    _context.CustomerLedgers.Add(ledgerEntry);
                }
            }

            invoice.CashAmount = dto.CashAmount;
            invoice.BankAmount = dto.BankAmount;
            invoice.PaidAmount = dto.CashAmount + dto.BankAmount;
            invoice.DueAmount = invoice.TotalAmount - invoice.PaidAmount;
            invoice.UpdatedAt = DateTime.UtcNow;
            invoice.UpdatedBy = User.GetAuthenticatedUserId();

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Ok(new { Message = "Invoice payment updated successfully." });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}
