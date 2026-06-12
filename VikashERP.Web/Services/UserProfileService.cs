using System;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class UserProfileService : IUserProfileService
{
    public event Action<UserProfileDto>? OnProfileUpdated;

    public void NotifyProfileUpdated(UserProfileDto profile)
    {
        OnProfileUpdated?.Invoke(profile);
    }
}
