using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface IHolidayWebService
{
    Task<List<HolidayDto>> GetHolidaysAsync();
    Task<HolidayDto?> GetHolidayByIdAsync(Guid id);
    Task<HolidayDto?> CreateHolidayAsync(CreateHolidayDto request);
    Task<bool> UpdateHolidayAsync(Guid id, UpdateHolidayDto request);
    Task<bool> DeleteHolidayAsync(Guid id);
}
