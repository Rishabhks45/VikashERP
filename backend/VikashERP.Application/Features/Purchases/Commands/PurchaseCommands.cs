using MediatR;
using VikashERP.Application.Features.Purchases.DTOs;
using VikashERP.SharedKernel.Common;

namespace VikashERP.Application.Features.Purchases.Commands;

public record CreatePurchaseEntryCommand(CreatePurchaseEntryDto Dto) : IRequest<Guid>;

public record ApprovePurchaseEntryCommand(Guid PurchaseEntryId) : IRequest<bool>;
