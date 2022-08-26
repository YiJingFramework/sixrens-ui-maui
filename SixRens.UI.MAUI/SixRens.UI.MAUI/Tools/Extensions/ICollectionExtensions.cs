using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Tools.Extensions
{
    public static class ICollectionExtensions
    {
        public static void AddOneByOne<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            foreach (var value in values)
                collection.Add(value);
        }
    }
}
