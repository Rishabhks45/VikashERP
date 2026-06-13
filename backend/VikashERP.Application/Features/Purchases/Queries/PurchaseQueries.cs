using MediatR;
using VikashERP.Application.Features.Purchases.DTOs;
using VikashERP.SharedKernel.Common;

namespace VikashERP.Application.Features.Purchases.Queries;

public record GetPurchaseEntriesQuery() : IRequest<List<PurchaseEntryDto>>;

public record GetPurchaseEntryByIdQuery(Guid Id) : IRequest<PurchaseEntryDto?>;

