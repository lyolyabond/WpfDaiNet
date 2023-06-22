using System.Collections.Generic;

namespace WpfDaiNet.Model
{
    public enum CloningType
    {
        Proportional = 0,
        InverselyProportional = 1,
        Static = 2,
    }

    public enum MutationType
    {
        Proportional = 0,
        InverselyProportional = 1,
        LimitedInverselyProportional = 2,
        Static = 3,
        Random = 4,
    }

    class StaticData
    {
        public static int Height = 100;
        public static int Width = 100;
        public static int Limit = 100;
        //public static double C = 0.5;
        public static string FolderPath = "result";
        public static int Id = 0;
        public static string StartName = "start";

        public static Dictionary<CloningType, string> CloningTypeNames = new Dictionary<CloningType, string>
        {
            {CloningType.Proportional, "Proportional"},
            {CloningType.InverselyProportional, "Inversely"},
            {CloningType.Static, "Static"},
        };

        public static Dictionary<MutationType, string> MutationTypeNames = new Dictionary<MutationType, string>
        {
            {MutationType.Proportional, "Proportional"},
            {MutationType.LimitedInverselyProportional, "Limited inversely"},
            {MutationType.InverselyProportional, "Inversely"},
            {MutationType.Static, "Static"},
            {MutationType.Random, "Random"},
        };
    }
}
