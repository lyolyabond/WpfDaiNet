using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class Cloning
    {
        public static Dictionary<(int x, int y), List<Clone>> ProportionalCloning(IEnumerable<Antigen> antigens, IEnumerable<Cluster> clusters, int C, int N)
        {
            Func<double, int> GetlNumbersOfClones = (double maxAffinity) => GetProportionalNumbersOfClones(N, C, maxAffinity);
            return Clone(antigens, clusters, GetlNumbersOfClones);
        }

        public static Dictionary<(int x, int y), List<Clone>> InverselyProportionalCloning(IEnumerable<Antigen> antigens, IEnumerable<Cluster> clusters, int N)
        {
            Func<double, int> GetlNumbersOfClones = (double maxAffinity) => GetInverselyProportionalNumbersOfClones(N, maxAffinity);
            return Clone(antigens, clusters, GetlNumbersOfClones);
        }

        public static Dictionary<(int x, int y), List<Clone>> StaticCloning(IEnumerable<Antigen> antigens, int Nc)
        {
            var clonesDictionary = new Dictionary<(int x, int y), List<Clone>>(antigens.Count());

            foreach (var antibody in antigens)
            {
               clonesDictionary[antibody.Coordinates] = Clone(antibody, Nc);
            }

            return clonesDictionary;
        }

        private static int GetProportionalNumbersOfClones(int M, int C, double maxAffinity)
        {
            return (int)(M * C * maxAffinity);
        }

        private static int GetInverselyProportionalNumbersOfClones(int M, double maxAffinity)
        {
            return (int)(M * (1 - maxAffinity));
        }

        public static List<Antigen> GetAntigens(IEnumerable<Antibody> antibodies, IEnumerable<Cluster> clusters)
        {
            var antigens = new List<Antigen>();

            foreach (var antibody in antibodies)
            {
                var cluster = clusters.FirstOrDefault(cluster => cluster.Center == antibody || cluster.Antibodies.Contains(antibody));
                if (cluster == null)
                    antigens.Add(new Antigen(antibody));
            }

            return antigens;
        }

        private static Dictionary<(int x, int y), List<Clone>> Clone(IEnumerable<Antigen> antigens, IEnumerable<Cluster> clusters, Func<double, int> GetNumbersOfClones)
        {
            var clonesDictionary = new Dictionary<(int x, int y), List<Clone>>(antigens.Count());

            foreach (var antibody in antigens)
            {
                var maxAffinity = clusters.Select(cluster => antibody.AllAffinities[cluster.Center.Coordinates]).Max();
                var Nc = GetNumbersOfClones(maxAffinity);

                clonesDictionary[antibody.Coordinates] = Clone(antibody, Nc);
            }

            return clonesDictionary;
        }

        private static List<Clone> Clone(Antigen antigen, int Nc)
        {
            if (Nc == 0)
                Nc = 1;

            var clones = new List<Clone>(Nc);

            for(var i = 0; i < Nc; i++)
            {
                clones.Add(new Clone(antigen));
            }

            return clones;
        }
    }
}
