using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.ExceptionHandling;
using SixRens.UI.MAUI.Services.Paths;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public sealed partial class SixRensCore
    {
        public 插件包管理器 PluginPackageManager { get; }
        public 预设管理器 PresetManager { get; }

        private readonly IFileSystem fileSystem;

        public async Task<bool> InstallDefaultPluginsAsync()
        {
            using var s = await fileSystem.OpenAppPackageFileAsync("SixRens.DefaultPlugins-1.14.1.srspg");

            var (package, hasInstalled) = await Task.Factory.StartNew(
                () => PluginPackageManager.从外部加载插件包(s));

            if (hasInstalled)
            {
                package.Dispose();
                return false;
            }
            return true;
        }

        public async Task InstallDefaultPresetsAsync()
        {
            string content;
            using (var s = await fileSystem.OpenAppPackageFileAsync("DefaultPreset.txt"))
            using (var reader = new StreamReader(s))
            {
                content = await reader.ReadToEndAsync();
            }
            for (var name = "默认预设"; ;)
            {
                var p = PresetManager.导入预设文件内容(name, content);
                if (p is not null)
                    return;

                var guid = Guid.NewGuid().ToString("D");
                guid = guid.Split('-', 2)[0];
                name = $"默认预设 {guid}";
            }
        }

        public SixRensCore(
            ExceptionHandler exceptionHandler,
            PathProvider pathProvider,
            IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
            var saver = new DataStorager(pathProvider);
            try
            {
                PluginPackageManager = new(saver);
                PresetManager = new(saver);
            }
            catch (Exception e)
            {
                exceptionHandler.Handle(e);
            }
        }
    }
}
