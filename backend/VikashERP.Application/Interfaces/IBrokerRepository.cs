using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IBrokerRepository
{
    Task<List<Broker>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Broker?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Broker?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task AddAsync(Broker broker, CancellationToken cancellationToken = default);
    Task UpdateAsync(Broker broker, CancellationToken cancellationToken = default);
    Task DeleteAsync(Broker broker, CancellationToken cancellationToken = default);
}
