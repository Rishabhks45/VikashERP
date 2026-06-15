using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.Infrastructure.Repositories;
using VikashERP.SharedKernel.Common;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Infrastructure.Sales;

public class SalesRepository : Repository<Invoice>, ISalesRepository
{
    private readonly ApplicationDbContext _context;

    public SalesRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<System.Collections.Generic.List<Invoice>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
    {
        return await _context.Invoices
            .Include(x => x.Customer)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Invoice?> GetInvoiceWithDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Invoices
            .Include(x => x.Customer)
            .Include(x => x.Items)
                .ThenInclude(i => i.Variant)
                    .ThenInclude(v => v.Product)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> ApproveInvoiceAsync(Guid id, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var rawInvoice = await _context.Invoices.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (rawInvoice == null) 
                throw new Exception($"CRITICAL: Invoice with ID {id} does not exist in the database!");

            var invoice = await GetInvoiceWithDetailsAsync(id, cancellationToken);
            if (invoice is null) 
                throw new Exception($"Invoice {id} exists, but GetInvoiceWithDetailsAsync returned null. This means an INNER JOIN failed (e.g. missing Customer, missing Product, or IsDeleted=true somewhere).");
            
            if (invoice.Status != SalesInvoiceStatus.Draft)
                throw new Exception("Invoice is not in Draft state");

            // 1. Post to Customer Ledger
            var lastLedger = await _context.CustomerLedgers
                .Where(x => x.CustomerId == invoice.CustomerId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            var prevBalance = lastLedger?.RunningBalance ?? 0;
            var newBalance = prevBalance + invoice.TotalAmount; // Debit to customer (they owe us)

            var ledgerEntry = new CustomerLedger
            {
                CustomerId = invoice.CustomerId,
                TransactionType = "Sales Invoice",
                ReferenceId = invoice.Id,
                Debit = invoice.TotalAmount, // They owe money
                Credit = 0,
                RunningBalance = newBalance,
                Remarks = $"Sales Invoice {invoice.InvoiceNumber} - {invoice.Remarks}"
            };
            _context.CustomerLedgers.Add(ledgerEntry);
            
            // Also update customer's current balance
            invoice.Customer.CurrentBalance = newBalance;

            // 2. Post to Stock Ledger (Outward)
            foreach (var item in invoice.Items)
            {
                var defaultGodown = await _context.Godowns.FirstOrDefaultAsync(cancellationToken);
                if (defaultGodown == null) throw new Exception("No Godown found in the system. Please create a Godown first.");
            
                var lastStock = await _context.StockLedgers
                    .Where(x => x.VariantId == item.VariantId)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync(cancellationToken);

                var prevStockPcs = lastStock?.RunningPcs ?? 0;
                var prevStockKg = lastStock?.RunningWeightKg ?? 0;

                var stockEntry = new StockLedger
                {
                    VariantId = item.VariantId,
                    GodownId = defaultGodown.Id,
                    TransactionType = "Stock Outward",
                    ReferenceId = invoice.Id,
                    QtyPcs = -item.QtyPcs, // Negative for outward
                    WeightKg = -item.WeightKg, // Negative for outward
                    RunningPcs = prevStockPcs - item.QtyPcs,
                    RunningWeightKg = prevStockKg - item.WeightKg,
                    Remarks = $"Sales Invoice {invoice.InvoiceNumber}"
                };
                _context.StockLedgers.Add(stockEntry);
            }

            invoice.Status = SalesInvoiceStatus.Approved;
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return true;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

public class SalesService : ISalesService
{
    private readonly ISalesRepository _repository;
    private readonly ISharedRepository _sharedRepository;
    private readonly ApplicationDbContext _context;

    public SalesService(ISalesRepository repository, ISharedRepository sharedRepository, ApplicationDbContext context)
    {
        _repository = repository;
        _sharedRepository = sharedRepository;
        _context = context;
    }

    public async Task<Guid> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        var nextId = await _sharedRepository.GetNextSequenceValueAsync("sales_invoice_seq");
        invoice.InvoiceNumber = $"INV/{DateTime.UtcNow.Year}/{(nextId).ToString().PadLeft(5, '0')}";
        
        await _repository.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }

    public async Task<Guid> UpdateInvoiceAsync(Guid id, Invoice updatedInvoice, CancellationToken cancellationToken)
    {
        var existingInvoice = await _context.Invoices.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        if (existingInvoice == null) throw new Exception("Invoice not found.");

        if (existingInvoice.Status != VikashERP.SharedKernel.Enums.SalesInvoiceStatus.Draft)
            throw new Exception("Only Draft invoices can be edited.");

        existingInvoice.CustomerId = updatedInvoice.CustomerId;
        existingInvoice.Subtotal = updatedInvoice.Subtotal;
        existingInvoice.FreightCharge = updatedInvoice.FreightCharge;
        existingInvoice.LoadingCharge = updatedInvoice.LoadingCharge;
        existingInvoice.CgstAmount = updatedInvoice.CgstAmount;
        existingInvoice.SgstAmount = updatedInvoice.SgstAmount;
        existingInvoice.IgstAmount = updatedInvoice.IgstAmount;
        existingInvoice.RoundingAmount = updatedInvoice.RoundingAmount;
        existingInvoice.TotalAmount = updatedInvoice.TotalAmount;
        existingInvoice.PaidAmount = updatedInvoice.PaidAmount;
        existingInvoice.CashAmount = updatedInvoice.CashAmount;
        existingInvoice.BankAmount = updatedInvoice.BankAmount;
        existingInvoice.DueAmount = updatedInvoice.DueAmount;
        existingInvoice.PaymentMode = updatedInvoice.PaymentMode;
        existingInvoice.VehicleNumber = updatedInvoice.VehicleNumber;
        existingInvoice.Remarks = updatedInvoice.Remarks;
        existingInvoice.InvoiceDate = updatedInvoice.InvoiceDate;
        existingInvoice.UpdatedAt = DateTime.UtcNow;

        // Delete existing items directly to avoid navigation property tracking issues
        var existingItems = await _context.InvoiceItems.Where(i => i.InvoiceId == id).ToListAsync(cancellationToken);
        _context.InvoiceItems.RemoveRange(existingItems);
        
        // Add new items directly
        foreach (var item in updatedInvoice.Items)
        {
            item.InvoiceId = id;
            _context.InvoiceItems.Add(item);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return id;
    }
}
