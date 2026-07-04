using System.Threading.Tasks;

namespace VikashERP.Mobile.Services.Interfaces;

public interface IPdfExportService
{
    Task<(bool IsSuccess, string ErrorMessage)> SaveAndSharePdfAsync(string base64Content, string filename);
    Task<(bool IsSuccess, string ErrorMessage)> SaveAndOpenPdfAsync(string base64Content, string filename);
    Task<(bool IsSuccess, string ErrorMessage)> PrintPdfAsync(string base64Content, string filename);
}
