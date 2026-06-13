using System;
using System.Collections.Generic;

namespace VikashERP.Domain.Entities;

public class Broker : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    
    public decimal CurrentBalance { get; set; }

    public ICollection<BrokerLedger> LedgerEntries { get; set; } = new List<BrokerLedger>();
}
