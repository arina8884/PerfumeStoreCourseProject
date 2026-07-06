namespace PerfumeStore.MVCC.Helpers;

public static class ImageUrlHelper
{
    public const string Placeholder = "/images/products/no-image.png";

    public static string GetProductImageUrl(string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Placeholder;
        }

        var path = imageUrl.Trim().Replace('\\', '/');

        if (path.StartsWith("/images/products/", StringComparison.OrdinalIgnoreCase))
        {
            return path;
        }

        if (path.StartsWith("/images/", StringComparison.OrdinalIgnoreCase))
        {
            var fileName = Path.GetFileName(path);
            return string.IsNullOrWhiteSpace(fileName) ? Placeholder : $"/images/products/{fileName}";
        }

        if (path.StartsWith('/'))
        {
            return path;
        }

        return $"/images/products/{path}";
    }
}
