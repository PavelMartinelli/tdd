using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization;

public interface IImageSaver
{
    string GenerateFileName(string baseName);
    string SaveBitmap(Bitmap bitmap, string fileName, ImageFormat format = null);
}