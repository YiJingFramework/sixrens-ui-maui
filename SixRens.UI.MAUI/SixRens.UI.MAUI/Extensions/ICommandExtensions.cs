using System.Windows.Input;

namespace SixRens.UI.MAUI.Extensions
{
    public static class ICommandExtensions
    {
        public static void CheckAndExecute(this ICommand command, object parameters = null)
        {
            lock (command)
            {
                if (command.CanExecute(parameters))
                    command.Execute(parameters);
            }
        }
    }
}
