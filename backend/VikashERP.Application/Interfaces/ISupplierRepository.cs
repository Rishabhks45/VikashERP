using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Supplier?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Supplier?> GetByGstinAsync(string gstin, CancellationToken cancellationToken = default);
    Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default);
    Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default);
    Task DeleteAsync(Supplier supplier, CancellationToken cancellationToken = default);
}
