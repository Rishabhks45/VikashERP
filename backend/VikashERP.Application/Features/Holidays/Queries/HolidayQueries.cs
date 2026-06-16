using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Holidays.DTOs;
using VikashERP.Domain.Entities;
using VikashERP.Domain.Interfaces;

namespace VikashERP.Application.Features.Holidays.Queries;

public record GetHolidaysQuery : IRequest<IReadOnlyList<HolidayDto>>;

public class GetHolidaysQueryHandler : IRequestHandler<GetHolidaysQuery, IReadOnlyList<HolidayDto>>
{
    private readonly IRepository<Holiday> _holidayRepository;

    public GetHolidaysQueryHandler(IRepository<Holiday> holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<IReadOnlyList<HolidayDto>> Handle(GetHolidaysQuery request, CancellationToken cancellationToken)
    {
        var holidays = await _holidayRepository.FindAsync(h => !h.IsDeleted, cancellationToken);
        
        return holidays
            .OrderBy(h => h.Date)
            .Select(h => new HolidayDto
            {
                Id = h.Id,
                Name = h.Name,
                Date = h.Date,
                IsRecurring = h.IsRecurring,
                Description = h.Description,
                IsActive = h.IsActive,
                CreatedAt = h.CreatedAt
            })
            .ToList();
    }
}

public record GetHolidayByIdQuery(Guid Id) : IRequest<HolidayDto?>;

public class GetHolidayByIdQueryHandler : IRequestHandler<GetHolidayByIdQuery, HolidayDto?>
{
    private readonly IRepository<Holiday> _holidayRepository;

    public GetHolidayByIdQueryHandler(IRepository<Holiday> holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<HolidayDto?> Handle(GetHolidayByIdQuery request, CancellationToken cancellationToken)
    {
        var h = await _holidayRepository.GetByIdAsync(request.Id, cancellationToken);
        if (h == null || h.IsDeleted) return null;

        return new HolidayDto
        {
            Id = h.Id,
            Name = h.Name,
            Date = h.Date,
            IsRecurring = h.IsRecurring,
            Description = h.Description,
            IsActive = h.IsActive,
            CreatedAt = h.CreatedAt
        };
    }
}
