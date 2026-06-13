using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Features.Brokers.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Brokers;

public class BrokerService : IBrokerService
{
    private readonly IBrokerRepository _brokerRepository;

    public BrokerService(IBrokerRepository brokerRepository)
    {
        _brokerRepository = brokerRepository;
    }

    public async Task<List<BrokerListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var brokers = await _brokerRepository.GetAllAsync(cancellationToken);
        return brokers.Select(b => new BrokerListDto
        {
            Id = b.Id,
            Name = b.Name,
            Phone = b.Phone,
            CurrentBalance = b.CurrentBalance,
            CreatedAt = b.CreatedAt
        }).OrderByDescending(b => b.CreatedAt).ToList();
    }

    public async Task<BrokerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var broker = await _brokerRepository.GetByIdAsync(id, cancellationToken);
        if (broker == null) return null;

        return new BrokerDto
        {
            Id = broker.Id,
            Name = broker.Name,
            Phone = broker.Phone,
            Address = broker.Address,
            CurrentBalance = broker.CurrentBalance,
            CreatedAt = broker.CreatedAt,
            UpdatedAt = broker.UpdatedAt
        };
    }

    public async Task<BrokerDto?> CreateAsync(CreateBrokerDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _brokerRepository.GetByNameAsync(dto.Name, cancellationToken);
        if (existing != null)
        {
            throw new InvalidOperationException($"Broker with name '{dto.Name}' already exists.");
        }

        var broker = new Broker
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Address = dto.Address,
            CurrentBalance = dto.OpeningBalance
        };

        if (dto.OpeningBalance != 0)
        {
            var ledgerEntry = new BrokerLedger
            {
                Broker = broker,
                TransactionType = "Opening Balance",
                Remarks = "Opening balance entry",
                Debit = dto.OpeningBalance > 0 ? 0 : Math.Abs(dto.OpeningBalance),
                Credit = dto.OpeningBalance > 0 ? dto.OpeningBalance : 0,
                RunningBalance = dto.OpeningBalance
            };
            broker.LedgerEntries.Add(ledgerEntry);
        }

        await _brokerRepository.AddAsync(broker, cancellationToken);

        return new BrokerDto
        {
            Id = broker.Id,
            Name = broker.Name,
            Phone = broker.Phone,
            Address = broker.Address,
            CurrentBalance = broker.CurrentBalance,
            CreatedAt = broker.CreatedAt
        };
    }

    public async Task<BrokerDto?> UpdateAsync(Guid id, UpdateBrokerDto dto, CancellationToken cancellationToken = default)
    {
        var broker = await _brokerRepository.GetByIdAsync(id, cancellationToken);
        if (broker == null) return null;

        if (broker.Name.ToLower() != dto.Name.ToLower())
        {
            var existing = await _brokerRepository.GetByNameAsync(dto.Name, cancellationToken);
            if (existing != null)
            {
                throw new InvalidOperationException($"Broker with name '{dto.Name}' already exists.");
            }
        }

        broker.Name = dto.Name;
        broker.Phone = dto.Phone;
        broker.Address = dto.Address;
        broker.UpdatedAt = DateTime.UtcNow;

        await _brokerRepository.UpdateAsync(broker, cancellationToken);

        return new BrokerDto
        {
            Id = broker.Id,
            Name = broker.Name,
            Phone = broker.Phone,
            Address = broker.Address,
            CurrentBalance = broker.CurrentBalance,
            CreatedAt = broker.CreatedAt,
            UpdatedAt = broker.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var broker = await _brokerRepository.GetByIdAsync(id, cancellationToken);
        if (broker == null) return false;

        await _brokerRepository.DeleteAsync(broker, cancellationToken);
        return true;
    }
}
