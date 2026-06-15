using System;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IExpenseService
{
    Task<Guid> CreateExpenseAsync(Expense expense, CancellationToken cancellationToken);
    Task<bool> UpdateExpenseAsync(Guid id, Expense expense, CancellationToken cancellationToken);
    Task<bool> DeleteExpenseAsync(Guid id, CancellationToken cancellationToken);
}
