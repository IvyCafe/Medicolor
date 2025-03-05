using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Graphics.Skia;

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

        using FileStream fs = File.OpenRead(args[0]);
        IImage source = PlatformImage.FromStream(fs);

        if (source is not SkiaImage && source is PlatformImage platformImage)
        {
            IImage image = SkiaImage.FromStream(platformImage.AsStream());
            source.Dispose();
            source = image;
        }

        using SkiaBitmapExportContext skiaBitmapExportContext = new((int)source.Width, (int)source.Height, 1);
        skiaBitmapExportContext.Canvas.DrawImage(source, 0, 0, source.Width, source.Height);

        // Modifire the image (this is just an example)
        ICanvas canvas = skiaBitmapExportContext.Canvas;
        canvas.FillColor = Colors.Cyan;
        float circleRadius = (source.Width >= source.Height) ? source.Height / 4f : source.Width / 4f;
        canvas.FillCircle(source.Width / 2f, source.Height / 2f, circleRadius);

        skiaBitmapExportContext.WriteToFile("output.png"); // output

        source.Dispose();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Success: Image saved to output.png");
        Console.ResetColor();
        return 0;
    }
}
