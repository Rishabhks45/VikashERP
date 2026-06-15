using System;

namespace VikashERP.Domain.Entities;

public class Expense : BaseEntity
{
    public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}
