using System;
using System.ComponentModel.DataAnnotations;

namespace VikashERP.Web.Models.Brokers;

public class BrokerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public decimal CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class BrokerListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class BrokerFormModel
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string? Address { get; set; }
    
    public decimal OpeningBalance { get; set; }
}
