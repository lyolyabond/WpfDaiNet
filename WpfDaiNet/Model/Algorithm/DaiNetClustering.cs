using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WpfDaiNet.Model;
using WpfDaiNet.Model.ImmuneObject;
using WpfDaiNet.Model.Managers;
using WpfDaiNet.Model.Operators;

namespace DaiNet
{
    class DaiNetClustering
    {
        private static Func<IEnumerable<Antigen>, List<Cluster>, Dictionary<(int x, int y), List<Clone>>> GetCloningMethod(int pointsCount, Settings settings)
        {
            return (IEnumerable<Antigen> antigens, List<Cluster> clusters) =>
            {
                switch (settings.CloningType)
                {
                    case CloningType.InverselyProportional:
                        return Cloning.InverselyProportionalCloning(antigens, clusters, pointsCount);
                    case CloningType.Proportional:
                        return Cloning.ProportionalCloning(antigens, clusters, settings.CloningC, pointsCount);
                    case CloningType.Static:
                        return Cloning.StaticCloning(antigens, settings.NumberOfClones);
                }
                return null;
            };
        }

        private static Action<Dictionary<(int x, int y), List<Clone>>, List<Cluster>, IEnumerable<BaseImmuneObject>> GetMutationMethod(Settings settings)
        {
            return (Dictionary<(int x, int y), List<Clone>> clones, List<Cluster> clusters, IEnumerable<BaseImmuneObject> antibodies) =>
            {
                switch (settings.MutationType)
                {
                    case MutationType.InverselyProportional:
                        Mutation.InverselyProportionalMutation(clones, clusters);
                        break;
                    case MutationType.Proportional:
                        Mutation.ProportionalMutation(clones, clusters);
                        break;
                    case MutationType.LimitedInverselyProportional:
                        Mutation.LimitedInverselyProportionalMutation(clones, clusters);
                        break;
                    case MutationType.Static:
                        Mutation.StaticMutation(clones, clusters, settings.MutationC);
                        break;
                    case MutationType.Random:
                        Mutation.RandomMutation(clones, clusters, antibodies);
                        break;
                }
            };
        }

        private static string GetPath(int i)
        {
            var path = Path.Combine(StaticData.FolderPath + StaticData.Id, $"result{i}.png");
            return Path.GetFullPath(path);
        }

        private static void SetTime(Stopwatch stopwatch, ref long time)
        {
            stopwatch.Stop();
            time += stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
        }

        public static async Task ClusterAsync(IEnumerable<(int x, int y)> points, Action<string> imageUpdateCallback)
        {
            var stopwatch = new Stopwatch();
            long time = 0;

            StaticData.Id++;
            var settings = AppModel.Settings;
            var clone = GetCloningMethod(points.Count(), settings);
            var mutate = GetMutationMethod(settings);
            var i = 0;

            stopwatch.Start();
            var antibodies = Presentation.GetAntibodies(points);
            SetTime(stopwatch, ref time);

            ImageManager.ConvertPointsToImage(settings.ImageWidth, settings.ImageWidth, antibodies.Select(antybody => antybody), GetPath(i));
            imageUpdateCallback(GetPath(i));

            stopwatch.Start();
            var NAT = NATCalculation.NATCalculate(antibodies);
            DKnetCreation.GetDKNet(antibodies, settings.K);
            var centers = CenterSelection.SelectCenters(antibodies, settings.K, settings.N, NAT);
            var clusters = DendricClustering.DendricCluster(antibodies, centers);
            SetTime(stopwatch, ref time);

            i++;
            await Task.Delay(settings.Delay);
            
            ImageManager.SetClustersToImage(clusters, GetPath(i - 1), GetPath(i));
            stopwatch.Start();
            imageUpdateCallback(GetPath(i));
            var nonClusteredAntibodies = Cloning.GetAntigens(antibodies, clusters);
            
            while (nonClusteredAntibodies.Count > 0)
            {
                i++;
                if (i > settings.Limit)
                    break;

                var clones = clone(nonClusteredAntibodies, clusters);
                mutate(clones, clusters, antibodies);
                var supressedClones = CloneSupression.SupressessClone(clones, clusters);
                AntibodySupression.SupressesAntibody(supressedClones, nonClusteredAntibodies);

                Presentation.CalculateAffinities(nonClusteredAntibodies);
                ClusterSelection.SelectCluster(nonClusteredAntibodies, clusters, settings.IsClustering);
                SetTime(stopwatch, ref time);

                await Task.Delay(settings.Delay);
                ImageManager.SetClustersToImage(clusters, GetPath(i - 1), GetPath(i));
                imageUpdateCallback(GetPath(i));
            }

            if (nonClusteredAntibodies.Count > 0)
            {
                stopwatch.Start();
                ClusterSelection.SelectClusterForUnspecified(nonClusteredAntibodies, clusters);
                SetTime(stopwatch, ref time);

                await Task.Delay(settings.Delay);
                ImageManager.SetClustersToImage(clusters, GetPath(i - 1), GetPath(i));
                imageUpdateCallback(GetPath(i));
            }

            AppModel.Result.Seconds = (double)time / 1000;
        }
    }
}
