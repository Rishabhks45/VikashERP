using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface ISalaryConfigurationService
{
    Task<List<SalaryConfigDto>> GetConfigurationsAsync();
    Task<List<RoleUserDto>> GetUsersByRoleAsync(string roleName);
    Task<bool> CreateConfigurationAsync(CreateSalaryConfigDto dto);
    Task<bool> UpdateConfigurationAsync(Guid id, UpdateSalaryConfigDto dto);
    Task<bool> DeleteConfigurationAsync(Guid id);
}
