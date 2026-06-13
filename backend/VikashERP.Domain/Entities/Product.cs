namespace VikashERP.Domain.Entities;

public class Product : BaseEntity
{

    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? HsnCode { get; set; }
    public string? ProductImageUrl { get; set; }


    public Category Category { get; set; } = null!;
    public ICollection<ProductVariant> Variants { get; set; } = [];
    public ICollection<ProductSubImage> SubImages { get; set; } = [];
}
