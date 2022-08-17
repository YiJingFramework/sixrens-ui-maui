using SixRens.UI.MAUI.Extensions;

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
