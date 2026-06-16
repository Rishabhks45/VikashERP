using MediatR;
using VikashERP.Application.Features.Sales.DTOs;
using VikashERP.Application.Interfaces;

namespace VikashERP.Application.Features.Sales.Queries;

public record GetInvoicesQuery : IRequest<List<InvoiceListDto>>;
public record GetInvoiceByIdQuery(Guid Id) : IRequest<InvoiceDetailDto?>;

public class SalesQueryHandlers :
    IRequestHandler<GetInvoicesQuery, List<InvoiceListDto>>,
    IRequestHandler<GetInvoiceByIdQuery, InvoiceDetailDto?>
{
    private readonly ISalesRepository _salesRepository;

    public SalesQueryHandlers(ISalesRepository salesRepository)
    {
        _salesRepository = salesRepository;
    }

    public async Task<List<InvoiceListDto>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        var invoices = await _salesRepository.GetAllWithDetailsAsync(cancellationToken);
        
        return invoices.Select(i => new InvoiceListDto
        {
            Id = i.Id,
            InvoiceNumber = i.InvoiceNumber,
            CustomerId = i.CustomerId,
            CustomerName = i.Customer?.CompanyName ?? $"{i.Customer?.FirstName} {i.Customer?.LastName}",
            Subtotal = i.Subtotal,
            FreightCharge = i.FreightCharge,
            LoadingCharge = i.LoadingCharge,
            TotalAmount = i.TotalAmount,
            InvoiceDate = i.InvoiceDate,
            Status = i.Status,
            ItemCount = i.Items?.Count ?? 0,
            TotalWeightKg = i.Items?.Sum(item => item.WeightKg) ?? 0,
            CashAmount = i.CashAmount,
            BankAmount = i.BankAmount,
            DueAmount = i.DueAmount
        }).ToList();
    }

    public async Task<InvoiceDetailDto?> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        var i = await _salesRepository.GetInvoiceWithDetailsAsync(request.Id, cancellationToken);
        if (i == null) return null;

        return new InvoiceDetailDto
        {
            Id = i.Id,
            InvoiceNumber = i.InvoiceNumber,
            CustomerId = i.CustomerId,
            CustomerName = i.Customer?.CompanyName ?? $"{i.Customer?.FirstName} {i.Customer?.LastName}",
            Subtotal = i.Subtotal,
            FreightCharge = i.FreightCharge,
            LoadingCharge = i.LoadingCharge,
            CgstAmount = i.CgstAmount,
            SgstAmount = i.SgstAmount,
            IgstAmount = i.IgstAmount,
            RoundingAmount = i.RoundingAmount,
            TotalAmount = i.TotalAmount,
            PaidAmount = i.PaidAmount,
            CashAmount = i.CashAmount,
            BankAmount = i.BankAmount,
            DueAmount = i.DueAmount,
            PaymentMode = i.PaymentMode,
            VehicleNumber = i.VehicleNumber,
            Remarks = i.Remarks,
            InvoiceDate = i.InvoiceDate,
            Status = i.Status,
            ItemCount = i.Items?.Count ?? 0,
            Items = i.Items?.Select(item => new InvoiceItemDetailDto
            {
                Id = item.Id,
                VariantId = item.VariantId,
                ProductId = item.Variant?.ProductId ?? Guid.Empty,
                VariantName = $"{item.Variant?.Product?.Name} - {item.Variant?.Size} {item.Variant?.Thickness}",
                CategoryName = item.Variant?.Product?.Category?.Name ?? "N/A",
                HsnCode = item.Variant?.Product?.HsnCode ?? string.Empty,
                QtyPcs = item.QtyPcs,
                WeightKg = item.WeightKg,
                Rate = item.Rate,
                RateOn = item.RateOn.ToString(),
                CgstRate = item.CgstRate,
                SgstRate = item.SgstRate,
                TotalPrice = item.TotalPrice
            }).ToList() ?? new List<InvoiceItemDetailDto>()
        };
    }
}
