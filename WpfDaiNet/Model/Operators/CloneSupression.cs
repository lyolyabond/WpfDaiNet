using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class CloneSupression
    {
        public static Dictionary<(int x, int y), Clone> SupressessClone(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters)
        {
            foreach(var item in clones)
            {
                foreach(var clone in item.Value)
                {
                    var affinity = clusters.ToDictionary(cluster => cluster.Center.Coordinates, cluster => Presentation.CalculateAffinity(cluster.Center, clone.Coordinates))
                        .OrderByDescending(item => item.Value)
                        .First();
                    clone.ClusterCenterCoordinates = affinity.Key;
                    clone.Affinity = affinity.Value; 
                }
            }

            return clones.Where(clone => clone.Value.Count > 0).ToDictionary(item => item.Key, item => item.Value.OrderByDescending(clone => clone.Affinity).First());
        }
    }
}
