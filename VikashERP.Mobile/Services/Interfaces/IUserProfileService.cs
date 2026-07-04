using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IUserProfileService
{
    Task<UserAccountDto?> GetProfileAsync(Guid userId);
    Task<(bool IsSuccess, string ErrorMessage)> UpdateProfileAsync(Guid userId, UpdateUserAccountDto profile);
}
