using VikashERP.Web.Models;

namespace VikashERP.Web.Services.Interfaces;

public interface ITimezoneService
{
    string GetIanaId();
    string FormatDate(DateTime? utc, string empty = "—");
    DateTime ToUtcDate(DateTime localDate);
    DateTime GetUserTime(DateTime utc);
    string FormatDateTime(DateTime utc);
    Task<List<TimezoneListItemDto>> GetActiveTimezonesAsync(CancellationToken cancellationToken = default);
}
