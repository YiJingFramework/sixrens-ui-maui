namespace SixRens.UI.MAUI.Services.Paths
{
    public class PathProvider
    {
        private readonly string basePath;
        public PathProvider(IFileSystem fileSystem)
        {
            basePath = fileSystem.AppDataDirectory;
        }
        public string GetFullPath(string path)
        {
            return Path.GetFullPath(path, basePath);
        }
        public DirectoryInfo GetDirectory(string path, bool autoCreate = true)
        {
            var result = new DirectoryInfo(GetFullPath(path));
            if (autoCreate)
                result.Create();
            return result;
        }
        public FileInfo GetFile(string path)
        {
            return new(GetFullPath(path));
        }

        public FileInfo Database => GetFile("database.db");

        [Obsolete("用数据库")]
        public DirectoryInfo PluginPackages => GetDirectory("plugins");
        [Obsolete("用数据库")]
        public DirectoryInfo Presets => GetDirectory("presets");
    }
}
