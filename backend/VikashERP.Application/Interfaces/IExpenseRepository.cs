using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;
using VikashERP.Domain.Interfaces;

namespace VikashERP.Application.Interfaces;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<List<Expense>> GetExpensesAsync(CancellationToken cancellationToken);
}
