using VikashERP.SharedKernel.Enums;

namespace VikashERP.Domain.Entities;

public class Customer : BaseEntity
{

    public string AccountNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public CustomerPaymentMode DefaultPaymentMode { get; set; } = CustomerPaymentMode.Account;
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }


    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public string FullName => $"{FirstName} {LastName}".Trim();

    public ICollection<Invoice> Invoices { get; set; } = [];
    public ICollection<CustomerLedger> LedgerEntries { get; set; } = [];
    public ICollection<UserCustomerMapping> UserMappings { get; set; } = [];
}
