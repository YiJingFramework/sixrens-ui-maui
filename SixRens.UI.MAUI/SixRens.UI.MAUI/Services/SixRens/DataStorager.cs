using LiteDB;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.MAUI.Services.Paths;
using System.Globalization;
using System.Text;

namespace SixRens.UI.MAUI.Services.SixRens
{
    internal sealed partial class DataStorager : IDisposable
    {
        readonly LiteDatabase database;
        public DataStorager(PathProvider pathProvider)
        {
            this.database = new LiteDatabase(new ConnectionString() {
                Collation = new(lcid: 2052, CompareOptions.IgnoreCase),
                Filename = pathProvider.Database.FullName
            });
        }
        public void Dispose()
        {
            this.database.Dispose();
        }
    }
}
