using System;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Domain.Entities;

namespace VikashERP.Application.Interfaces;

public interface IHolidayService
{
    Task<Guid> CreateHolidayAsync(Holiday holiday, CancellationToken cancellationToken);
    Task<bool> UpdateHolidayAsync(Guid id, Holiday holiday, CancellationToken cancellationToken);
    Task<bool> DeleteHolidayAsync(Guid id, CancellationToken cancellationToken);
}
