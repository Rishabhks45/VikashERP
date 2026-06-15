using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Expenses.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Expenses.Queries;

public record GetExpensesQuery : IRequest<IReadOnlyList<ExpenseListDto>>;

public class GetExpensesQueryHandler : IRequestHandler<GetExpensesQuery, IReadOnlyList<ExpenseListDto>>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpensesQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<IReadOnlyList<ExpenseListDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var expenses = await _expenseRepository.GetExpensesAsync(cancellationToken);
        return expenses.Select(e => new ExpenseListDto
        {
            Id = e.Id,
            ExpenseDate = e.ExpenseDate,
            Category = e.Category,
            Amount = e.Amount,
            PaymentMode = e.PaymentMode,
            Remarks = e.Remarks,
            CreatedAt = e.CreatedAt
        }).ToList();
    }
}

public record GetExpenseByIdQuery(Guid Id) : IRequest<ExpenseDto?>;

public class GetExpenseByIdQueryHandler : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    private readonly IExpenseRepository _expenseRepository;

    public GetExpenseByIdQueryHandler(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseDto?> Handle(GetExpenseByIdQuery request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.IsDeleted) return null;

        return new ExpenseDto
        {
            Id = expense.Id,
            ExpenseDate = expense.ExpenseDate,
            Category = expense.Category,
            Amount = expense.Amount,
            PaymentMode = expense.PaymentMode,
            Remarks = expense.Remarks,
            CreatedAt = expense.CreatedAt,
            UpdatedAt = expense.UpdatedAt,
            IsActive = expense.IsActive
        };
    }
}
