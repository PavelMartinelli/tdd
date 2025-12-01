using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public class ImageSaver : IImageSaver
{
    private readonly string _outputDirectory;

    public ImageSaver(string outputDirectory)
    {
        _outputDirectory = outputDirectory ?? throw new ArgumentNullException(nameof(outputDirectory));
        Directory.CreateDirectory(_outputDirectory);
    }
    
    public string GenerateFileName(string baseName)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        return $"{baseName}_{timestamp}.png";
    }

    public string SaveBitmap(Bitmap bitmap, string fileName, ImageFormat format = null)
    {
        ArgumentNullException.ThrowIfNull(bitmap);

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name cannot be empty", nameof(fileName));
            
        var filePath = Path.Combine(_outputDirectory, fileName);
        bitmap.Save(filePath, format ?? ImageFormat.Png);
        
        return Path.GetFullPath(filePath);
    }
}