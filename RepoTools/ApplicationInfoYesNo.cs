using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools
{
    internal static class ApplicationInfoYesNo
    {
        public static MessageBoxResult GetApplicationInfoYesNo(string InfoMessage)
        {
            MessageBoxResult result =  MessageBox.Show(InfoMessage, "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            return result;
        }
    }
}
