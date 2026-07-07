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
            InvoiceDate = DateTime.SpecifyKind(request.Dto.InvoiceDate, DateTimeKind.Utc),
            LoadingCharge = request.Dto.LoadingCharge,
            FreightCharge = request.Dto.FreightCharge,
            CgstAmount = request.Dto.CgstAmount,
            SgstAmount = request.Dto.SgstAmount,
            RoundingAmount = request.Dto.RoundingAmount,
            Remarks = request.Dto.Remarks,
            VehicleNumber = request.Dto.VehicleNumber,
            Status = PurchaseEntryStatus.Draft
        };

        decimal totalAmt = 0;
        foreach (var itemDto in request.Dto.Items)
        {
            var totalPrice = itemDto.TotalPrice > 0 
                ? itemDto.TotalPrice
                : (itemDto.RateOn == RateOn.Pcs
                    ? itemDto.QuantityPcs * itemDto.Rate
                    : itemDto.WeightKg * itemDto.Rate);
            totalAmt += totalPrice;

            entry.Items.Add(new PurchaseEntryItem
            {
                ProductVariantId = itemDto.ProductVariantId,
                QuantityPcs = itemDto.QuantityPcs,
                WeightKg = itemDto.WeightKg,
                Rate = itemDto.Rate,
                RateOn = itemDto.RateOn,
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

