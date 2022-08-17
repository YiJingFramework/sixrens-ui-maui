using SixRens.UI.MAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Extensions
{
    public static class IWithBindingContextExtensions
    {
        public static T GetBindingContext<T>(this IWithBindingContext<T> obj)
        {
            return (T)obj.BindingContext;
        }
    }
}
