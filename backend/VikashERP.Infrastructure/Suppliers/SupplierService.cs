using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Features.Suppliers.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Infrastructure.Suppliers;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(ISupplierRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<SupplierListDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = await _repository.GetAllAsync(cancellationToken);
        return suppliers.Select(s => new SupplierListDto
        {
            Id = s.Id,
            Name = s.Name,
            CompanyName = s.CompanyName,
            Phone = s.Phone,
            CurrentBalance = s.CurrentBalance,
            CreatedAt = s.CreatedAt
        }).OrderByDescending(s => s.CreatedAt).ToList();
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var s = await _repository.GetByIdAsync(id, cancellationToken);
        if (s == null) return null;

        return new SupplierDto
        {
            Id = s.Id,
            Name = s.Name,
            CompanyName = s.CompanyName,
            Phone = s.Phone,
            Gstin = s.Gstin,
            Address = s.Address,
            CurrentBalance = s.CurrentBalance,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        };
    }

    public async Task<SupplierDto?> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        // Trim strings
        var name = dto.Name?.Trim() ?? string.Empty;
        var gstin = dto.Gstin?.Trim();

        // Check duplicates
        var existing = await _repository.GetByNameAsync(name, cancellationToken);
        if (existing != null) return null; // Or throw exception

        if (!string.IsNullOrWhiteSpace(gstin))
        {
            var existingGstin = await _repository.GetByGstinAsync(gstin, cancellationToken);
            if (existingGstin != null) return null; // Duplicate GSTIN
        }

        var supplier = new Supplier
        {
            Name = name,
            CompanyName = dto.CompanyName?.Trim(),
            Phone = dto.Phone?.Trim() ?? string.Empty,
            Gstin = gstin,
            Address = dto.Address?.Trim(),
            CurrentBalance = dto.OpeningBalance
        };

        // If opening balance is not 0, we should probably add a ledger entry, but let's keep it simple for now
        // or we can add it to LedgerEntries
        if (dto.OpeningBalance != 0)
        {
            supplier.LedgerEntries.Add(new SupplierLedger
            {
                TransactionDate = DateTime.UtcNow,
                TransactionType = "Opening Balance",
                Remarks = "Opening Balance",
                Credit = dto.OpeningBalance > 0 ? dto.OpeningBalance : 0,
                Debit = dto.OpeningBalance < 0 ? Math.Abs(dto.OpeningBalance) : 0,
                RunningBalance = dto.OpeningBalance
            });
        }

        await _repository.AddAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(supplier.Id, cancellationToken);
    }

    public async Task<SupplierDto?> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, cancellationToken);
        if (supplier == null) return null;

        var name = dto.Name?.Trim() ?? string.Empty;
        var gstin = dto.Gstin?.Trim();

        if (!supplier.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repository.GetByNameAsync(name, cancellationToken);
            if (existing != null) return null;
        }

        if (!string.IsNullOrWhiteSpace(gstin) && supplier.Gstin != gstin)
        {
            var existingGstin = await _repository.GetByGstinAsync(gstin, cancellationToken);
            if (existingGstin != null) return null;
        }

        supplier.Name = name;
        supplier.CompanyName = dto.CompanyName?.Trim();
        supplier.Phone = dto.Phone?.Trim() ?? string.Empty;
        supplier.Gstin = gstin;
        supplier.Address = dto.Address?.Trim();

        await _repository.UpdateAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(supplier.Id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var supplier = await _repository.GetByIdAsync(id, cancellationToken);
        if (supplier == null) return false;

        // Optionally check if supplier has active transactions before deleting

        await _repository.DeleteAsync(supplier, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
