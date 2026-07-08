using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface ISalesService
{
    Task<List<InvoiceListDto>> GetInvoicesAsync();
    Task<InvoiceDetailDto?> GetInvoiceByIdAsync(Guid id);
    Task<byte[]?> GetInvoicePdfAsync(Guid id);
    Task<(bool IsSuccess, string ErrorMessage, Guid? InvoiceId)> CreateInvoiceAsync(CreateInvoiceModel invoice);
    Task<(bool IsSuccess, string ErrorMessage)> ApproveInvoiceAsync(Guid id);
}
