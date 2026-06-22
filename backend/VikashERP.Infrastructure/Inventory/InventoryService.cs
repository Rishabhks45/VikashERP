using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Features.Inventory.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Infrastructure.Data;

namespace VikashERP.Infrastructure.Inventory;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GodownStockDto>> GetGodownStockAsync(CancellationToken cancellationToken)
    {
        var variants = await _context.ProductVariants
            .Include(v => v.Product)
                .ThenInclude(p => p.Category)
            .Include(v => v.StockLedgerEntries)
            .ToListAsync(cancellationToken);

        var stockList = new List<GodownStockDto>();

        foreach (var v in variants)
        {
            var latestStock = v.StockLedgerEntries
                .OrderByDescending(s => s.CreatedAt)
                .FirstOrDefault();

            var pcs = latestStock?.RunningPcs ?? 0;
            var weight = latestStock?.RunningWeightKg ?? 0;

            string status = "In Stock";
            bool isOut = v.Product?.SellingUnit == VikashERP.SharedKernel.Enums.RateOn.Kg ? weight <= 0 : pcs <= 0;
            bool isLow = v.Product?.SellingUnit == VikashERP.SharedKernel.Enums.RateOn.Kg ? weight <= v.AlertQtyPcs : pcs <= v.AlertQtyPcs;

            if (isOut) status = "Out of Stock";
            else if (isLow) status = "Low Stock";

            stockList.Add(new GodownStockDto
            {
                VariantId = v.Id,
                CategoryName = v.Product?.Category?.Name ?? "Uncategorized",
                ProductName = v.Product?.Name ?? "Unknown",
                Size = v.Size,
                Thickness = v.Thickness,
                RunningPcs = pcs,
                RunningWeightKg = weight,
                AlertQtyPcs = v.AlertQtyPcs,
                StockStatus = status,
                LastUpdateDate = latestStock?.CreatedAt
            });
        }

        return stockList
            .OrderBy(s => s.CategoryName)
            .ThenBy(s => s.ProductName)
            .ThenBy(s => s.Size)
            .ToList();
    }
}
