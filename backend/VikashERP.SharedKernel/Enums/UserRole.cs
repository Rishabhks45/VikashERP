using System.ComponentModel;

namespace VikashERP.SharedKernel.Enums;

/// <summary>
/// Defines the roles available in the system.
/// </summary>
public enum UserRole
{
    [Description("Super Admin")]
    SuperAdmin = 1,

    [Description("Back Office User")]
    BackOfficeUser = 2,

    [Description("Customer")]
    Customer = 3,

    [Description("Employee")]
    Employee = 4,

    [Description("Manager")]
    Manager = 5
}

/// <summary>
/// Extension methods and helpers for the <see cref="UserRole"/> enum.
/// </summary>
public static class UserRoleExtensions
{
    /// <summary>
    /// Returns the user-friendly string representation of the role.
    /// </summary>
    public static string ToFriendlyName(this UserRole role)
    {
        return role switch
        {
            UserRole.SuperAdmin => "Super Admin",
            UserRole.BackOfficeUser => "Back Office User",
            UserRole.Customer => "Customer",
            UserRole.Employee => "Employee",
            UserRole.Manager => "Manager",
            _ => role.ToString()
        };
    }

    /// <summary>
    /// Parses a string into the corresponding <see cref="UserRole"/>, ignoring case and spacing.
    /// </summary>
    public static UserRole? FromString(string? roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return null;

        var normalized = roleName.Replace(" ", "", StringComparison.Ordinal).ToLowerInvariant();
        var fromFriendly = normalized switch
        {
            "superadmin" => UserRole.SuperAdmin,
            "backofficeuser" => UserRole.BackOfficeUser,
            "customer" => UserRole.Customer,
            "employee" => UserRole.Employee,
            "manager" => UserRole.Manager,
            _ => (UserRole?)null
        };

        if (fromFriendly.HasValue)
            return fromFriendly;

        if (Enum.TryParse<UserRole>(roleName.Replace(" ", "", StringComparison.Ordinal), ignoreCase: true, out var parsed))
            return parsed;

        return null;
    }
}
