using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VikashERP.Domain.Entities;

[Table("timezones")]
public class Timezone : BaseEntity
{
    [Column("iana_id")]
    public string IanaId { get; set; } = string.Empty;

    [Column("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [Column("abbreviation")]
    public string? Abbreviation { get; set; }

    [Column("is_default")]
    public bool IsDefault { get; set; } = false;
}
