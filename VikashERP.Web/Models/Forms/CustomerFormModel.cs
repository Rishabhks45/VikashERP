namespace VikashERP.Web.Models.Forms;

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
