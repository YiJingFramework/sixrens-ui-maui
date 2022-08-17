using SixRens.UI.MAUI.Views;

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
