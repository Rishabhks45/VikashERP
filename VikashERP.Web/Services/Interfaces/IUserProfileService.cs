using System;
using VikashERP.Web.Services.Interfaces;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IUserProfileService
{
    event Action<UserProfileDto>? OnProfileUpdated;
    void NotifyProfileUpdated(UserProfileDto profile);
    Task<UserAccountDto?> GetProfileAsync(Guid userId);
    Task<bool> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile);
}
