using System;
using System.Collections.Generic;

namespace VikashERP.Web.Models.Forms;

public class ProductFormModel
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public bool IsActive { get; set; } = true;
    public List<ProductVariantFormModel> Variants { get; set; } = new();
}

public class ProductVariantFormModel
{
    public Guid? Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;
    public bool IsActive { get; set; } = true;
}
