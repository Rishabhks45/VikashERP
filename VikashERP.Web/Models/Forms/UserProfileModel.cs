using System;

namespace VikashERP.Web.Models.Forms;

public class UserProfileModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Location { get; set; } = "India";
    public string Bio { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
}
