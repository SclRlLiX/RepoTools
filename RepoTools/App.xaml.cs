using System.Diagnostics;
using System.IO;
using System.Windows;

namespace RepoTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
  
        void App_Startup(object sender, StartupEventArgs e)
        {
            string? SvnArchivePath = GlobalVariables.GetSvnArchivePath();

            Debug.WriteLine("SvnArchivePath = " + SvnArchivePath);

            if (String.IsNullOrEmpty(SvnArchivePath))
            {
                string errorMessage = "Die Variable SvnArchivePath ist leer. \nDie Anwendung wird beendet.";
                ApplicationError.ShowApplicationError(errorMessage);
                Application.Current.Shutdown();
                return;
            }

            //Check if SVN Archive Folder exists 

            if (!(Directory.Exists(SvnArchivePath)))
            {
                string errorMessage = "Der Ordner [" + SvnArchivePath + "] existiert nicht. \nDie Anwendung wird beendet.";
                ApplicationError.ShowApplicationError(errorMessage);
                Application.Current.Shutdown();
                return;
            }
        }
    }

}
