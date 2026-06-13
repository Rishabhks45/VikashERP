using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Features.Purchases.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Purchases.Queries;

public class GetPurchaseEntriesQueryHandler : IRequestHandler<GetPurchaseEntriesQuery, List<PurchaseEntryDto>>
{
    private readonly IPurchaseRepository _repository;

    public GetPurchaseEntriesQueryHandler(IPurchaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PurchaseEntryDto>> Handle(GetPurchaseEntriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllWithDetailsAsync(cancellationToken);
        var entries = result.Select(x => new PurchaseEntryDto
        {
            Id = x.Id,
            EntryNumber = x.EntryNumber,
            SupplierId = x.SupplierId,
            SupplierName = x.Supplier.Name,
            InvoiceNumber = x.InvoiceNumber,
            InvoiceDate = x.InvoiceDate,
            TotalAmount = x.TotalAmount,
            LoadingCharge = x.LoadingCharge,
            FreightCharge = x.FreightCharge,
            NetAmount = x.NetAmount,
            Status = x.Status,
            Remarks = x.Remarks,
            VehicleNumber = x.VehicleNumber,
            CreatedAt = x.CreatedAt
        }).ToList();

        return entries;
    }
}

public class GetPurchaseEntryByIdQueryHandler : IRequestHandler<GetPurchaseEntryByIdQuery, PurchaseEntryDto?>
{
    private readonly IPurchaseRepository _repository;

    public GetPurchaseEntryByIdQueryHandler(IPurchaseRepository repository)
    {
        _repository = repository;
    }

    public async Task<PurchaseEntryDto?> Handle(GetPurchaseEntryByIdQuery request, CancellationToken cancellationToken)
    {
        var entry = await _repository.GetEntryWithDetailsAsync(request.Id, cancellationToken);
        if (entry is null) return null;

        var dto = new PurchaseEntryDto
        {
            Id = entry.Id,
            EntryNumber = entry.EntryNumber,
            SupplierId = entry.SupplierId,
            SupplierName = entry.Supplier.Name,
            InvoiceNumber = entry.InvoiceNumber,
            InvoiceDate = entry.InvoiceDate,
            TotalAmount = entry.TotalAmount,
            LoadingCharge = entry.LoadingCharge,
            FreightCharge = entry.FreightCharge,
            NetAmount = entry.NetAmount,
            Status = entry.Status,
            Remarks = entry.Remarks,
            VehicleNumber = entry.VehicleNumber,
            CreatedAt = entry.CreatedAt,
            Items = entry.Items.Select(i => new PurchaseEntryItemDto
            {
                Id = i.Id,
                ProductVariantId = i.ProductVariantId,
                ProductName = i.ProductVariant.Product.Name,
                VariantSize = i.ProductVariant.Size,
                QuantityPcs = i.QuantityPcs,
                WeightKg = i.WeightKg,
                Rate = i.Rate,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        return dto;
    }
}
