using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Brokers.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Brokers.Queries;

public class BrokerQueryHandlers : 
    IRequestHandler<GetBrokersQuery, List<BrokerListDto>>,
    IRequestHandler<GetBrokerByIdQuery, BrokerDto?>
{
    private readonly IBrokerService _brokerService;

    public BrokerQueryHandlers(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }

    public async Task<List<BrokerListDto>> Handle(GetBrokersQuery request, CancellationToken cancellationToken)
    {
        return await _brokerService.GetAllAsync(cancellationToken);
    }

    public async Task<BrokerDto?> Handle(GetBrokerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _brokerService.GetByIdAsync(request.Id, cancellationToken);
    }
}
