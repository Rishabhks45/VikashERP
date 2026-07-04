using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IUserService
{
    Task<List<UserAccountDto>> GetAllAsync();
    Task<UserAccountDto?> GetByIdAsync(Guid id);
    Task<(bool IsSuccess, string ErrorMessage)> CreateAsync(CreateUserAccountDto dto);
    Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Guid id, UpdateUserAccountDto dto);
    Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id);
}
