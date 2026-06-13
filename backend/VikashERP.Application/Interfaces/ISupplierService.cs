using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Features.Suppliers.DTOs;

namespace VikashERP.Application.Interfaces;

public interface ISupplierService
{
    Task<List<SupplierListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SupplierDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SupplierDto?> CreateAsync(CreateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<SupplierDto?> UpdateAsync(Guid id, UpdateSupplierDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
