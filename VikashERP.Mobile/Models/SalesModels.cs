using System;

namespace VikashERP.Mobile.Models;

public enum SalesInvoiceStatus
{
    Draft = 0,
    Approved = 1,
    Cancelled = 2
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
    public System.Collections.Generic.List<InvoiceItemDetailDto> Items { get; set; } = new();
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
