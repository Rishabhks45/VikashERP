using System;
using System.Collections.Generic;

namespace VikashERP.Web.Models;

public class ProductListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public int TotalVariants { get; set; }
    public bool IsActive { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn SellingUnit { get; set; }
    public string? ProductImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn SellingUnit { get; set; }
    public string? ProductImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ProductVariantDto> Variants { get; set; } = new();
    public List<ProductSubImageDto> SubImages { get; set; } = new();
}

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; }
    public int AlertQtyPcs { get; set; }
    public decimal LastPurchaseRate { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn LastPurchaseRateOn { get; set; }
    public decimal DefaultMargin { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn SellingUnit { get; set; } = VikashERP.SharedKernel.Enums.RateOn.Kg;
    public string? ProductImageUrl { get; set; }
    public bool IsActive { get; set; } = true;
    public List<CreateProductVariantDto> Variants { get; set; } = new();
    public List<CreateProductSubImageDto> SubImages { get; set; } = new();
}

public class UpdateProductDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public VikashERP.SharedKernel.Enums.RateOn SellingUnit { get; set; } = VikashERP.SharedKernel.Enums.RateOn.Kg;
    public string? ProductImageUrl { get; set; }
    public bool IsActive { get; set; }
    public List<UpdateProductVariantDto> Variants { get; set; } = new();
    public List<UpdateProductSubImageDto> SubImages { get; set; } = new();
}

public class ProductSubImageDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateProductSubImageDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

public class UpdateProductSubImageDto
{
    public Guid? Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}

public class CreateProductVariantDto
{
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;
    public decimal DefaultMargin { get; set; } = 0m;
    public bool IsActive { get; set; } = true;
}

public class UpdateProductVariantDto
{
    public Guid? Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;
    public decimal DefaultMargin { get; set; } = 0m;
    public bool IsActive { get; set; } = true;
}
