using System;

namespace VikashERP.Application.Features.Customers.DTOs;

public class CreateCustomerPaymentDto
{
    public Guid CustomerId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
}

public class CustomerPaymentResponseDto
{
    public Guid LedgerEntryId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal RemainingBalance { get; set; }
}

public class RecentCustomerPaymentDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}
