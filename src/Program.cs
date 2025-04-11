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

        // Check color type
        string? typedNumber;
        if (args.Length > 1)
        {
            typedNumber = args[1];
        }
        else
        {
            Console.WriteLine("Which type would you like to convert? (1 = Protanomaly, 2 = Deuteranomaly, 3 = Tritanomaly)");
            Console.Write("Type the number here: ");
            typedNumber = Console.ReadLine();
        }

        // check the input is number or not
        if (!int.TryParse(typedNumber, out int returnNumber))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: It can't convert {typedNumber} into number.");
            Console.ResetColor();
            return 1;
        }

        // check the number is in range or not
        float[] colorMatrix = ColorMatrix(returnNumber);
        if (colorMatrix.Length == 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: Invalid color type {returnNumber}.");
            Console.ResetColor();
            return 1;
        }
        // end of check color type

        using (SKPaint paint = new())
        {
            paint.ColorFilter = SKColorFilter.CreateColorMatrix(colorMatrix);
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

    /// <summary>
    /// The method to create color-matrix
    /// </summary>
    /// <param name="colorType">1 = Protanomaly, 2 = Deuteranomaly, 3 = Tritanomaly</param>
    /// <returns>Matrix type</returns>
    static float[] ColorMatrix(int colorType)
    {
        switch (colorType)
        {
            case 1:
                return [
                    0.152286f,  1.052583f,  -0.204868f, 0,  0,
                    0.114503f,  0.786281f,  0.099216f,  0,  0,
                    -0.003882f, -0.048116f, 1.051998f,  0,  0,
                    0,          0,          0,          1,  0
                ];

            case 2:
                return [
                    0.367322f,  0.860646f,  -0.227968f, 0,  0,
                    0.280085f,  0.672501f,  0.047413f,  0,  0,
                    -0.011820f, 0.042940f,  0.968881f,  0,  0,
                    0,          0,          0,          1,  0
                ];

            case 3:
                return [
                    1.255528f,  -0.076749f, -0.178779f, 0,  0,
                    -0.078411f, 0.930809f,  0.147602f,  0,  0,
                    0.004733f,  0.691367f,  0.303900f,  0,  0,
                    0,          0,          0,          1,  0
                ];

            default:
                return [];
        }
    }
}
