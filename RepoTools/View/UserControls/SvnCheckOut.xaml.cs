using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RepoTools.View.UserControls
{
    /// <summary>
    /// Interaction logic for SvnCheckOut.xaml
    /// </summary>
    public partial class SvnCheckOut : UserControl
    {
        public SvnCheckOut()
        {
            InitializeComponent();
        }

        private void btnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if(!(File.Exists(@"C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe")))
            {
                ApplicationError.ShowApplicationError(@"Die Datei [C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe] existiert nicht. Tortoise Checkout kann nicht aufgerufen werden.");
                return;
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "TortoiseProc.exe";
            processStartInfo.WorkingDirectory = @"C:\Program Files\TortoiseSVN\bin";
            processStartInfo.Arguments = $@"c\ /command:checkout /path:""C:\SVN-Archiv"" ";
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;

            Process process = new Process();
            process.StartInfo = processStartInfo;

            process.Start();

            return;
        }
    }
}
