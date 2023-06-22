using WpfDaiNet.Model.Managers;

namespace WpfDaiNet.Model
{
    class AppModel
    {
        public readonly static Settings Settings = new Settings();
        public readonly static Result Result = new Result();

        public static void SetSettingsParameters(string imagePath, bool isClustering, string N, string K, string limit, string cloning, string mutation, string numbersOfClones, string cloningC, string mutationC, string delay)
        {
            var parameters = ImageManager.GetParameters(imagePath);
            Settings.SetImagePath(imagePath)
                .SetImageWidth(parameters.width)
                .SeImageHeight(parameters.height)
                .SetCloningC(int.Parse(cloningC))
                .SetMutationC(double.Parse(mutationC))
                .SetNumberOfClones(int.Parse(numbersOfClones));
            SetSettingsParameters(isClustering, int.Parse(N), int.Parse(K), int.Parse(limit), cloning, mutation, int.Parse(delay));
        }

        public static void SetSettingsParameters(bool isClustering, int N, int K, int limit, string cloning, string mutation, int delay)
        {
            Settings
                .SetIsClustering(isClustering)
                .SetN(N)
                .SetK(K)
                .SetLimit(limit)
                .SetCloningType(Settings.GetCloningType(cloning))
                .SetMutationType(Settings.GetMutationType(mutation))
                .SetDelay(delay);
        }
    }
}
