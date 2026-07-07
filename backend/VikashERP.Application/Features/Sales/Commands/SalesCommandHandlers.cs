using MediatR;
using VikashERP.Application.Features.Sales.DTOs;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.SharedKernel.Common.Interfaces;

namespace VikashERP.Application.Features.Sales.Commands;

public record CreateInvoiceCommand(CreateInvoiceDto Dto) : IRequest<Guid>;
public record UpdateInvoiceCommand(Guid Id, CreateInvoiceDto Dto) : IRequest<Guid>;
public record ApproveInvoiceCommand(Guid Id) : IRequest<bool>;

public class SalesCommandHandlers : 
    IRequestHandler<CreateInvoiceCommand, Guid>,
    IRequestHandler<UpdateInvoiceCommand, Guid>,
    IRequestHandler<ApproveInvoiceCommand, bool>
{
    private readonly ISalesService _salesService;
    private readonly ISalesRepository _salesRepository;

    public SalesCommandHandlers(
        ISalesService salesService, 
        ISalesRepository salesRepository)
    {
        _salesService = salesService;
        _salesRepository = salesRepository;
    }

    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var invoice = new Invoice
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            Subtotal = dto.Subtotal,
            FreightCharge = dto.FreightCharge,
            LoadingCharge = dto.LoadingCharge,
            CgstAmount = dto.CgstAmount,
            SgstAmount = dto.SgstAmount,
            IgstAmount = dto.IgstAmount,
            RoundingAmount = dto.RoundingAmount,
            TotalAmount = dto.TotalAmount,
            PaidAmount = dto.PaidAmount,
            CashAmount = dto.CashAmount,
            BankAmount = dto.BankAmount,
            DueAmount = dto.DueAmount,
            PaymentMode = dto.PaymentMode,
            VehicleNumber = dto.VehicleNumber,
            Remarks = dto.Remarks,
            InvoiceDate = dto.InvoiceDate == default ? DateTime.UtcNow : DateTime.SpecifyKind(dto.InvoiceDate, DateTimeKind.Utc),
        };

        foreach (var itemDto in dto.Items)
        {
            invoice.Items.Add(new InvoiceItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                VariantId = itemDto.VariantId,
                QtyPcs = itemDto.QtyPcs,
                WeightKg = itemDto.WeightKg,
                Rate = itemDto.Rate,
                RateOn = itemDto.RateOn,
                CgstRate = itemDto.CgstRate,
                SgstRate = itemDto.SgstRate,
                IgstRate = itemDto.IgstRate,
                TotalPrice = itemDto.TotalPrice
            });
        }

        return await _salesService.CreateInvoiceAsync(invoice, cancellationToken);
    }

    public async Task<bool> Handle(ApproveInvoiceCommand request, CancellationToken cancellationToken)
    {
        return await _salesRepository.ApproveInvoiceAsync(request.Id, cancellationToken);
    }

    public async Task<Guid> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Dto;
        var invoice = new Invoice
        {
            Id = request.Id,
            CustomerId = dto.CustomerId,
            Subtotal = dto.Subtotal,
            FreightCharge = dto.FreightCharge,
            LoadingCharge = dto.LoadingCharge,
            CgstAmount = dto.CgstAmount,
            SgstAmount = dto.SgstAmount,
            IgstAmount = dto.IgstAmount,
            RoundingAmount = dto.RoundingAmount,
            TotalAmount = dto.TotalAmount,
            PaidAmount = dto.PaidAmount,
            CashAmount = dto.CashAmount,
            BankAmount = dto.BankAmount,
            DueAmount = dto.DueAmount,
            PaymentMode = dto.PaymentMode,
            VehicleNumber = dto.VehicleNumber,
            Remarks = dto.Remarks,
            InvoiceDate = dto.InvoiceDate == default ? DateTime.UtcNow : DateTime.SpecifyKind(dto.InvoiceDate, DateTimeKind.Utc)
        };

        foreach (var itemDto in dto.Items)
        {
            invoice.Items.Add(new InvoiceItem
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoice.Id,
                VariantId = itemDto.VariantId,
                QtyPcs = itemDto.QtyPcs,
                WeightKg = itemDto.WeightKg,
                Rate = itemDto.Rate,
                RateOn = itemDto.RateOn,
                CgstRate = itemDto.CgstRate,
                SgstRate = itemDto.SgstRate,
                IgstRate = itemDto.IgstRate,
                TotalPrice = itemDto.TotalPrice
            });
        }

        return await _salesService.UpdateInvoiceAsync(request.Id, invoice, cancellationToken);
    }
}
