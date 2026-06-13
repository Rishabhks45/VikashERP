namespace VikashERP.Domain.Entities;

public class ProductSubImage : BaseEntity
{

    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }


    public Product Product { get; set; } = null!;
}
