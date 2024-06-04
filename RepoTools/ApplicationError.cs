using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools
{
    internal static class ApplicationError
    {
        public static void ShowApplicationError(string ErrorMessage)
        {
            MessageBox.Show(ErrorMessage, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }
}
