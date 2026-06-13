using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VikashERP.Application.Features.Brokers.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Brokers.Commands;

public class BrokerCommandHandlers : 
    IRequestHandler<CreateBrokerCommand, BrokerDto?>,
    IRequestHandler<UpdateBrokerCommand, BrokerDto?>,
    IRequestHandler<DeleteBrokerCommand, bool>
{
    private readonly IBrokerService _brokerService;

    public BrokerCommandHandlers(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }

    public async Task<BrokerDto?> Handle(CreateBrokerCommand request, CancellationToken cancellationToken)
    {
        return await _brokerService.CreateAsync(request.BrokerDto, cancellationToken);
    }

    public async Task<BrokerDto?> Handle(UpdateBrokerCommand request, CancellationToken cancellationToken)
    {
        return await _brokerService.UpdateAsync(request.Id, request.BrokerDto, cancellationToken);
    }

    public async Task<bool> Handle(DeleteBrokerCommand request, CancellationToken cancellationToken)
    {
        return await _brokerService.DeleteAsync(request.Id, cancellationToken);
    }
}
