using System;

namespace VikashERP.Web.Models.Forms;

public class SupplierFormModel
{
    public string Name { get; set; } = string.Empty;
    public string? CompanyName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Gstin { get; set; }
    public string? Address { get; set; }
    public decimal OpeningBalance { get; set; }
    public bool IsNewSupplier { get; set; } = true;
}
