using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{ 
    class StimulationCalculation
    {
        public static double CalculateStimulation(Antibody antibody, int K)
        {
            return Math.Round(antibody.Affinities.Sum(affinity => affinity.Value) / K, 4);
        }

        public static void CalculateStimulation(IEnumerable<Antibody> antibodies, int K)
        {
            foreach (var antibody in antibodies)
            {
                antibody.Stimulation = CalculateStimulation(antibody, K);

                var newAffinities = antibodies.Where(a => !antibody.Affinities.ContainsKey(a.Coordinates) && a.Affinities.ContainsKey(antibody.Coordinates))
                .Select(a => new KeyValuePair<(int, int), double>(a.Coordinates, a.Affinities[antibody.Coordinates]));

                foreach (var item in newAffinities)
                {
                    antibody.Affinities.Add(item.Key, item.Value);
                }
            }
        }
    }
}
