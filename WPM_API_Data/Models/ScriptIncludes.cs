namespace  WPM_API.Data.Models
{
    public static class ScriptIncludes
    {
        public const string Versions = "Versions";

        public static string[] GetAllIncludes()
        {
            string[] includes =
            {
                Versions
            };
            return includes;
        }
    }
}
