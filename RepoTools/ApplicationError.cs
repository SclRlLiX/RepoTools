using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools
{
    internal class ApplicationError
    {
        public ApplicationError(string ErrorMessage)
        {
            MessageBox.Show(ErrorMessage, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            System.Windows.Application.Current.Shutdown();
            return;
        }
    }
}
