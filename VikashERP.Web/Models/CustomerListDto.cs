using System;

namespace VikashERP.Web.Models;

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
    public string DefaultPaymentMode { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
    public decimal DefaultFreightRate { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateCustomerDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public string DefaultPaymentMode { get; set; } = "Cash";
    public decimal CreditLimit { get; set; } = 0;
    public decimal DefaultFreightRate { get; set; } = 0;
}

public class CreateCustomerResponse
{
    public CustomerListDto Customer { get; set; } = new();
    public bool PortalAccessCreated { get; set; }
}

public class CustomerLedgerEntryDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    public string? Remarks { get; set; }
}
