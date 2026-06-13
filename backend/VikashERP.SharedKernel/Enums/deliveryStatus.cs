using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VikashERP.SharedKernel.Enums;

// Delivery Status Enum  ---'PENDING', 'LOADED', 'IN_TRANSIT', 'DELIVERED', 'CANCELLED' ---

public enum DeliveryStatus
{
    [Description("Pending")]
    PENDING = 1,
    [Description("Loaded")]
    LOADED = 2,
    [Description("In Transit")]
    IN_TRANSIT = 3,
    [Description("Delivered")]
    DELIVERED = 4,
    [Description("Cancelled")]
    CANCELLED = 5
}