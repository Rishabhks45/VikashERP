using System;

namespace VikashERP.Domain.Entities;

public class BrokerLedger : BaseEntity
{
    public Guid BrokerId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public string TransactionType { get; set; } = string.Empty;
    public Guid? ReferenceId { get; set; }
    
    // Debit means payment made to the broker
    public decimal Debit { get; set; }
    // Credit means brokerage earned by the broker
    public decimal Credit { get; set; }
    public decimal RunningBalance { get; set; }
    
    public string? Remarks { get; set; }

    public Broker Broker { get; set; } = null!;
}
