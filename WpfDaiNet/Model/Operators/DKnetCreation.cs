using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class DKnetCreation
    {
        public static IEnumerable<Antibody> GetDKNet(IEnumerable<Antibody> antibodies, int K)
        {
            return antibodies.Select(antibody =>
            {
                antibody.Affinities = antibody.AllAffinities.OrderByDescending(affinity => affinity.Value)
                .Take(K)
                .ToDictionary(affinity => affinity.Key, affinity => affinity.Value);
                return antibody;
            }).ToList();
        }
    }
}
