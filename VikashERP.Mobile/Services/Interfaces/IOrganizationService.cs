using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IOrganizationService
{
    Task<OrganizationResponse?> GetOrganizationAsync();
    Task<OrganizationResponse?> UpdateOrganizationAsync(OrganizationFormModel model);
}
