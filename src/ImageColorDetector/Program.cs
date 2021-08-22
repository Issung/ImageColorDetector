using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageColorDetector
{
    class Program
    {
        static readonly float blackThreshold = Color.FromArgb(3, 3, 3).GetBrightness();
        static readonly float whiteThreshold = Color.FromArgb(252, 252, 252).GetBrightness();

        static void Main(string[] args)
        {
            Console.Write("Enter directory: ");
            var dir = Console.ReadLine();

            if (!dir.EndsWith(Path.DirectorySeparatorChar))
            {
                dir += Path.DirectorySeparatorChar;
            }

            List<string> detectedFiles = new();
            object detectedFilesLock = new object();

            var files = Directory.GetFiles(dir);

            files = files.Select(t => t.Replace(dir, string.Empty)).ToArray();

            Parallel.ForEach(files, (file) => 
            {
                var image = new Bitmap(Path.Combine(dir, file));

                var thumb = new Bitmap(image, new Size(8, 8));
                image.Dispose();

                var avgColor = GetImageAverageColor(thumb);

                var brightness = avgColor.GetBrightness();

                if (brightness < blackThreshold || brightness > whiteThreshold)
                {
                    lock (detectedFilesLock)
                    {
                        detectedFiles.Add(file);
                    }
                }
            });

            // Do something with the detected files.
            Directory.CreateDirectory(dir + Path.DirectorySeparatorChar + "Detected");

            Parallel.ForEach(detectedFiles, (file) =>
            {
                File.Move(Path.Combine(dir, file), Path.Combine(dir, "Detected", file), true);
            });
        }

        private static Color GetImageAverageColor(Bitmap img)
        {
            int r = 0, g = 0, b = 0;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    var pixel = img.GetPixel(x, y);
                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;
                }
            }

            var total = 8 * 8;

            var rAvg = r / total;
            var gAvg = g / total;
            var bAvg = b / total;

            return Color.FromArgb(rAvg, gAvg, bAvg);
        }
    }
}
