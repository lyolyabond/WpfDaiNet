using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDaiNet.Model.ImmuneObject
{
    class Cluster
    {
        public Antibody Center { get; private set; }

        public List<BaseImmuneObject> Antibodies { get; private set; }

        public double Avidity { get; set; }

        public Cluster(Antibody center)
        {
            Center = center;
            Antibodies = new List<BaseImmuneObject>();
        }

        public void AddAntibody(BaseImmuneObject antibody)
        {
            Antibodies.Add(antibody);
        }
    }
}
