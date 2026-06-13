using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Common;

namespace VikashERP.Application.Interfaces;

public interface IPurchaseRepository : VikashERP.Domain.Interfaces.IRepository<PurchaseEntry>
{
    Task<System.Collections.Generic.List<PurchaseEntry>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<PurchaseEntry?> GetEntryWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ApproveEntryAsync(Guid id, CancellationToken cancellationToken);
}

public interface IPurchaseService
{
    Task<Guid> CreateEntryAsync(PurchaseEntry entry, CancellationToken cancellationToken);
}



