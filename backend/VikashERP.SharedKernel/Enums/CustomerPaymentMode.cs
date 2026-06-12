using System.ComponentModel;

namespace VikashERP.SharedKernel.Enums;

/// <summary>
/// Default billing / settlement mode for a customer master record.
/// </summary>
public enum CustomerPaymentMode
{
    [Description("Cash")]
    Cash = 1,

    [Description("A/C")]
    Account = 2
}

public static class CustomerPaymentModeExtensions
{
    public static string ToStorageValue(this CustomerPaymentMode mode) => mode switch
    {
        CustomerPaymentMode.Cash => "Cash",
        CustomerPaymentMode.Account => "A/C",
        _ => mode.ToString()
    };

    public static string ToFriendlyName(this CustomerPaymentMode mode) => mode.ToStorageValue();

    public static CustomerPaymentMode? FromString(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        var normalized = value.Trim().Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
        return normalized switch
        {
            "cash" => CustomerPaymentMode.Cash,
            "a/c" or "ac" or "account" => CustomerPaymentMode.Account,
            _ => Enum.TryParse<CustomerPaymentMode>(value, ignoreCase: true, out var parsed) ? parsed : null
        };
    }
}
