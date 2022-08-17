using SixRens.UI.MAUI.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixRens.UI.MAUI.Views
{
    /// <summary>
    /// 和 <seealso cref="IWithBindingContextExtensions"/> 一同使用
    /// </summary>
    /// <typeparam name="TBindingContext"></typeparam>
    public interface IWithBindingContext<TBindingContext>
    {
        object BindingContext { get; }
    }
}
