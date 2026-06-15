using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IExpenseWebService
{
    Task<List<ExpenseListDto>> GetExpensesAsync();
    Task<ExpenseDto?> GetExpenseByIdAsync(Guid id);
    Task<ExpenseDto?> CreateExpenseAsync(CreateExpenseDto request);
    Task<bool> UpdateExpenseAsync(Guid id, UpdateExpenseDto request);
    Task<bool> DeleteExpenseAsync(Guid id);
}
