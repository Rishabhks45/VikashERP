using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface ISalesService
{
    Task<List<InvoiceListDto>> GetInvoicesAsync();
    Task<InvoiceDetailDto?> GetInvoiceByIdAsync(Guid id);
    Task<byte[]?> GetInvoicePdfAsync(Guid id);
}
