using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace RepoTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //Enable / Disable DEBUG Mode 
        public static bool Debug = true;
        public static bool Test = false;

        //Global Variables 
        public static string? SvnArchivePath;
        public static string? SvnArchiveUrl;


        void App_Startup(object sender, StartupEventArgs e)
        {

            //Set Global Variables
            if (Debug)
            {
                SvnArchivePath = @"C:\SVN-Archiv";
                SvnArchiveUrl = @"https://WORK-VM-W11.cis-test.dcstest.de/svn/Archiv/";
            }
            else if (Test)
            {
                SvnArchivePath = @"C:\SVN-Archiv-Test";
                SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv-TEST/";
            }
            else
            {
                SvnArchivePath = @"C:\SVN-Archiv";
                SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv/";
            }


            //Check if SVN Archive Folder exists 
            if (!(Directory.Exists(SvnArchivePath)))
            {
                string errorMessage = "Der Ordner [" + SvnArchivePath + "] existiert nicht. \nDie Anwendung wird beendet.";
                _ = new ApplicationError(ErrorMessage: errorMessage);
                return;
            }


        }
    }

}
