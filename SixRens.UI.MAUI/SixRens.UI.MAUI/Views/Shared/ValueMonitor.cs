using SixRens.UI.MAUI.Extensions;
using System.Windows.Input;

namespace SixRens.UI.MAUI.Views.Shared
{
    public class ValueMonitor<T> : ContentView
    {
        public static readonly BindableProperty MonitoringValueProperty =
            BindableProperty.Create(
                nameof(MonitoringValue),
                typeof(T),
                typeof(ValueMonitor<T>),
                defaultValue: default(T),
                propertyChanged: OnChanged);

        public T MonitoringValue
        {
            get => (T)GetValue(MonitoringValueProperty);
            set => SetValue(MonitoringValueProperty, value);
        }

        public event EventHandler<ChangedValue> ValueChanged;

        public sealed record ChangedValue(T OldValue, T NewValue);

        private static void OnChanged(
            BindableObject bindable, object oldValue, object newValue)
        {
            var monitor = (ValueMonitor<T>)bindable;
            monitor.ValueChanged?.Invoke(monitor, new((T)oldValue, (T)newValue));
        }
    }
}
