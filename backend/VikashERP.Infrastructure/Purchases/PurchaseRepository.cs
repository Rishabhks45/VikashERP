using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.Infrastructure.Repositories;
using VikashERP.SharedKernel.Common;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Purchases;

public class PurchaseRepository : Repository<PurchaseEntry>, IPurchaseRepository
{
    private readonly ApplicationDbContext _context;

    public PurchaseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<System.Collections.Generic.List<PurchaseEntry>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        return await _context.PurchaseEntries
            .Include(x => x.Supplier)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<PurchaseEntry?> GetEntryWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.PurchaseEntries
            .Include(x => x.Supplier)
            .Include(x => x.Items)
                .ThenInclude(i => i.ProductVariant)
                    .ThenInclude(v => v.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ApproveEntryAsync(Guid id, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var entry = await GetEntryWithDetailsAsync(id, cancellationToken);
            if (entry is null) throw new Exception("Error");
            
            if (entry.Status != PurchaseEntryStatus.Draft)
                throw new Exception("Error");

            // 1. Post to Supplier Ledger
            var lastLedger = await _context.SupplierLedgers
                .Where(x => x.SupplierId == entry.SupplierId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            var prevBalance = lastLedger?.RunningBalance ?? 0;
            var newBalance = prevBalance + entry.NetAmount; // Credit to supplier (we owe them)

            var ledgerEntry = new SupplierLedger
            {
                SupplierId = entry.SupplierId,
                
                TransactionType = "Purchase Invoice",
                ReferenceId = entry.Id,
                Credit = entry.NetAmount, // We owe money
                Debit = 0,
                RunningBalance = newBalance,
                Remarks = $"Purchase Invoice {entry.InvoiceNumber} - {entry.Remarks}"
            };
            _context.SupplierLedgers.Add(ledgerEntry);

            // 2. Post to Stock Ledger
            foreach (var item in entry.Items)
            {
                var defaultGodown = await _context.Godowns.FirstOrDefaultAsync(cancellationToken);
            if (defaultGodown == null) throw new Exception("No Godown found in the system. Please create a Godown first.");
            
            var lastStock = await _context.StockLedgers
                    .Where(x => x.VariantId == item.ProductVariantId ) // Assuming central stock for now
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync(cancellationToken);

                var prevStockPcs = lastStock?.RunningPcs ?? 0;
                var prevStockKg = lastStock?.RunningWeightKg ?? 0;

                var stockEntry = new StockLedger
                {
                    VariantId = item.ProductVariantId,
                    GodownId = defaultGodown.Id,
                    
                    TransactionType = "Stock Inward",
                    ReferenceId = entry.Id,
                    QtyPcs = item.QuantityPcs,
                    WeightKg = item.WeightKg,
                    
                    
                    RunningPcs = prevStockPcs + item.QuantityPcs,
                    RunningWeightKg = prevStockKg + item.WeightKg,
                    Remarks = $"Purchase Inward {entry.InvoiceNumber}"
                };
                _context.StockLedgers.Add(stockEntry);

                // Update Last Purchase Rate on the Variant
                if (item.ProductVariant != null)
                {
                    item.ProductVariant.LastPurchaseRate = item.Rate;
                    item.ProductVariant.LastPurchaseRateOn = item.RateOn;
                    _context.ProductVariants.Update(item.ProductVariant);
                }
            }

            // 3. Mark as Approved
            entry.Status = PurchaseEntryStatus.Approved;
            _context.PurchaseEntries.Update(entry);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return true;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new Exception($"Failed to approve entry: {ex.Message}");
        }
    }
}




