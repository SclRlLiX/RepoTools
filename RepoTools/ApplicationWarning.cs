using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools
{
    internal static class ApplicationWarning
    {
        public static void ShowApplicationWarning(string WarningMessage) 
        {
            MessageBox.Show(WarningMessage, "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
    }
}
