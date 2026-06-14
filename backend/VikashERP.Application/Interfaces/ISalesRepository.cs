using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Common;

namespace VikashERP.Application.Interfaces;

public interface ISalesRepository : VikashERP.Domain.Interfaces.IRepository<Invoice>
{
    Task<System.Collections.Generic.List<Invoice>> GetAllWithDetailsAsync(CancellationToken cancellationToken);
    Task<Invoice?> GetInvoiceWithDetailsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ApproveInvoiceAsync(Guid id, CancellationToken cancellationToken);
}

public interface ISalesService
{
    Task<Guid> CreateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
}
