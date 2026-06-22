using System;

namespace VikashERP.Web.Models.Brokers;

public class CreateBrokerTransactionDto
{
    public Guid BrokerId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty; // "Payment" or "Commission"
    public string PaymentMode { get; set; } = string.Empty; // "Cash", "UPI", "NetBanking", "N/A"
    public string? Remarks { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}

public class BrokerTransactionResponseDto
{
    public Guid LedgerEntryId { get; set; }
    public Guid BrokerId { get; set; }
    public decimal RemainingBalance { get; set; }
}

public class RecentBrokerTransactionDto
{
    public Guid Id { get; set; }
    public Guid BrokerId { get; set; }
    public string BrokerName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; } = string.Empty; // "Payment" or "Commission"
    public decimal Amount { get; set; }
    public string PaymentMode { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}

public class BrokerLedgerEntryDto
{
    public Guid Id { get; set; }
    public Guid BrokerId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    public string? Remarks { get; set; }
}

public class BrokerTransactionFormModel
{
    public Guid? BrokerId { get; set; }
    public BrokerListDto? SelectedBroker { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = "Payment"; // "Payment" or "Commission"
    public string PaymentMode { get; set; } = "Cash"; // "Cash", "UPI", "NetBanking", "N/A"
    public string? Remarks { get; set; }
    public DateTime? TransactionDate { get; set; } = DateTime.Today;
}
