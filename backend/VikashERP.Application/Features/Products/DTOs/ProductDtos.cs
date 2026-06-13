

namespace VikashERP.Application.Features.Products.DTOs;

public class ProductListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public int TotalVariants { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductDto
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public List<ProductVariantDto> Variants { get; set; } = new();
}

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; }
    public int AlertQtyPcs { get; set; }
    public bool IsActive { get; set; }
}

public class CreateProductDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public bool IsActive { get; set; } = true;
    public List<CreateProductVariantDto> Variants { get; set; } = new();
}

public class UpdateProductDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public bool IsActive { get; set; }
    public List<UpdateProductVariantDto> Variants { get; set; } = new();
}

public class CreateProductVariantDto
{
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;
    public bool IsActive { get; set; } = true;
}

public class UpdateProductVariantDto
{
    public Guid? Id { get; set; } // Null if it's a new variant being added during an update
    public string Size { get; set; } = string.Empty;
    public string Thickness { get; set; } = string.Empty;
    public decimal UnitPcsToKg { get; set; } = 1m;
    public int AlertQtyPcs { get; set; } = 10;
    public bool IsActive { get; set; } = true;
}
