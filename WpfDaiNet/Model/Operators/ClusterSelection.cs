using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class ClusterSelection
    {
        public static void SelectCluster(List<Antigen> antigens, IEnumerable<Cluster> clusters, bool isClustering)
        {
            AvidityCalculation.CalculateAvidity(clusters);

            foreach(var cluster in clusters)
            {
                var objects = antigens.Where(antigen => antigen.AllAffinities[cluster.Center.Coordinates] >= cluster.Avidity)
                    .Select(antigen =>
                    {
                        antigens.Remove(antigen);
                        if (isClustering)
                            antigen.Coordinates = antigen.BaseCoordinates;
                        return antigen;
                    })
                    .ToList();

                if (objects.Count > 0)
                cluster.Antibodies.AddRange(objects);   
            }
        }

        public static void SelectClusterForUnspecified(List<Antigen> antigens, IEnumerable<Cluster> clusters)
        {
            var newItems = new List<KeyValuePair<(int x, int y), double>>();

            foreach(var antigen in antigens)
            {
                var @object = antigen.AllAffinities.Where(item => clusters.Any(cluster => cluster.Center.Coordinates == item.Key))
                     .OrderByDescending(item => item.Value).First();
                antigen.Coordinates = antigen.BaseCoordinates;
                clusters.First(cluster => cluster.Center.Coordinates == @object.Key).AddAntibody(antigen);
            }
        }
    }
}
