using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Managers
{
    class ImageManager
    {
        private static Random rand = new Random();

        public static List<Color> Colors = new List<Color>()
        {
            Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Violet, Color.Pink, Color.Chocolate, Color.Salmon,
        };

        public static string GenerateImage(int count)
        {
            var bitmap = CreateBitmap(StaticData.Width, StaticData.Height);
            foreach (var point in rand.NextPoints(count, StaticData.Width, StaticData.Height))
            {
                bitmap.SetPixel(point.x, point.y, Color.Black);
            }

            var path = StaticData.StartName + StaticData.Id + ".png";
            SaveImage(path, bitmap);
            bitmap.Dispose();
            return Path.GetFullPath(path);
        }

       private static Bitmap CreateBitmap(int width, int height)
        {
            var image = new Bitmap(width, height);

            using (var g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
            }

            return image;
        }

        public static void SetPoints(Bitmap bitmap, IEnumerable<BaseImmuneObject> points, Color color)
        {
            foreach (var point in points)
            {
                if (!AppModel.Settings.IsClustering)
                {
                    var antigen = point as Antigen;
                    if (antigen != null)
                    {
                        SetPoint(bitmap, antigen.BaseCoordinates, Color.White);
                    }
                }
                
                SetPoint(bitmap, point.Coordinates, color);
            }
        }

        public static void SetPoint(Bitmap bitmap, (int x, int y) point, Color color)
        {
            bitmap.SetPixel(point.x, point.y, color);
        }

        public static void ConvertPointsToImage(int width, int height, IEnumerable<BaseImmuneObject> points, string path)
        {
            Directory.CreateDirectory(StaticData.FolderPath + StaticData.Id);

            var bitmap = CreateBitmap(width, height);
            SetPoints(bitmap, points, Color.Black);
            SaveImage(path, bitmap);
            bitmap.Dispose();
        }

        public static void SetClustersToImage(List<Cluster> clusters, string previousPath, string path)
        {
            var image = Image.FromFile(previousPath);

            var newBitmap = new Bitmap(image);
            for (var i = 0; i < clusters.Count; i++)
            {
                SetPoints(newBitmap, clusters[i].Antibodies, Colors[i % Colors.Count]);
                SetPoint(newBitmap, clusters[i].Center.Coordinates, Colors[i % Colors.Count]);
            }

            SaveImage(path, newBitmap);
            image.Dispose();
            newBitmap.Dispose();
        }

     
        public static void SaveImage(string path, Bitmap image)
        {
            image.Save(path);
        }

        public static (int height, int width) GetParameters(string path)
        {
            using (var bitmap = new Bitmap(path))
                return (bitmap.Height, bitmap.Width);
        }

        public static IEnumerable<(int x, int y)> GetPoints(string path)
        {
            var bitmap = new Bitmap(path);
            var points = new List<(int x, int y)>();
            var white = Color.White.ToArgb();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y).ToArgb();

                    if (color == white)
                        continue;

                    points.Add((x, y));
                }
            }

            bitmap.Dispose();
            return points;
        }
    }
}
