using VikashERP.Domain.Entities;
using VikashERP.Domain.Interfaces;

namespace VikashERP.Application.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<string> GenerateAccountNumberAsync(CancellationToken cancellationToken = default);
    Task<Customer?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
}
