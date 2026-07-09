using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services;

public interface ISalaryConfigurationService
{
    Task<List<SalaryConfigDto>> GetConfigurationsAsync();
    Task<List<RoleUserDto>> GetUsersByRoleAsync(string roleName);
    Task<bool> CreateConfigurationAsync(CreateSalaryConfigDto request);
    Task<bool> UpdateConfigurationAsync(Guid id, UpdateSalaryConfigDto request);
    Task<bool> DeleteConfigurationAsync(Guid id);
}
