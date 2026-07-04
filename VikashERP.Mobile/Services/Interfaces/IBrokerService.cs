using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IBrokerService
{
    Task<List<BrokerListDto>> GetBrokersAsync();
    Task<List<RecentBrokerTransactionDto>> GetRecentTransactionsAsync(int count = 50);
    Task<BrokerTransactionResponseDto?> RecordTransactionAsync(BrokerTransactionFormModel model);
}
