using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools.UtilityClasses
{
    internal static class ApplicationSuccess
    {
        public static void ShowApplicationSuccess(string SuccessMessage)
        {
            MessageBox.Show(SuccessMessage, "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
    }
}
