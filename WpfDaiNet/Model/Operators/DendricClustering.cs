using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{ 
    class DendricClustering
    {

        public static List<Cluster> DendricCluster(IEnumerable<Antibody> antibodies, IEnumerable<Antibody> centers)
        {
            var clusters = centers.Where(center => center != null).Select(center => new Cluster(center)).ToList();

            
            foreach (var antibody in antibodies)
            {
                var selectedClusters = clusters.Where(cluster => antibody.Affinities.ContainsKey(cluster.Center.Coordinates) || cluster.Center.Affinities.ContainsKey(antibody.Coordinates))
                    .ToList();

                if (selectedClusters.Count == 1)
                    selectedClusters.First().AddAntibody(antibody);
                else if (selectedClusters.Count > 1)
                {
                    selectedClusters.OrderByDescending(cluster => cluster.Center.Affinities[antibody.Coordinates])
                        .First()
                        .AddAntibody(antibody);
                }
            }

            return clusters;
        }
    }
}
