namespace VikashERP.Web.Models.Forms;

public class CustomerShopFormModel
{
    public string CompanyName { get; set; } = string.Empty;
    public string Gstin { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string DefaultPaymentMode { get; set; } = "A/C";
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }
}
