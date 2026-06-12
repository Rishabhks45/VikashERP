namespace VikashERP.Web.Models.Forms;

public class LoginFormModel
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
