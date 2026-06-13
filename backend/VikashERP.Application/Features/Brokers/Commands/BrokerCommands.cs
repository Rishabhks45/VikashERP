using System;
using MediatR;
using VikashERP.Application.Features.Brokers.DTOs;

namespace VikashERP.Application.Features.Brokers.Commands;

public record CreateBrokerCommand(CreateBrokerDto BrokerDto) : IRequest<BrokerDto?>;

public record UpdateBrokerCommand(Guid Id, UpdateBrokerDto BrokerDto) : IRequest<BrokerDto?>;

public record DeleteBrokerCommand(Guid Id) : IRequest<bool>;
