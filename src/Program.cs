using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics.Skia;
using SkiaSharp;

namespace Medicolor;

internal class Program
{
    /// <summary>
    /// Main program entry point.
    /// </summary>
    /// <param name="args">Location of image to input.</param>
    /// <returns>success or failure.</returns>
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: No image path provided.");
            Console.ResetColor();
            Console.WriteLine("Usage: medicolor <image path>");
            return 1;
        }

        IImage source;
        using (FileStream fs = File.OpenRead(args[0]))
        {
            source = PlatformImage.FromStream(fs);
        }

        if (source is not SkiaImage && source is PlatformImage platformImage)
        {
            IImage image = SkiaImage.FromStream(platformImage.AsStream());
            source.Dispose();
            source = image;
        }

        SKBitmap bitmap = new();
        using (FileStream fs = File.OpenRead(args[0]))
        {
            bitmap = SKBitmap.Decode(fs);
        }
        SKImageInfo info = new((int)source.Width, (int)source.Height);
        using SKSurface surface = SKSurface.Create(info);
        SKCanvas canvas = surface.Canvas;

        float[] colorMatrixTritanomaly10 =
        [
            1.255528f,  -0.076749f, -0.178779f, 0,  0,
            -0.078411f, 0.930809f,  0.147602f,  0,  0,
            0.004733f,  0.691367f,  0.303900f,  0,  0,
            0,          0,          0,          1,  0
        ];

        using (SKPaint paint = new())
        {
            paint.ColorFilter = SKColorFilter.CreateColorMatrix(colorMatrixTritanomaly10);
            canvas.DrawBitmap(bitmap, new SKRect(0, 0, source.Width, source.Height), paint);
        }

        using FileStream output = File.OpenWrite("output.png");
        surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100).SaveTo(output);

        source.Dispose();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success: Image saved to output.png");
        Console.ResetColor();
        return 0;
    }
}
