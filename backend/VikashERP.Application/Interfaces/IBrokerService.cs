using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Features.Brokers.DTOs;

namespace VikashERP.Application.Interfaces;

public interface IBrokerService
{
    Task<List<BrokerListDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BrokerDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BrokerDto?> CreateAsync(CreateBrokerDto dto, CancellationToken cancellationToken = default);
    Task<BrokerDto?> UpdateAsync(Guid id, UpdateBrokerDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
