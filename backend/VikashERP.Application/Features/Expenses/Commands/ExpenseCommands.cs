using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Expenses.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Features.Expenses.Commands;

public class CreateExpenseCommand : IRequest<ExpenseDto>
{
    public CreateExpenseDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IExpenseService _expenseService;
    private readonly IExpenseRepository _expenseRepository;

    public CreateExpenseCommandHandler(IExpenseService expenseService, IExpenseRepository expenseRepository)
    {
        _expenseService = expenseService;
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            ExpenseDate = request.Request.ExpenseDate.ToUniversalTime(),
            Category = request.Request.Category.Trim(),
            Amount = request.Request.Amount,
            PaymentMode = request.Request.PaymentMode.Trim(),
            Remarks = request.Request.Remarks?.Trim(),
            CreatedBy = request.UserId
        };

        var id = await _expenseService.CreateExpenseAsync(expense, cancellationToken);
        var created = await _expenseRepository.GetByIdAsync(id, cancellationToken);
        
        if (created == null) throw new Exception("Failed to retrieve created expense.");

        return new ExpenseDto
        {
            Id = created.Id,
            ExpenseDate = created.ExpenseDate,
            Category = created.Category,
            Amount = created.Amount,
            PaymentMode = created.PaymentMode,
            Remarks = created.Remarks,
            CreatedAt = created.CreatedAt,
            IsActive = created.IsActive
        };
    }
}

public class UpdateExpenseCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public UpdateExpenseDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IExpenseService _expenseService;

    public UpdateExpenseCommandHandler(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            ExpenseDate = request.Request.ExpenseDate.ToUniversalTime(),
            Category = request.Request.Category.Trim(),
            Amount = request.Request.Amount,
            PaymentMode = request.Request.PaymentMode.Trim(),
            Remarks = request.Request.Remarks?.Trim(),
            UpdatedBy = request.UserId,
            UpdatedAt = DateTime.UtcNow
        };

        return await _expenseService.UpdateExpenseAsync(request.Id, expense, cancellationToken);
    }
}

public class DeleteExpenseCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
}

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpenseService _expenseService;

    public DeleteExpenseCommandHandler(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        return await _expenseService.DeleteExpenseAsync(request.Id, cancellationToken);
    }
}
