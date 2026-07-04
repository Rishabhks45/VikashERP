using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IExpenseService
{
    Task<List<ExpenseListDto>> GetExpensesAsync();
    Task<bool> RecordExpenseAsync(ExpenseFormModel model);
    Task<bool> UpdateExpenseAsync(System.Guid id, ExpenseFormModel model);
}
