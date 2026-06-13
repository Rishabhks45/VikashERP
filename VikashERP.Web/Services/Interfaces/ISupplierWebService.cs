using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;
using VikashERP.Web.Models.Forms;

namespace VikashERP.Web.Services.Interfaces;

public interface ISupplierWebService
{
    Task<List<SupplierListDto>> GetSuppliersAsync();
    Task<SupplierDto?> GetSupplierByIdAsync(Guid id);
    Task<SupplierDto?> CreateSupplierAsync(SupplierFormModel model);
    Task<SupplierDto?> UpdateSupplierAsync(Guid id, SupplierFormModel model);
    Task<bool> DeleteSupplierAsync(Guid id);
}