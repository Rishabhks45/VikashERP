namespace VikashERP.SharedKernel.Enums;

public static class EmailTemplateTypeExtensions
{
    public static string ToDisplayName(this EmailTemplateType type) => type switch
    {
        EmailTemplateType.ForgotPassword => "Forgot Password",
        EmailTemplateType.Welcome => "Welcome",
        EmailTemplateType.PasswordResetSuccess => "Password Reset Success",
        _ => type.ToString()
    };
}
