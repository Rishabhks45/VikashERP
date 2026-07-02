using System.Net.Http.Json;
using VikashERP.Web.Helpers;
using VikashERP.Web.Models;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class TimezoneService : ITimezoneService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;

    public TimezoneService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public string GetIanaId()
    {
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("timezone_iana")?.Value;
        return string.IsNullOrWhiteSpace(claim) ? DateFormatHelper.DefaultIanaId : claim;
    }

    public string FormatDate(DateTime? utc, string empty = "—")
        => DateFormatHelper.FormatDate(utc, GetIanaId(), empty);

    public string FormatDateTime(DateTime utc)
        => DateFormatHelper.FormatDateTime(utc, GetIanaId());

    public async Task<List<TimezoneListItemDto>> GetActiveTimezonesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<List<TimezoneListItemDto>>("api/timezones/active", cancellationToken);
            return result ?? new List<TimezoneListItemDto>();
        }
        catch
        {
            return new List<TimezoneListItemDto>();
        }
    }
}
