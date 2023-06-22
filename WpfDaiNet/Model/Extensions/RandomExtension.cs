using System;
using System.Collections.Generic;

namespace WpfDaiNet.Model
{
    public static class RandomExtension
    {
        public static (int x, int y) NextCoordinate(this Random rand, int width, int hight) => (rand.Next(0, width), rand.Next(0, hight));

        public static IEnumerable<(int x, int y)> NextPoints(this Random rand, int count, int width, int hight)
        {
            var points = new HashSet<(int, int)>(count);

            while (points.Count < count)
            {
                var point = rand.NextCoordinate(width, hight);
                points.Add(point);
            }

            return points;
        }
    }
}
