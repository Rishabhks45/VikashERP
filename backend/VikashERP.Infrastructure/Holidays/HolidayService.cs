using System;
using System.Threading;
using System.Threading.Tasks;
using VikashERP.Application.Interfaces;
using VikashERP.Domain.Entities;
using VikashERP.Domain.Interfaces;

namespace VikashERP.Infrastructure.Holidays;

public class HolidayService : IHolidayService
{
    private readonly IRepository<Holiday> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public HolidayService(IRepository<Holiday> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateHolidayAsync(Holiday holiday, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(holiday.Name))
            throw new ArgumentException("Holiday name is required.");

        await _repository.AddAsync(holiday, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return holiday.Id;
    }

    public async Task<bool> UpdateHolidayAsync(Guid id, Holiday holiday, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(holiday.Name))
            throw new ArgumentException("Holiday name is required.");

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null || existing.IsDeleted) return false;

        existing.Name = holiday.Name;
        existing.Date = holiday.Date;
        existing.IsRecurring = holiday.IsRecurring;
        existing.Description = holiday.Description;
        existing.IsActive = holiday.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;
        existing.UpdatedBy = holiday.UpdatedBy;

        _repository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteHolidayAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing == null || existing.IsDeleted) return false;

        existing.IsDeleted = true;
        existing.IsActive = false;
        existing.UpdatedAt = DateTime.UtcNow;

        _repository.Update(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
