using System;

namespace VikashERP.Mobile.Models;

public class CustomerListDto
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public string DefaultPaymentMode { get; set; } = "A/C";
    public decimal CurrentBalance { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal DefaultFreightRate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CustomerFormModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public string DefaultPaymentMode { get; set; } = "A/C";
    public decimal CreditLimit { get; set; }
    public decimal DefaultFreightRate { get; set; }
    public bool IsNewCustomer { get; set; } = true;
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

public class CustomerPaymentFormModel
{
    public Guid? CustomerId { get; set; }
    public CustomerListDto? SelectedCustomer { get; set; }
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = "Cash"; // "Cash", "UPI", "NetBanking", "N/A"
    public string? Remarks { get; set; }
    public DateTime? PaymentDate { get; set; } = DateTime.Today;
}
