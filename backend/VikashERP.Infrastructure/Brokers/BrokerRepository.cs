using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Brokers;

public class BrokerRepository : IBrokerRepository
{
    private readonly ApplicationDbContext _context;

    public BrokerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Broker>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<Broker?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Brokers
            .FirstOrDefaultAsync(b => b.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task AddAsync(Broker broker, CancellationToken cancellationToken = default)
    {
        await _context.Brokers.AddAsync(broker, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Broker broker, CancellationToken cancellationToken = default)
    {
        _context.Brokers.Update(broker);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Broker broker, CancellationToken cancellationToken = default)
    {
        _context.Brokers.Remove(broker);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
