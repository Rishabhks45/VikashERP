using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Holidays.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Domain.Interfaces;


namespace VikashERP.Application.Features.Holidays.Commands;

public class CreateHolidayCommand : IRequest<HolidayDto>
{
    public CreateHolidayDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class CreateHolidayCommandHandler : IRequestHandler<CreateHolidayCommand, HolidayDto>
{
    private readonly IHolidayService _holidayService;
    private readonly IRepository<Holiday> _holidayRepository;

    public CreateHolidayCommandHandler(IHolidayService holidayService, IRepository<Holiday> holidayRepository)
    {
        _holidayService = holidayService;
        _holidayRepository = holidayRepository;
    }

    public async Task<HolidayDto> Handle(CreateHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = new Holiday
        {
            Name = request.Request.Name.Trim(),
            Date = request.Request.Date,
            IsRecurring = request.Request.IsRecurring,
            Description = request.Request.Description?.Trim(),
            CreatedBy = request.UserId
        };

        var id = await _holidayService.CreateHolidayAsync(holiday, cancellationToken);
        var created = await _holidayRepository.GetByIdAsync(id, cancellationToken);
        if (created == null) throw new Exception("Failed to retrieve created holiday.");

        return new HolidayDto
        {
            Id = created.Id,
            Name = created.Name,
            Date = created.Date,
            IsRecurring = created.IsRecurring,
            Description = created.Description,
            IsActive = created.IsActive,
            CreatedAt = created.CreatedAt
        };
    }
}

public class UpdateHolidayCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public UpdateHolidayDto Request { get; set; } = null!;
    public Guid? UserId { get; set; }
}

public class UpdateHolidayCommandHandler : IRequestHandler<UpdateHolidayCommand, bool>
{
    private readonly IHolidayService _holidayService;

    public UpdateHolidayCommandHandler(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    public async Task<bool> Handle(UpdateHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = new Holiday
        {
            Name = request.Request.Name.Trim(),
            Date = request.Request.Date,
            IsRecurring = request.Request.IsRecurring,
            Description = request.Request.Description?.Trim(),
            IsActive = request.Request.IsActive,
            UpdatedBy = request.UserId
        };

        return await _holidayService.UpdateHolidayAsync(request.Id, holiday, cancellationToken);
    }
}

public class DeleteHolidayCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
}

public class DeleteHolidayCommandHandler : IRequestHandler<DeleteHolidayCommand, bool>
{
    private readonly IHolidayService _holidayService;

    public DeleteHolidayCommandHandler(IHolidayService holidayService)
    {
        _holidayService = holidayService;
    }

    public async Task<bool> Handle(DeleteHolidayCommand request, CancellationToken cancellationToken)
    {
        return await _holidayService.DeleteHolidayAsync(request.Id, cancellationToken);
    }
}
