namespace VikashERP.Web.Models;

public class CustomerShopProfileModel
{
    public Guid Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public string DefaultPaymentMode { get; set; } = "A/C";
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
}

public class CustomerShopSaveModel
{
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public string DefaultPaymentMode { get; set; } = "A/C";
}

public class CustomerShopResult
{
    public bool Success { get; init; }
    public bool NotLinked { get; init; }
    public string? ErrorMessage { get; init; }
    public CustomerShopProfileModel? Profile { get; init; }

    public static CustomerShopResult Ok(CustomerShopProfileModel profile) =>
        new() { Success = true, Profile = profile };

    public static CustomerShopResult NotLinkedResult(string message) =>
        new() { NotLinked = true, ErrorMessage = message };

    public static CustomerShopResult Fail(string message) =>
        new() { ErrorMessage = message };
}
