using System;
using System.Collections.Generic;
using System.Linq;
using WpfDaiNet.Model.ImmuneObject;

namespace WpfDaiNet.Model.Operators
{
    class NATCalculation
    {
        public static double NATCalculate(IEnumerable<Antibody> antibodies)
        {
            var n = antibodies.Count();
            return Math.Round(antibodies.Sum(antibody => antibody.AllAffinities.Sum(affinity => affinity.Value)) / (n * (n - 1)), 4);
        }
    }
}
