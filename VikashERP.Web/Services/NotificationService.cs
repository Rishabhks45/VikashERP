using MudBlazor;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class NotificationService : INotificationService
{
    private readonly ISnackbar _snackbar;

    public NotificationService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }

    public void Success(string message) =>
        _snackbar.Add(message, Severity.Success);

    public void Warning(string message) =>
        _snackbar.Add(message, Severity.Warning);

    public void Error(string message) =>
        _snackbar.Add(message, Severity.Error);
}
