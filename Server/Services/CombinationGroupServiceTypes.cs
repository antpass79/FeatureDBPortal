namespace FeatureDBPortal.Server.Services
{
    public delegate ICombinationGroupService CombinationGroupServiceResolver(string key);
    public static class CombinationGroupServiceTypes
    {
        public const string COMBINATION_GROUP_BY_ANY = "COMBINATION_GROUP_BY_ANY";
        public const string COMBINATION_GROUP_BY_ONE = "COMBINATION_GROUP_BY_ONE";
        public const string COMBINATION_GROUP_BY_TWO = "COMBINATION_GROUP_BY_TWO";
        public const string COMBINATION_GROUP_BY_THREE = "COMBINATION_GROUP_BY_THREE";
    }
}
