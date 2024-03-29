﻿using Javax.Security.Auth;
using LiteDB;
using SixRens.Core.插件管理.插件包管理;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public partial class DataStorager : I插件包管理器储存器
    {
        private const string packageDeletedKey = "deleted";
        private ILiteStorage<string> GetPluginPackageStorage()
        {
            return database.GetStorage<string>("plugin_packages", "plugin_packages_chunks");
        }
        public ValueTask<string> 储存插件包文件(Stream 插件包)
        {
            var storage = GetPluginPackageStorage();
            for (; ; )
            {
                var id = Guid.NewGuid().ToString();
                if (storage.FindById(id) is not null)
                    continue;
                _ = storage.Upload(id, $"{id}.srspg", 插件包);
                return ValueTask.FromResult(id);
            }
        }

        public ValueTask 移除插件包文件(string 插件包本地识别码)
        {
            var storage = GetPluginPackageStorage();
            _ = storage.SetMetadata(插件包本地识别码, new BsonDocument() {
                [packageDeletedKey] = true
            });
            return ValueTask.CompletedTask;
        }

        public ValueTask<IEnumerable<(string 插件包本地识别码, Stream 插件包)>> 获取所有插件包文件()
        {
            IEnumerable<(string 插件包本地识别码, Stream 插件包)> SyncMethod()
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
            return ValueTask.FromResult(SyncMethod());
        }

        public ValueTask<Stream> 获取插件包文件(string 插件包本地识别码)
        {
            var storage = GetPluginPackageStorage();
            return ValueTask.FromResult((Stream)storage.FindById(插件包本地识别码)?.OpenRead());
        }
    }
}
