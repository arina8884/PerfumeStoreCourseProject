namespace PerfumeStore.MVCC.Services.Interfaces;

public interface IImageUploadService
{
    Task<string?> SaveProductImageAsync(Microsoft.AspNetCore.Http.IFormFile? file);
}
