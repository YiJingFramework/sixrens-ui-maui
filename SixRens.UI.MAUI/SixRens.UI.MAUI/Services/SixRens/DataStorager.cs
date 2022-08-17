using LiteDB;
using SixRens.UI.MAUI.Services.Paths;
using System.Globalization;

namespace SixRens.UI.MAUI.Services.SixRens
{
    public sealed partial class DataStorager : IDisposable
    {
        private readonly LiteDatabase database;
        internal DataStorager(PathProvider pathProvider)
        {
            database = new LiteDatabase(new ConnectionString() {
                Collation = new(lcid: 2052, CompareOptions.IgnoreCase),
                Filename = pathProvider.Database.FullName
            });
        }
        public void Dispose()
        {
            database.Dispose();
        }
    }
}
