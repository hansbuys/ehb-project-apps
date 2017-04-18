using System.Windows.Input;

namespace Ehb.Dijlezonen.Kassa.App.Testing
{
    public static class CommandExtensions
    {
        public static void Click(this ICommand command, object parameter = null)
        {
            if (command.CanExecute(parameter))
                command.Execute(parameter);
        }
    }
}
