using VikashERP.SharedKernel.Enums;

namespace VikashERP.Application.Features.Sales.DTOs;

public class CreateInvoiceItemDto
{
    public Guid VariantId { get; set; }
    public int QtyPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Rate { get; set; }
    public RateOn RateOn { get; set; }
    public decimal CgstRate { get; set; } = 9m;
    public decimal SgstRate { get; set; } = 9m;
    public decimal IgstRate { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CreateInvoiceDto
{
    public Guid CustomerId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal CgstAmount { get; set; }
    public decimal SgstAmount { get; set; }
    public decimal IgstAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal CashAmount { get; set; }
    public decimal BankAmount { get; set; }
    public decimal DueAmount { get; set; }
    public string PaymentMode { get; set; } = "A/C";
    public string? VehicleNumber { get; set; }
    public string? Remarks { get; set; }
    public DateTime InvoiceDate { get; set; }
    
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}

public class InvoiceListDto
{
    public Guid Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal FreightCharge { get; set; }
    public decimal LoadingCharge { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public SalesInvoiceStatus Status { get; set; }
    public int ItemCount { get; set; }
    public decimal TotalWeightKg { get; set; }
    public decimal CashAmount { get; set; }
    public decimal BankAmount { get; set; }
    public decimal DueAmount { get; set; }
}

public class InvoiceDetailDto : InvoiceListDto
{
    public decimal CgstAmount { get; set; }
    public decimal SgstAmount { get; set; }
    public decimal IgstAmount { get; set; }
    public decimal RoundingAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? VehicleNumber { get; set; }
    public string? Remarks { get; set; }
    public List<InvoiceItemDetailDto> Items { get; set; } = new();
}

public class InvoiceItemDetailDto
{
    public Guid Id { get; set; }
    public Guid VariantId { get; set; }
    public Guid ProductId { get; set; }
    public string VariantName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string HsnCode { get; set; } = string.Empty;
    public int QtyPcs { get; set; }
    public decimal WeightKg { get; set; }
    public decimal Rate { get; set; }
    public string RateOn { get; set; } = string.Empty;
    public decimal CgstRate { get; set; }
    public decimal SgstRate { get; set; }
    public decimal TotalPrice { get; set; }
}

public class UpdateInvoicePaymentDto
{
    public decimal CashAmount { get; set; }
    public decimal BankAmount { get; set; }
}
