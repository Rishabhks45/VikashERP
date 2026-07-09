using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Mobile.Models;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IHolidayService
{
    Task<List<HolidayDto>> GetHolidaysAsync();
    Task<HolidayDto?> CreateHolidayAsync(CreateHolidayDto dto);
    Task<HolidayDto?> UpdateHolidayAsync(Guid id, UpdateHolidayDto dto);
    Task<bool> DeleteHolidayAsync(Guid id);
}
