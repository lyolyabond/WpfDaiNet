using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class Mutation
    {
        private static Random rand = new Random();

        public static void CloneMutation(Clone clone, double C, double affinity)
        {
            var isX = rand.Next(2) == 0;
            var isPlus = rand.Next(2) == 0;

            if (isX)
                clone.Coordinates = (GetValue(clone.X, C, affinity, isPlus, StaticData.Width), clone.Y);
            else clone.Coordinates = (clone.X, GetValue(clone.Y, C, affinity, isPlus, StaticData.Height));
        }

        private static int GetValue(int coordinate, double C, double affinity, bool isPlus, int maxValue)
        {
           var result = GetValue(coordinate, C, affinity, isPlus);

           if (result > maxValue || result < 0)
                result = GetValue(coordinate, C, affinity, !isPlus);
            if (result > maxValue || result < 0)
                result = coordinate;

            return result;
        }

        private static int GetValue(int coordinate, double C, double affinity, bool isPlus)
        {
            var result = 0;
            if (!isPlus)
                result = (int)(coordinate - C * (1 / affinity - 1));
            if (isPlus || result < 0)
                result = (int)(coordinate + C * (1 / affinity - 1));

            return result;
        }


        private static double GetAffinity(List<Cluster> clusters, Clone clone)
        {
            var clusterCenter = clusters[rand.Next(clusters.Count)].Center;
            return clone.AllAffinities[clusterCenter.Coordinates];
        }

        public static void StaticMutation(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters, double C)
        {
            foreach (var item in clones)
            {
                foreach (var clone in item.Value)
                {
                    var affinity = GetAffinity(clusters, clone);
                    CloneMutation(clone, C, affinity);
                }
            }
        }

        public static void RandomMutation(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters, IEnumerable<BaseImmuneObject> antibodies)
        {
            var allAffinities = antibodies.SelectMany(antibody => antibody.AllAffinities);
            var minAffinity = allAffinities.Min(affinity => affinity.Value);
            var maxAffinity = allAffinities.Max(affinity => affinity.Value);

            foreach (var item in clones)
            {
                foreach (var clone in item.Value)
                {
                    var affinity = GetAffinity(clusters, clone);
                    var C = rand.NextDouble() * (maxAffinity - minAffinity) + minAffinity;
                    CloneMutation(clone, C, affinity);
                }
            }
        }

        public static void ProportionalMutation(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters)
        {
            Func<double, double> getMaxValue = (affinity) => affinity;
            Func<double, double> getMinValue = _ => 0;
            Mutate(clones, clusters, getMinValue, getMaxValue);
        }


        public static void InverselyProportionalMutation(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters)
        {
            Func<double, double> getMaxValue = (affinity) => 1 - affinity;
            Func<double, double> getMinValue = _ => 0;
            Mutate(clones, clusters, getMinValue, getMaxValue);
        }

        public static void LimitedInverselyProportionalMutation(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters)
        {
            Func<double, double> getMaxValue = (affinity) => 1 - affinity;
            Func<double, double> getMinValue = maxValue => maxValue / 2;
            Mutate(clones, clusters, getMinValue, getMaxValue);
        }

        private static void Mutate(Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters, Func<double, double> getMinValue, Func<double, double> getMaxValue)
        {
            foreach (var item in clones)
            {
                foreach (var clone in item.Value)
                {
                    var affinity = GetAffinity(clusters, clone);
                    var maxValue = getMaxValue(affinity);
                    var minValue = getMinValue(maxValue);
                    var C = rand.NextDouble() * (maxValue - minValue) + minValue;

                    CloneMutation(clone, C, affinity);
                }
            }
        }

    }
}
