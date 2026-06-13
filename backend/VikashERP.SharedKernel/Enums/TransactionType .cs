using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VikashERP.SharedKernel.Enums;

//transaction_type ` Enum  'PURCHASE', 'PAYMENT', 'RETURN', 'ADJUSTMENT'

public enum TransactionType
{
    [Description("Purchase")]
    PURCHASE = 1,
    [Description("Payment")]
    PAYMENT = 2,
    [Description("Return")]
    RETURN = 3,
    [Description("Adjustment")]
    ADJUSTMENT = 4
}

