using System;
using System.Collections.Generic;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class AntibodySupression
    {

        public static void SupressesAntibody(Dictionary<(int x, int y), Clone> clones, List<Antigen> antigens)
        {
            foreach(var antigen in antigens)
            {
                var clone = clones[antigen.Coordinates];
                if (clone.Affinity > antigen.AllAffinities[clone.ClusterCenterCoordinates])
                {
                    antigen.Coordinates = clone.Coordinates;
                }
            }
        }
    }
}
