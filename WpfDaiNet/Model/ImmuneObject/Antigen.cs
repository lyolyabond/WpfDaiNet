using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfDaiNet.Model.ImmuneObject
{
    class Antigen : BaseImmuneObject
    {
        public (int x, int y) BaseCoordinates { get; set; }
        public Antigen((int x, int y) coordinates) : base(coordinates)
        {
        }

        public Antigen(Antibody antibody) : base(antibody.Coordinates)
        {
            BaseCoordinates = antibody.Coordinates;
            AllAffinities = antibody.AllAffinities;
        }
    }
}
