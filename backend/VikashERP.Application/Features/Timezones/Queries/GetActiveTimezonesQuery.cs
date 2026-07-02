using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Timezones.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Timezones.Queries;

public record GetActiveTimezonesQuery() : IRequest<List<TimezoneDto>>;

public class GetActiveTimezonesQueryHandler : IRequestHandler<GetActiveTimezonesQuery, List<TimezoneDto>>
{
    private readonly ITimezoneRepository _repository;

    public GetActiveTimezonesQueryHandler(ITimezoneRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TimezoneDto>> Handle(GetActiveTimezonesQuery request, CancellationToken cancellationToken)
    {
        var timezones = await _repository.GetActiveTimezonesAsync(cancellationToken);
        
        return timezones
            .OrderBy(t => t.DisplayName)
            .Select(t => new TimezoneDto
            {
                TimezoneId = t.Id,
                IanaId = t.IanaId,
                DisplayName = t.DisplayName,
                Abbreviation = t.Abbreviation,
                IsDefault = t.IsDefault
            })
            .ToList();
    }
}
