using System;
using System.Collections.Generic;
using MediatR;
using VikashERP.Application.Features.Brokers.DTOs;

namespace VikashERP.Application.Features.Brokers.Queries;

public record GetBrokersQuery() : IRequest<List<BrokerListDto>>;

public record GetBrokerByIdQuery(Guid Id) : IRequest<BrokerDto?>;
