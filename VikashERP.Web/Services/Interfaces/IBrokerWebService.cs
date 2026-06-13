using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models.Brokers;

namespace VikashERP.Web.Services.Interfaces;

public interface IBrokerWebService
{
    Task<List<BrokerListDto>> GetBrokersAsync();
    Task<BrokerDto?> GetBrokerByIdAsync(Guid id);
    Task<BrokerDto?> CreateBrokerAsync(BrokerFormModel model);
    Task<BrokerDto?> UpdateBrokerAsync(Guid id, BrokerFormModel model);
    Task<bool> DeleteBrokerAsync(Guid id);
}
