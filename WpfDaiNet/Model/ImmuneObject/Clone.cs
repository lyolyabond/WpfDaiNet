using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDaiNet.Model.ImmuneObject
{
    class Clone : Antigen
    {
        public (int x, int y) ClusterCenterCoordinates { get; set; }
        public double Affinity { get; set; }

        public Clone(Antigen antigen) : base(antigen.Coordinates)
        {
            BaseCoordinates = antigen.BaseCoordinates;
            AllAffinities = antigen.AllAffinities;
        }
    }
}
