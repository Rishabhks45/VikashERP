using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VikashERP.Web.Services.Interfaces;

namespace VikashERP.Web.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly HttpClient _httpClient;
    private const long MaxFileSize = 2 * 1024 * 1024;
    private static readonly string[] AllowedTypes = { ".jpg", ".jpeg", ".png", ".svg" };
    private const string ImagePath = "images";

    public FileUploadService(IWebHostEnvironment webHostEnvironment, System.Net.Http.IHttpClientFactory httpClientFactory)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpClient = httpClientFactory.CreateClient("VikashERP.Api");
    }

    public async Task<FileValidationResult> ValidateFileAsync(IBrowserFile file)
    {
        var result = new FileValidationResult();

        try
        {
            if (file == null)
            {
                result.IsValid = false;
                result.ErrorMessage = "No file selected.";
                return result;
            }

            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (!AllowedTypes.Contains(extension))
            {
                result.IsValid = false;
                result.ErrorMessage = $"File type {extension} not allowed. Allowed: {string.Join(", ", AllowedTypes)}";
                return result;
            }

            if (file.Size > MaxFileSize)
            {
                result.IsValid = false;
                result.ErrorMessage = $"File size exceeds {MaxFileSize / (1024 * 1024)} MB";
                return result;
            }

            if (extension == ".svg")
            {
                using Stream svgStream = file.OpenReadStream(MaxFileSize);
                using var svgMemoryStream = new MemoryStream();
                await svgStream.CopyToAsync(svgMemoryStream);
                svgMemoryStream.Position = 0;

                var svgContent = System.Text.Encoding.UTF8.GetString(svgMemoryStream.ToArray());
                if (!svgContent.Contains("<svg") || !svgContent.Contains("</svg>"))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Invalid SVG file format.";
                    return result;
                }
            }

            using Stream stream = file.OpenReadStream(MaxFileSize);
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var base64 = Convert.ToBase64String(memoryStream.ToArray());
            var dataUrl = $"data:{file.ContentType};base64,{base64}";

            result.IsValid = true;
            result.Base64Data = dataUrl;
        }
        catch (Exception ex)
        {
            result.IsValid = false;
            result.ErrorMessage = $"File validation failed: {ex.Message}";
        }

        return result;
    }

    public async Task<string> HandleFileUploadAsync(IBrowserFile file, string subFolder = "")
    {
        using var content = new MultipartFormDataContent();
        using var stream = file.OpenReadStream(MaxFileSize);
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
        
        content.Add(fileContent, "File", file.Name);
        if (!string.IsNullOrWhiteSpace(subFolder))
        {
            content.Add(new StringContent(subFolder), "CategoryName");
        }

        var response = await _httpClient.PostAsync("api/upload/file", content);
        if (response.IsSuccessStatusCode)
        {
            var result = await System.Net.Http.Json.HttpContentJsonExtensions.ReadFromJsonAsync<System.Text.Json.JsonElement>(response.Content);
            return result.GetProperty("url").GetString()!;
        }

        throw new Exception("Failed to upload file to the server.");
    }

    public async Task<string> HandleFileUploadInByteAsync(byte[] file)
    {
        if (file == null || file.Length == 0) return "File data is empty.";
        if (file.Length > MaxFileSize) return $"File size {file.Length} bytes exceeds the maximum allowed size of {MaxFileSize} bytes.";

        using var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(file);
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        
        content.Add(fileContent, "File", $"{Guid.NewGuid()}.png");
        content.Add(new StringContent("profiles"), "CategoryName");

        var response = await _httpClient.PostAsync("api/upload/file", content);
        if (response.IsSuccessStatusCode)
        {
            var result = await System.Net.Http.Json.HttpContentJsonExtensions.ReadFromJsonAsync<System.Text.Json.JsonElement>(response.Content);
            return result.GetProperty("url").GetString()!;
        }

        return string.Empty;
    }

    public Task<bool> DeleteFileAsync(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return Task.FromResult(false);
        }

        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));
        var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        if (!IsWithinUploadsDirectory(fullPath, uploadsDir))
        {
            return Task.FromResult(false);
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    public bool FileExists(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            return false;
        }

        var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));
        var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        return IsWithinUploadsDirectory(fullPath, uploadsDir) && File.Exists(fullPath);
    }

    private static bool IsWithinUploadsDirectory(string fullPath, string uploadsDir)
    {
        return Path.GetFullPath(fullPath).StartsWith(Path.GetFullPath(uploadsDir), StringComparison.OrdinalIgnoreCase);
    }
}
