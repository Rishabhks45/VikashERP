using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IPurchaseWebService
{
    Task<List<PurchaseEntryDto>> GetPurchaseEntriesAsync();
    Task<PurchaseEntryDto?> GetPurchaseEntryByIdAsync(Guid id);
    Task<Guid> CreatePurchaseEntryAsync(CreatePurchaseEntryDto dto);
    Task<bool> ApprovePurchaseEntryAsync(Guid id);
}

