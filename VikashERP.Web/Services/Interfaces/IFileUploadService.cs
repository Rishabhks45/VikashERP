using Microsoft.AspNetCore.Components.Forms;

namespace VikashERP.Web.Services.Interfaces;

public class FileValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string Base64Data { get; set; } = string.Empty;
}

public interface IFileUploadService
{
    Task<string> HandleFileUploadAsync(IBrowserFile file, string subFolder = "");
    Task<string> HandleFileUploadInByteAsync(byte[] file);
    Task<FileValidationResult> ValidateFileAsync(IBrowserFile file);
    Task<bool> DeleteFileAsync(string relativePath);
    bool FileExists(string relativePath);
}
