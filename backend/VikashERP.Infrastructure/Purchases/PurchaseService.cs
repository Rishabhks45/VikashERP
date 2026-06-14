using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.SharedKernel.Common;

namespace VikashERP.Infrastructure.Purchases;

public class PurchaseService : IPurchaseService
{
    private readonly ApplicationDbContext _context;

    public PurchaseService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateEntryAsync(PurchaseEntry entry, CancellationToken cancellationToken)
    {
        var count = await _context.PurchaseEntries.CountAsync(cancellationToken);
        entry.EntryNumber = $"PE-{DateTime.UtcNow.Year}-{count + 1:D5}";

        _context.PurchaseEntries.Add(entry);
        await _context.SaveChangesAsync(cancellationToken);

        return entry.Id;
    }
}