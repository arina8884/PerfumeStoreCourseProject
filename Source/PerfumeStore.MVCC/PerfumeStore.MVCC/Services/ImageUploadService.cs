using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PerfumeStore.MVCC.Services.Interfaces;

namespace PerfumeStore.MVCC.Services;

public class ImageUploadService : IImageUploadService
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024;

    private readonly IWebHostEnvironment _environment;

    public ImageUploadService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveProductImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return null;
        }

        if (file.Length > MaxFileSizeBytes)
        {
            throw new InvalidOperationException("Размер файла не должен превышать 5 МБ.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Допустимые форматы: JPG, PNG, WEBP.");
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");

        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var physicalPath = Path.Combine(uploadsFolder, fileName);

        await using var stream = new FileStream(physicalPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/images/products/{fileName}";
    }
}
