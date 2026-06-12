namespace VikashERP.Web.Models.Forms;

public class UserFormModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Password { get; set; }
    public bool IsNewUser { get; set; } = true;
    public bool IsActive { get; set; } = true;
}
