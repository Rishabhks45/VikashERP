using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface ICustomerShopService
{
    Task<CustomerShopResult> LoadMyShopAsync(CancellationToken cancellationToken = default);

    Task<CustomerShopResult> SaveMyShopAsync(CustomerShopSaveModel model, CancellationToken cancellationToken = default);
}
