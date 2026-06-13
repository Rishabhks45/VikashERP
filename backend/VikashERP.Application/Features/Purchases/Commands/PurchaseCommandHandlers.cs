using MediatR;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Common;
using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Purchases.Commands;

public class CreatePurchaseEntryCommandHandler : IRequestHandler<CreatePurchaseEntryCommand, Guid>
{
    private readonly IPurchaseService _purchaseService;

    public CreatePurchaseEntryCommandHandler(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    public async Task<Guid> Handle(CreatePurchaseEntryCommand request, CancellationToken cancellationToken)
    {
        var entry = new PurchaseEntry
        {
            SupplierId = request.Dto.SupplierId,
            InvoiceNumber = request.Dto.InvoiceNumber,
            InvoiceDate = request.Dto.InvoiceDate.ToUniversalTime(),
            LoadingCharge = request.Dto.LoadingCharge,
            FreightCharge = request.Dto.FreightCharge,
            Remarks = request.Dto.Remarks,
            VehicleNumber = request.Dto.VehicleNumber,
            Status = PurchaseEntryStatus.Draft
        };

        decimal totalAmt = 0;
        foreach (var itemDto in request.Dto.Items)
        {
            var totalPrice = itemDto.WeightKg * itemDto.Rate; // Assuming rate is per Kg. If per piece, it should be adjusted.
            totalAmt += totalPrice;

            entry.Items.Add(new PurchaseEntryItem
            {
                ProductVariantId = itemDto.ProductVariantId,
                QuantityPcs = itemDto.QuantityPcs,
                WeightKg = itemDto.WeightKg,
                Rate = itemDto.Rate,
                TotalPrice = totalPrice
            });
        }

        entry.TotalAmount = totalAmt;

        return await _purchaseService.CreateEntryAsync(entry, cancellationToken);
    }
}

public class ApprovePurchaseEntryCommandHandler : IRequestHandler<ApprovePurchaseEntryCommand, bool>
{
    private readonly IPurchaseRepository _purchaseRepository;

    public ApprovePurchaseEntryCommandHandler(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<bool> Handle(ApprovePurchaseEntryCommand request, CancellationToken cancellationToken)
    {
        return await _purchaseRepository.ApproveEntryAsync(request.PurchaseEntryId, cancellationToken);
    }
}

