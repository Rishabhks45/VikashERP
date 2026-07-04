using System;

namespace VikashERP.Mobile.Models;

public class ExpenseListDto
{
    public Guid Id { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ExpenseFormModel
{
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
    public string PaymentMode { get; set; } = "Cash"; // "Cash", "UPI", "NetBanking"
    public string? Remarks { get; set; }
    public DateTime? ExpenseDate { get; set; } = DateTime.Today;
}
