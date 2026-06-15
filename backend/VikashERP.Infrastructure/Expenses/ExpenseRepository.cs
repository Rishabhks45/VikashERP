using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Infrastructure.Data;
using VikashERP.Infrastructure.Repositories;

namespace VikashERP.Infrastructure.Expenses;

public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Expense>> GetExpensesAsync(CancellationToken cancellationToken)
    {
        return await _context.Expenses
            .Where(e => !e.IsDeleted)
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync(cancellationToken);
    }
}

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ExpenseService(IExpenseRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateExpenseAsync(Expense expense, CancellationToken cancellationToken)
    {
        if (expense.Amount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.");

        await _repository.AddAsync(expense, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return expense.Id;
    }

    public async Task<bool> UpdateExpenseAsync(Guid id, Expense expense, CancellationToken cancellationToken)
    {
        if (expense.Amount <= 0)
            throw new ArgumentException("Expense amount must be greater than zero.");

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null || existing.IsDeleted) return false;

        existing.ExpenseDate = expense.ExpenseDate;
        existing.Category = expense.Category;
        existing.Amount = expense.Amount;
        existing.PaymentMode = expense.PaymentMode;
        existing.Remarks = expense.Remarks;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.UpdatedBy = expense.UpdatedBy;

        _repository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteExpenseAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null || existing.IsDeleted) return false;

        existing.IsDeleted = true;
        existing.IsActive = false;
        existing.UpdatedAt = DateTime.UtcNow;

        _repository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
