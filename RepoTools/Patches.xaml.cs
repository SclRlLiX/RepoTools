using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace RepoTools
{
    /// <summary>
    /// Interaction logic for Patches.xaml
    /// </summary>
    public partial class Patches : Window
    {
        public Patches()
        {
            InitializeComponent();

            tbxCurrentVersion.Text = $@"Aktuelle Version { FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion}";
        }
    }
}
