using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class CenterSelection
    {
        private static Antibody SelectCenter(List<Antibody> candidates)
        {
            var center = candidates.First();
            candidates.RemoveAt(0);

            return center;
        }

        private static void AddCenter(List<Antibody> candidates, List<Antibody> result, double NAT)
        {
            Antibody center = null;
            while(center == null && candidates.Count > 0)
            {
                center = SelectCenter(candidates);

                if (result.FirstOrDefault(antibody => antibody.AllAffinities[center.Coordinates] > NAT) != null)
                    center = null;
            }

            result.Add(center);
        }

        public static IEnumerable<Antibody> SelectCenters(IEnumerable<Antibody> antibodies, int K, int N, double NAT)
        {
            var result = new List<Antibody>();

            StimulationCalculation.CalculateStimulation(antibodies, K);

            var candidates = antibodies.OrderByDescending(antibody => antibody.NumberOfConnections)
                .ThenByDescending(antibody => antibody.Stimulation)
                .ToList();

            while (result.Count < N && candidates.Count > 0)
            {
                AddCenter(candidates, result, NAT);
            }

            return result;
        }
    }
}
