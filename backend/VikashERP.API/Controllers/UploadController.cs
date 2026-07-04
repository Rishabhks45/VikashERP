using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace VikashERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpPost("product-image")]
    public async Task<IActionResult> UploadProductImage([FromForm] FileUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("No file uploaded.");

        var categoryName = request.CategoryName;
        if (string.IsNullOrWhiteSpace(categoryName))
            categoryName = "Uncategorized";

        // Sanitize category name for folder
        var safeCategoryName = string.Join("_", categoryName.Split(Path.GetInvalidFileNameChars()));
        
        var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", safeCategoryName);
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/{safeCategoryName}/{uniqueFileName}";
        return Ok(new { Url = relativePath });
    }

    [HttpPost("file")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
            return BadRequest("No file uploaded.");

        var subFolder = request.CategoryName;
        if (string.IsNullOrWhiteSpace(subFolder))
            subFolder = "misc";

        var safeSubFolder = string.Join("_", subFolder.Split(Path.GetInvalidFileNameChars()));
        
        var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRoot, "uploads", safeSubFolder);
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/{safeSubFolder}/{uniqueFileName}";
        return Ok(new { Url = relativePath });
    }
}

public class FileUploadRequest
{
    public IFormFile File { get; set; } = null!;
    public string CategoryName { get; set; } = string.Empty;
}

