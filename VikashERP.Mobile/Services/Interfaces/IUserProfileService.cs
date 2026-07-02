using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IUserProfileService
{
    Task<UserAccountDto?> GetProfileAsync(Guid userId);
    Task<bool> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile);
}
