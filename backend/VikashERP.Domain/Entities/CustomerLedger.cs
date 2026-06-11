namespace VikashERP.Domain.Entities;

public class CustomerLedger
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string TransactionType { get; set; } = string.Empty;
    public int? ReferenceId { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    public string? Remarks { get; set; }

    public Customer Customer { get; set; } = null!;
}
