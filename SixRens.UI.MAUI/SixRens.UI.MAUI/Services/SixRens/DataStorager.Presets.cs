using LiteDB;
using SixRens.Core.插件管理.预设管理;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public partial class DataStorager : I预设管理器储存器
    {
        private class Preset
        {
            public string Id { get; set; }
            public string Content { get; set; }
            public bool Deleted { get; set; }
        }
        private ILiteCollection<Preset> GetPresetCollection()
        {
            var result = database.GetCollection<Preset>("presets");
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
    }
}
