using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class AvidityCalculation
    {
        private static double CalculateAvidity(List<double> affinities)
        {
            return Math.Round(affinities.Sum() / affinities.Count, 4);
        }

        public static void CalculateAvidity(IEnumerable<Cluster> clusters)
        {
            foreach(var cluster in clusters)
            {
                var affinities = cluster.Antibodies.Select(antibody => antibody.AllAffinities[cluster.Center.Coordinates]).ToList();
                cluster.Avidity = CalculateAvidity(affinities);
            }
        }
    }
}
