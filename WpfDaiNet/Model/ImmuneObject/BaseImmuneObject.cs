using System;
using System.Collections.Generic;

namespace WpfDaiNet.Model.ImmuneObject
{
    abstract class BaseImmuneObject
    {
        public (int x, int y) Coordinates { get; set; }
        public int X => Coordinates.x;
        public int Y => Coordinates.y;

        public Dictionary<(int x, int y), double> AllAffinities { get; set; }

        public BaseImmuneObject((int x, int y) coordinates)
        {
            Coordinates = coordinates;
            AllAffinities = new Dictionary<(int x, int y), double>();
        }

        public double CalculateManhattanDistance((int x, int y) antibody)
        {
            return Math.Abs(antibody.x - X) + Math.Abs(antibody.y - Y);
        }
    }
}
