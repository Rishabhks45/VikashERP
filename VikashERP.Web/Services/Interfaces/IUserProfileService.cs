using System;

namespace VikashERP.Web.Services.Interfaces;

public class UserProfileDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
}

public interface IUserProfileService
{
    event Action<UserProfileDto>? OnProfileUpdated;
    void NotifyProfileUpdated(UserProfileDto profile);
}
