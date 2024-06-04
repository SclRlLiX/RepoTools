using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private readonly string option3 = "Paketversion anpassen";

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
            cbxNoOrderId.IsEnabled = false;
            tbxRemark.IsEnabled = false;
            tbxSoftwareVersion.IsEnabled = false;
            cbxAddToMail.IsEnabled = false;
            btnCancel.IsEnabled = false;
            btnSubmit.IsEnabled = false;

            //Reset ChooseFolder field
            cbChooseFolder.ItemsSource= null; 
            cbChooseFolder.Items.Clear();
            //Fill Combo Box with Repo Folders
            cbChooseFolder.ItemsSource = RepoFolders.GetRepoFolders();

            //Reset ChoosePackage field
            lblChoosePackage.Content = "Paket wählen";
            lblChoosePackage.Foreground = Brushes.Black;
            cbChoosePackage.ItemsSource = null;

            //Reset ChoosePackageVersion field
            lblChoosePackageVersion.Content = "Paketversion wählen:";
            lblChoosePackageVersion.Foreground = Brushes.Black;
            cbChoosePackageVersion.ItemsSource = null;

            //Reset other Fields
            cbxDcsEntw.IsChecked = true; 
            cbxDcsTest.IsChecked = false;
            cbxDcsProd.IsChecked = false;
            cbxStvmv.IsChecked = false;
            cbxSccm.IsChecked = false;

            tbxOrderId.Text = "";
            tbxRemark.Text = "";
            tbxSoftwareVersion.Text = "";
            cbxAddToMail.IsChecked = false;
        }

        private SvnCheckInObject GetAndValidateData()
        {
            SvnCheckInObject svnCheckInObject = new();

            //cbChooseOption
            if (String.IsNullOrEmpty(cbChooseOption.Text.ToString()))
            {
                lblChooseOption.Content = "Bitte Option wählen";
                lblChooseOption.Foreground = Brushes.Red;
                lblChooseOption.FontWeight = FontWeights.Bold;
                svnCheckInObject.ValidationError = true; 
            }
            else
            {
                lblChooseOption.Content = "Option wählen:";
                lblChooseOption.Foreground = Brushes.Black;
                lblChooseOption.FontWeight = FontWeights.Bold;

                if ((string)cbChooseOption.SelectedItem == option1 || (string)cbChooseOption.SelectedItem == option3)
                {
                    svnCheckInObject.NewPackage = false; 
                }
                else
                {
                    svnCheckInObject.NewPackage = true;
                }

            }

            //cbChooseFolder
            if (String.IsNullOrEmpty(cbChooseFolder.Text.ToString()))
            {
                lblChooseFolder.Content = "Bitte Ordner wählen:";
                lblChooseFolder.Foreground = Brushes.Red;
                svnCheckInObject.ValidationError = true;
            }
            else
            {
                lblChooseFolder.Content = "Ordner wählen:";
                lblChooseFolder.Foreground = Brushes.Black;

                svnCheckInObject.RepoFolder = cbChooseFolder.Text.ToString();
            }

            //cbChoosePackage
            if (String.IsNullOrEmpty(cbChoosePackage.Text.ToString()))
            {
                lblChoosePackage.Content = "Bitte Paket wählen:";
                lblChoosePackage.Foreground = Brushes.Red;
                svnCheckInObject.ValidationError = true;
            }
            else
            {
                lblChoosePackage.Content = "Paket wählen:";
                lblChoosePackage.Foreground = Brushes.Black;

                svnCheckInObject.PackageName = cbChoosePackage.Text.ToString();
            }

            //cbChoosePackageVersion
            if (String.IsNullOrEmpty(cbChoosePackageVersion.Text.ToString()))
            {
                lblChoosePackageVersion.Content = "Bitte Paketversion wählen:";
                lblChoosePackageVersion.Foreground = Brushes.Red;
                svnCheckInObject.ValidationError = true;
            }
            else
            {
                lblChoosePackageVersion.Content = "Ordner Paketversion:";
                lblChoosePackageVersion.Foreground = Brushes.Black;

                svnCheckInObject.PackageVersion = cbChoosePackageVersion.Text.ToString();
            }

            //Environments
            if(cbxDcsEntw.IsChecked == false && cbxDcsTest.IsChecked == false && cbxDcsProd.IsChecked == false && cbxStvmv.IsChecked == false && cbxSccm.IsChecked == false)
            {
                string warningMessage = "Es muss mindestens eine Umgebung ausgewählt werden.";
                ApplicationWarning.ShowApplicationWarning(warningMessage);
                svnCheckInObject.ValidationError = true; 
            }
            else
            {
                //Convert bool? to bool
                svnCheckInObject.DcsEntw = (bool)(cbxDcsEntw.IsChecked.HasValue ? cbxDcsEntw.IsChecked : false);
                svnCheckInObject.DcsTest = (bool)(cbxDcsTest.IsChecked.HasValue ? cbxDcsTest.IsChecked : false);
                svnCheckInObject.DcsProd = (bool)(cbxDcsProd.IsChecked.HasValue ? cbxDcsProd.IsChecked : false);
                svnCheckInObject.Stvmv = (bool)(cbxStvmv.IsChecked.HasValue ? cbxStvmv.IsChecked : false);
                svnCheckInObject.Sccm = (bool)(cbxSccm.IsChecked.HasValue ? cbxSccm.IsChecked : false);
            }

            //tbxOrderId
            if (String.IsNullOrEmpty(tbxOrderId.Text.ToString()))
            {
                lblOrderId.Content = "Auftrag oder INC angeben";
                lblOrderId.Foreground = Brushes.Red;
                svnCheckInObject.ValidationError = true;
            }
            else
            {
                lblOrderId.Content = "Auftragsnummer / INC";
                lblOrderId.Foreground = Brushes.Black;

                svnCheckInObject.OrderId = tbxOrderId.Text.ToString();
            }

            //tbxRemark
            if (String.IsNullOrEmpty(tbxRemark.Text.ToString()))
            {
                lblRemark.Content = "Bemerkung angeben";
                lblRemark.Foreground = Brushes.Red;
                svnCheckInObject.ValidationError = true;
            }
            else
            {
                lblRemark.Content = "Bemerkung";
                lblRemark.Foreground = Brushes.Black;

                svnCheckInObject.PackageDescription = tbxRemark.Text.ToString();
            }

            //tbxSoftwareVersion
            if (!(String.IsNullOrEmpty(tbxSoftwareVersion.Text.ToString())))
            {
                svnCheckInObject.SoftwareVersion = tbxSoftwareVersion.Text.ToString();
            }
                
            //cbxAddToMail
            svnCheckInObject.AddToMail = cbxAddToMail.IsChecked;


            return svnCheckInObject;
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

                //Get Repo Folder from URL if Package has .svn Folder 
                if(!(cbChooseFolder.IsEnabled))
                {
                    //Get current URL and set Folder
                    string workingDir = GlobalVariables.GetSvnArchivePath() + @"\" + cbChoosePackage.SelectedItem.ToString();
                    string arguments = "info --show-item url .";
                    SvnProcessObject svnProcessObject = SvnProcess.StartSvnProcess(workingDir, arguments);

                    if (svnProcessObject.StandardOutput != null) 
                    {
                        string url = svnProcessObject.StandardOutput.Split("/")[^2];
                        if(!(cbChooseFolder.Items.Contains(url)))
                        {
                            cbChooseFolder.Items.Clear();
                            cbChooseFolder.Items.Add(url);
                        }
                        cbChooseFolder.Text = url;
                        Debug.WriteLine("URL " + url);
                    }

                    Debug.WriteLine(svnProcessObject.StandardOutput);
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
                cbxNoOrderId.IsEnabled = true;
                tbxRemark.IsEnabled = true;
                tbxSoftwareVersion.IsEnabled = true;
                cbxAddToMail.IsEnabled = true;
                btnCancel.IsEnabled = true;
                btnSubmit.IsEnabled = true;
            }

            //Check if JSON exists 
            if ((string)cbChooseOption.SelectedItem == option1 || (string)cbChooseOption.SelectedItem == option3)
            {
                string jsonFilePath = $@"{GlobalVariables.GetSvnArchivePath()}\{cbChoosePackage.SelectedItem}\{cbChoosePackageVersion.SelectedItem}\{GlobalVariables.JsonFileName}";
                if(File.Exists(jsonFilePath))
                {
                    string jsonString = File.ReadAllText(jsonFilePath);
                    SvnCheckInObject? svnCheckInObject = JsonSerializer.Deserialize<SvnCheckInObject>(jsonString);
                    if(svnCheckInObject != null)
                    {
                        //Fill form fields
                        cbxDcsEntw.IsChecked = svnCheckInObject.DcsEntw;
                        cbxDcsTest.IsChecked = svnCheckInObject.DcsTest;
                        cbxDcsProd.IsChecked = svnCheckInObject.DcsProd;
                        cbxStvmv.IsChecked = svnCheckInObject.Stvmv;
                        cbxSccm.IsChecked = svnCheckInObject.Sccm;

                        tbxOrderId.Text = svnCheckInObject.OrderId;
                        tbxRemark.Text = svnCheckInObject.PackageDescription;
                        tbxSoftwareVersion.Text = svnCheckInObject.SoftwareVersion;
                        cbxAddToMail.IsChecked = svnCheckInObject.AddToMail;
                    }

                    
                }
            }
        }

        //Events for checkbox cbxNoOrderId
        private void cbxNoOrderId_Checked(object sender, RoutedEventArgs e)
        {
            tbxOrderId.Text = GlobalVariables.EmptyTextboxText;
            tbxOrderId.IsEnabled= false;
        }
        private void cbxNoOrderId_Unchecked(object sender, RoutedEventArgs e)
        {
            tbxOrderId.Text = "";
            tbxOrderId.IsEnabled = true; 
        }


        // Submit Form
        private void btnSubmit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SvnCheckInObject svnCheckInObject = GetAndValidateData();

            //Check if Path to Package Version exists
            string pathToCurrentPackageVersion = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName + @"\" + svnCheckInObject.PackageVersion;
            if(!(Directory.Exists(pathToCurrentPackageVersion)))
            {
                string warningMessage = "Der Ordner [" + pathToCurrentPackageVersion + "] existiert nicht.";
                ApplicationWarning.ShowApplicationWarning(warningMessage);
            }


            /*
            *  CREATE README AND JSON  
            */
            ReadMe readMe = new();
            readMe.SvnCheckInObject = svnCheckInObject;
            readMe.PathToPackageVersion = pathToCurrentPackageVersion;

            //Only continue if success is returned
            //Write readme file
            //Error handling is done inside methods
            if(!(readMe.CreateAndSaveReadme()))
            {
                return; 
            }
            //Write Json file 
            if (!(readMe.CreateAndSaveJson()))
            {
                return;
            }


            /*
            *  ZIP .BUILDMSI Folder   
            */
            string buildmsiFilePath = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName + @"\" + svnCheckInObject.PackageVersion + ".buildmsi";
            if(Directory.Exists(buildmsiFilePath))
            {
                string zipFilePath = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName + @"\" + svnCheckInObject.PackageVersion + ".buildmsi.zip";

                // Check if the ZIP file already exists and delete it if it does
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }

                // Create the ZIP file from the folder
                ZipFile.CreateFromDirectory(buildmsiFilePath, zipFilePath);

                //Delete original .buildmsi directory
                Directory.Delete(buildmsiFilePath, recursive: true);

                if(Directory.Exists(buildmsiFilePath))
                {
                    string warningMessage = "Der Ordner [" + buildmsiFilePath + "] existiert noch. Beim zippen und löschen des .buildmsi Ordners ist ein Fehler aufgetreten. Das Einlagern wurde abgebrochen.";
                    ApplicationWarning.ShowApplicationWarning(warningMessage);
                    return;
                }

                if (!(File.Exists(zipFilePath)))
                {
                    string warningMessage = "Der Ordner [" + zipFilePath + "] existiert nicht. Beim zippen und löschen des .buildmsi Ordners ist ein Fehler aufgetreten. Das Einlagern wurde abgebrochen.";
                    ApplicationWarning.ShowApplicationWarning(warningMessage);
                }
            }


            /*
            *  Commit Package to SVN    
            */

            //Build commit string (set to flase if null) 
            string commitMessage = $@"{svnCheckInObject.DcsEntw};{svnCheckInObject.DcsTest};{svnCheckInObject.DcsProd};{svnCheckInObject.Stvmv};{svnCheckInObject.Sccm};{svnCheckInObject.OrderId};{svnCheckInObject.PackageDescription};{svnCheckInObject.SoftwareVersion};{svnCheckInObject.RepoFolder};{svnCheckInObject.PackageName};{svnCheckInObject.PackageVersion}";
            commitMessage = commitMessage.Replace("\"", "'");

            if (cbxAddToMail.IsChecked ?? false)
            {
                commitMessage = $@"automail4;{commitMessage}";
            }
            else
            {
                commitMessage = $@"nomail;{commitMessage}";
            }

            //Commit Package 
            if (svnCheckInObject.NewPackage ?? false)
            {
                //new Package 
                string workingDir = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName;
                string arguments = $@"import . ""{GlobalVariables.GetSvnArchiveUrl()}/{svnCheckInObject.RepoFolder}/{svnCheckInObject.PackageName}"" -m ""{commitMessage}"" ";
                SvnProcessObject svnProcessObject = SvnProcess.StartSvnProcess(workingDir, arguments);

                if (svnProcessObject.ExitCode != 0)
                {
                    string errorMessage = $@"Das Hinzufügen des Pakets [{svnCheckInObject.PackageName}] ins Repository ist fehlgeschlagen.";
                    ApplicationError.ShowApplicationError(errorMessage);
                    return;
                }
            }
            else
            {
                //existing Package

                //add new Folders/Files 
                string workingDir = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName;
                string arguments = "add . --force";
                SvnProcessObject svnAdd = SvnProcess.StartSvnProcess(workingDir, arguments);

                if (svnAdd.ExitCode != 0)
                {
                    string errorMessage = $@"Das Hinzufügen der neuen Dateien (svn add) aus dem Paket [{svnCheckInObject.PackageName}] ist fehlgeschlagen.";
                    ApplicationError.ShowApplicationError(errorMessage);
                    return;
                }

                //Commit changes to repo 
                arguments = $@"commit . -m ""{commitMessage}"" ";
                SvnProcessObject svnCommit = SvnProcess.StartSvnProcess(workingDir, arguments);

                if (svnCommit.ExitCode != 0)
                {
                    string errorMessage = $@"Das Aktualisieren des Pakets [{svnCheckInObject.PackageName}] im Repository ist fehlgeschlagen.";
                    ApplicationError.ShowApplicationError(errorMessage);
                    return;
                }
            }

            ApplicationSuccess.ShowApplicationSuccess("Das Paket wurde erfolgreich hinzugefügt / aktualisiert.");


        }
    }
}
