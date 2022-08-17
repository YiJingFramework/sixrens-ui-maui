using LiteDB;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Services.SixRens
{
    partial class DataStorager
    {
        private class Case
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
            public bool Deleted { get; set; }
        }
        private ILiteCollection<Case> GetCaseCollection()
        {
            var result = this.database.GetCollection<Case>("cases");
            return result;
        }
        public IEnumerable<(Guid id, string name, string content)> ListCases()
        {
            var collection = GetCaseCollection();
            foreach (var dCase in collection.FindAll())
            {
                if (!dCase.Deleted)
                    yield return (dCase.Id, dCase.Name, dCase.Content);
            }
        }
        public Guid CreateCase(string name, string content)
        {
            var collection = GetCaseCollection();
            var id = collection.Insert(new Case() {
                Name = name,
                Content = content,
                Deleted = false
            });
            return id.AsGuid;
        }
        public void UpdateCase(Guid id, string name, string content)
        {
            var collection = GetCaseCollection();
            _ = collection.Update(new Case() {
                Id = id,
                Content = content,
                Name = name,
                Deleted = false
            });
        }
        public void RemoveCase(Guid id)
        {
            var collection = GetCaseCollection();
            var preset = collection.FindById(id);
            preset.Deleted = true;
            _ = collection.Update(preset);
        }
    }
}
