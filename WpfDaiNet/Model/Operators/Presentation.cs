using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class Presentation
    {
        public static double CalculateAffinity(BaseImmuneObject antibody1, (int x, int y) point)
        {
            var distance = antibody1.CalculateManhattanDistance(point);
            return Math.Round(1 / (1 + distance), 6);
        }

        public static void CalculateAffinities(List<Antigen> antigens)
        {
            foreach(var antigen in antigens)
            {
                antigen.AllAffinities = antigen.AllAffinities.Keys.ToDictionary(point => point, point => CalculateAffinity(antigen, point));
            }
        }

        public static List<Antibody> GetAntibodies(IEnumerable<(int x, int y)> points)
        {
            var antibodies = points.Select(point => new Antibody(point)).ToList();

            foreach (var antibody1 in antibodies)
            {
                foreach (var antibody2 in antibodies)
                {
                    if (antibody1 == antibody2 || antibody1.AllAffinities.ContainsKey(antibody2.Coordinates))
                        continue;

                    var affinity = CalculateAffinity(antibody1, antibody2.Coordinates);
                    antibody1.AllAffinities.Add(antibody2.Coordinates, affinity);
                    antibody2.AllAffinities.Add(antibody1.Coordinates, affinity);
                }
            }

            return antibodies;
        }
    }
}
