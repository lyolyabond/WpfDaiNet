using System;
using System.Collections.Generic;

namespace WpfDaiNet.Model.ImmuneObject
{
    class Antibody : BaseImmuneObject
    {
        public Dictionary<(int x, int y), double> Affinities { get; set; }
        
        public double Stimulation { get; set; }

        public int NumberOfConnections => Affinities.Count;

        public Antibody((int x, int y) coordinates) : base(coordinates)
        {
            Affinities = new Dictionary<(int x, int y), double>();
        }
    }
}
