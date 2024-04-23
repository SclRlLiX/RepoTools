using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;

namespace RepoTools.View.UserControls
{
    /// <summary>
    /// Interaction logic for SvnCheckIn.xaml
    /// </summary>
    public partial class SvnCheckIn : UserControl
    {
        private readonly string option1 = "Neue Paketversion";
        private readonly string option2 = "Neues Paket";
        private readonly string option3 = "Bestehende Paketversion anpassen";

        public SvnCheckIn()
        {
            InitializeComponent();

            DefaultValues();
        }

        //Form Default Values 
        private void DefaultValues()
        {

            //Fill ComboBox cbChooseOption
            string[] options = [option1, option2, option3];
            cbChooseOption.ItemsSource = options;

            // Disable all other fields
            cbChooseFolder.IsEnabled = false;
            cbChoosePackage.IsEnabled = false;
            cbChoosePackageVersion.IsEnabled = false;
            cbxDcsEntw.IsEnabled = false;
            cbxDcsTest.IsEnabled = false;
            cbxDcsProd.IsEnabled = false;
            cbxStvmv.IsEnabled = false;
            cbxSccm.IsEnabled = false;
            tbxOrderId.IsEnabled = false;
            tbxRemark.IsEnabled = false;
            tbxSoftwareVersion.IsEnabled = false;
            cbxAddToMail.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnSubmit.IsEnabled = false;

            //Reset ChooseFolder field
            cbChooseFolder.ItemsSource = null;

            //Reset ChoosePackage field
            lblChoosePackage.Content = "Paket wählen";
            lblChoosePackage.Foreground = Brushes.Black;
            cbChoosePackage.ItemsSource = null;

            //Reset ChoosePackageVersion field
            lblChoosePackageVersion.Content = "Paketversion wählen:";
            lblChoosePackageVersion.Foreground = Brushes.Black;
            cbChoosePackageVersion.ItemsSource = null;
        }


        //cbChooseOption 
        private void cbChooseOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Load Default Values
            DefaultValues();

            //Get Packages from SVN-Archiv Folder 
            ArrayList packageFoldersWithoutSvnFolder = SvnArchivFolderPackages.GetPackagesWithoutSvnFolder();
            ArrayList packageFoldersWithSvnFolder = SvnArchivFolderPackages.GetPackagesWithSvnFolder();

            //Display Packages that already exist in SVN Repo
            if ((string)cbChooseOption.SelectedItem == option1 || (string)cbChooseOption.SelectedItem == option3)
            {
                // Enable - Disable fields
                cbChooseFolder.IsEnabled = false;
                cbChoosePackage.IsEnabled = true;

                //Clear cbChooseFolder
                cbChooseFolder.ItemsSource = null;

                //Display Packages from SVN Archiv Folder (with .svn Folder)
                if (packageFoldersWithSvnFolder.Count > 0)
                {
                    cbChoosePackage.ItemsSource = packageFoldersWithSvnFolder;

                    lblChoosePackage.Content = "Paket wählen";
                    lblChoosePackage.Foreground = Brushes.Black;
                }
                else
                {
                    lblChoosePackage.Content = "Kein(e) Paket(e) vorhanden! ";
                    lblChoosePackage.Foreground = Brushes.Red;
                }

            }

            //Display Packages that do not exist in SVN Repo
            if ((string)cbChooseOption.SelectedItem == option2)
            {
                // Enable next fields 
                cbChooseFolder.IsEnabled = true;
                cbChoosePackage.IsEnabled = true;

                //Display all Repo Folders
                cbChooseFolder.ItemsSource = RepoFolders.GetRepoFolders();

                //Display Packages from SVN Archiv Folder (without .svn Folder)
                if (packageFoldersWithoutSvnFolder.Count > 0)
                {
                    cbChoosePackage.ItemsSource = packageFoldersWithoutSvnFolder;

                    lblChoosePackage.Content = "Paket wählen";
                    lblChoosePackage.Foreground = Brushes.Black;
                }
                else
                {
                    lblChoosePackage.Content = "Kein(e) Paket(e) vorhanden! ";
                    lblChoosePackage.Foreground = Brushes.Red;
                }
            }



        }

        //cbChoosePackage
        private void cbChoosePackage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbChoosePackage.Items.Count > 0)
            {
                //Enable and fill PackageVersion Field
                cbChoosePackageVersion.IsEnabled = true;
                ArrayList svnArchivFolderPackageVersions = SvnArchivFolderPackageVersions.GetPackageVersions(cbChoosePackage.SelectedValue.ToString());
                if (svnArchivFolderPackageVersions.Count > 0)
                {
                    cbChoosePackageVersion.ItemsSource = svnArchivFolderPackageVersions;
                    lblChoosePackageVersion.Content = "Paketversion wählen:";
                    lblChoosePackageVersion.Foreground = Brushes.Black;
                }
                else
                {
                    lblChoosePackageVersion.Content = "Keine Version Verfügbar!";
                    lblChoosePackageVersion.Foreground = Brushes.Red;
                }

            }

        }

        //cbChoosePackageVersion
        private void cbChoosePackageVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbChoosePackageVersion.Items.Count > 0)
            {
                //Enable all other fields
                cbxDcsEntw.IsEnabled = true;
                cbxDcsTest.IsEnabled = true;
                cbxDcsProd.IsEnabled = true;
                cbxStvmv.IsEnabled = true;
                cbxSccm.IsEnabled = true;
                tbxOrderId.IsEnabled = true;
                tbxRemark.IsEnabled = true;
                tbxSoftwareVersion.IsEnabled = true;
                cbxAddToMail.IsEnabled = true;
                btnCancel.IsEnabled = true;
                btnSubmit.IsEnabled = true;
            }

        }
    }
}
