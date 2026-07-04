using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface ICustomerService
{
    Task<List<CustomerListDto>> GetAllAsync();
    Task<CustomerListDto?> GetByIdAsync(Guid id);
    Task<(bool IsSuccess, string ErrorMessage)> CreateAsync(CustomerFormModel model);
    Task<(bool IsSuccess, string ErrorMessage)> UpdateAsync(Guid id, CustomerFormModel model);
    Task<(bool IsSuccess, string ErrorMessage)> DeleteAsync(Guid id);
    Task<List<RecentCustomerPaymentDto>> GetRecentPaymentsAsync();
    Task<bool> RecordCustomerPaymentAsync(CustomerPaymentFormModel model);
}
