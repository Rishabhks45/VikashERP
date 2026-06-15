using System;
using System.Collections.Generic;

namespace VikashERP.Web.Models.Forms;

public class ProductFormModel
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn SellingUnit { get; set; } = VikashERP.SharedKernel.Enums.RateOn.Kg;
    public string? ProductImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public List<ProductVariantFormModel> Variants { get; set; } = new();
    public List<ProductSubImageFormModel> SubImages { get; set; } = new();
}

public class ProductSubImageFormModel
{
    public Guid? Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
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
