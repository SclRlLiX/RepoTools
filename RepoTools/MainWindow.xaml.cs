using RepoTools.View.UserControls;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Reflection;
using RepoTools.UtilityClasses;

namespace RepoTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            string title = $@"RepoTools {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion} - {GlobalVariables.GetSvnArchiveUrl()}";

            if (GlobalVariables.Debug)
            {
                this.Title = $@"{title} - DEBUG MODE";
            }
            else if(GlobalVariables.Test)
            {
                this.Title = $@"{title} - TEST MODE";
            }
            else
            {
                this.Title = title;
            }

            //Open SvnCheckIn Control on default 
            CC.Content = new SvnCheckIn();
            lblBtnCheckin.FontWeight = FontWeights.Bold;
            lblBtnCheckout.FontWeight = FontWeights.Regular;
        }

        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            //Open Tortoise Checkout 
            if (!(File.Exists(@"C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe")))
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

            //Close Form while Tortoise is open
            process.Start();
            this.WindowState = WindowState.Minimized;

            //Open file explorer if process completed successfully, otherwise repoen form 
            process.WaitForExit();
            if(process.ExitCode != 0)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                Process.Start("explorer.exe", $@"{GlobalVariables.GetSvnArchivePath()}");
            }
           
            return;

        }

        //CheckIn
        private void btnCheckin_Click(object sender, RoutedEventArgs e)
        {
            CC.Content = new SvnCheckIn();
            lblBtnCheckin.FontWeight = FontWeights.Bold;
            lblBtnCheckout.FontWeight = FontWeights.Regular;
        }

        //Repo Browser
        private void btnRepoBrowser_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo($@"{GlobalVariables.GetSvnArchiveUrl()}") { UseShellExecute = true });
        }


        private void btnTutorial_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo($@"{GlobalVariables.GetTutorialPathUrl()}") { UseShellExecute = true });
        }

            private void btnPatches_Click(object sender, RoutedEventArgs e)
        {
            new Patches().Show();
        }

    }
}