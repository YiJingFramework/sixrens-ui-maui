using Java.Security.Cert;
using LiteDB;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.Paths;
using System.Globalization;
using System.Text;

namespace SixRens.UI.MAUI.Services.SixRens
{
    internal sealed class DataStorager : I插件包管理器储存器, I预设管理器储存器, IDisposable
    {
        readonly LiteDatabase database;
        public DataStorager(PathProvider pathProvider)
        {
            this.database = new LiteDatabase(new ConnectionString() {
                Collation = new(lcid: 2052, CompareOptions.IgnoreCase),
                Filename = pathProvider.Database.FullName
            });
        }

        #region PluginPackage
        private const string packageDeletedKey = "deleted";
        private ILiteStorage<string> GetPluginPackageStorage()
        {
            return this.database.GetStorage<string>("plugin_packages", "plugin_packages_chunks");
        }
        public string 储存插件包文件(Stream 插件包)
        {
            var storage = GetPluginPackageStorage();
            for (; ; )
            {
                var id = Guid.NewGuid().ToString();
                if (storage.FindById(id) is not null)
                    continue;
                _ = storage.Upload(id, $"{id}.srspg", 插件包);
                return id;
            }
        }

        public void 移除插件包文件(string 插件包本地识别码)
        {
            var storage = GetPluginPackageStorage();
            _ = storage.SetMetadata(插件包本地识别码, new BsonDocument() {
                [packageDeletedKey] = true
            });
        }

        public IEnumerable<(string 插件包本地识别码, Stream 插件包)> 获取所有插件包文件()
        {
            var storage = GetPluginPackageStorage();
            foreach (var file in storage.FindAll())
            {
                if (file.Metadata.TryGetValue(packageDeletedKey, out var deleted) &&
                    deleted.AsBoolean is true)
                    continue;
                yield return (file.Id, file.OpenRead());
            }
        }

        public Stream 获取插件包文件(string 插件包本地识别码)
        {
            var storage = GetPluginPackageStorage();
            return storage.FindById(插件包本地识别码)?.OpenRead();
        }
        #endregion

        #region Preset
        public class Preset
        {
            public string Id { get; set; }
            public string Content { get; set; }
            public bool Deleted { get; set; }
        }
        private ILiteCollection<Preset> GetPresetCollection()
        {
            var result = this.database.GetCollection<Preset>("presets");
            return result;
        }
        public IEnumerable<(string 预设名, string 内容)> 获取所有预设文件()
        {
            var collection = GetPresetCollection();
            foreach (var preset in collection.FindAll())
            {
                if (!preset.Deleted)
                    yield return (preset.Id, preset.Content);
            }
        }

        public bool 新建预设文件(string 预设名)
        {
            var collection = GetPresetCollection();
            var preset = collection.FindById(预设名);
            if (preset is not null)
                return preset.Deleted;
            var id = collection.Insert(new Preset() {
                Id = 预设名,
                Content = string.Empty,
                Deleted = true
            });
            return id is not null;
        }

        public void 储存预设文件(string 预设名, string 内容)
        {
            var collection = GetPresetCollection();
            _ = collection.Update(new Preset() {
                Id = 预设名,
                Content = 内容,
                Deleted = false
            });
        }

        public void 移除预设文件(string 预设名)
        {
            var collection = GetPresetCollection();
            var preset = collection.FindById(预设名);
            preset.Deleted = true;
            _ = collection.Update(preset);
        }
        #endregion

        public void Dispose()
        {
            this.database.Dispose();
        }
    }
}
