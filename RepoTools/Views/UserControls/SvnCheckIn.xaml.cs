﻿using Microsoft.VisualBasic;
using RepoTools.UtilityClasses;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RepoTools.View.UserControls
{
    /// <summary>
    /// Interaction logic for SvnCheckIn.xaml
    /// </summary>
    public partial class SvnCheckIn : UserControl
    {
        private readonly string option1 = "Neue oder bestehende Paketversion";
        private readonly string option2 = "Neues Paket";

        public SvnCheckIn()
        {
            InitializeComponent();

            //Check if .svn folder exists in C:\SVN-Archiv 
            if (Directory.Exists($@"{GlobalVariables.GetSvnArchivePath()}\.svn"))
            {
                ApplicationWarning.ShowApplicationWarning($@"HINWEIS:{Environment.NewLine}Im Ordner [{GlobalVariables.GetSvnArchivePath()}] existiert ein .svn Ordner. In diesem Verzeichnis sollte sich kein .svn Ordner befinden. {Environment.NewLine}Der .svn Ordner sollte sich im Verzeichnis des Pakets befinden [SVN-Archiv\Paketname\.svn]. Um den .svn Ordner im Datei-Explorer sehen zu können, müssen versteckte Ordner sichtbar gemacht werden.");
            }

            DefaultValues();
        }

        //Form Default Values 
        private void DefaultValues()
        {

            //Fill ComboBox cbChooseOption
            string[] options = [option1, option2];
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

            //Reset OrderId field
            lblOrderId.Content = "Auftragsnummer / INC";
            lblOrderId.Foreground = Brushes.Black;
            tbxOrderId.Text = "";

            //Reset Remark field
            lblRemark.Content = "Bemerkung";
            lblRemark.Foreground = Brushes.Black;
            tbxRemark.Text = "";

            //Reset other Fields
            cbxDcsEntw.IsChecked = true; 
            cbxDcsTest.IsChecked = false;
            cbxDcsProd.IsChecked = false;
            cbxStvmv.IsChecked = false;
            cbxSccm.IsChecked = false;
            cbxNoOrderId.IsChecked = false;

            
            tbxSoftwareVersion.Text = "";
            cbxAddToMail.IsChecked = false;
        }

        //Validate form data 
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

                if ((string)cbChooseOption.SelectedItem == option1)
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
                lblChoosePackageVersion.Content = "Paketversion wählen:";
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

                string packageDescription = tbxRemark.Text.ToString();
                packageDescription = packageDescription.Replace("\r\n", "  \r\n");

                svnCheckInObject.PackageDescription = packageDescription;
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
            if ((string)cbChooseOption.SelectedItem == option1)
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

                //Add to mail on default 
                cbxAddToMail.IsChecked = true;
            
                if ((string)cbChooseOption.SelectedItem == option1)
                {
                    //Check if JSON exists 
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
                        }
                    }

                    //For existing Package, check if repository URL matches that defined in this application 
                    string workingDir = GlobalVariables.GetSvnArchivePath() + @"\" + cbChoosePackage.SelectedItem.ToString();
                    string arguments = @"info --show-item repos-root-url";
                    SvnProcessObject svnUrlObject = SvnProcess.StartSvnProcess(workingDir, arguments);
                    if (svnUrlObject.StandardOutput?.Trim() != GlobalVariables.GetSvnArchiveUrl())
                    {
                        ApplicationWarning.ShowApplicationWarning($@"Das Paket [{cbChoosePackage.SelectedItem.ToString()}] stammt aus dem Repository [{svnUrlObject.StandardOutput?.Trim()}] und stimmt nicht mit dem Repository überein, welches in dieser Anwendung definiert wurde [{GlobalVariables.GetSvnArchiveUrl()}].{Environment.NewLine}Ist das so gewollt? Die Funktion der Anwendung wird dadurch nicht beeinträchtigt.");
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
            /*
            *  VALIDATION  
            */
            SvnCheckInObject svnCheckInObject = GetAndValidateData();
            if(svnCheckInObject.ValidationError ?? false)
            {
                return;
            }

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
            string commitMessage = $@"{BoolAsNumber.GetBoolAsNumber(svnCheckInObject.DcsEntw)};{BoolAsNumber.GetBoolAsNumber(svnCheckInObject.DcsTest)};{BoolAsNumber.GetBoolAsNumber(svnCheckInObject.DcsProd)};{BoolAsNumber.GetBoolAsNumber(svnCheckInObject.Stvmv)};{BoolAsNumber.GetBoolAsNumber(svnCheckInObject.Sccm)};{svnCheckInObject.OrderId};{svnCheckInObject.PackageDescription};{svnCheckInObject.SoftwareVersion};{svnCheckInObject.RepoFolder};{svnCheckInObject.PackageName};{svnCheckInObject.PackageVersion}";
            commitMessage = SanitizeText.GetSanitizedText(commitMessage);

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
                string arguments = @$"add .\{svnCheckInObject.PackageVersion} --force";
                SvnProcessObject svnAdd = SvnProcess.StartSvnProcess(workingDir, arguments);

                if (svnAdd.ExitCode != 0)
                {
                    string errorMessage = $@"Das Hinzufügen der Version [{svnCheckInObject.PackageVersion}] aus dem Paket [{svnCheckInObject.PackageName}] ist fehlgeschlagen.";
                    ApplicationError.ShowApplicationError(errorMessage);
                    return;
                }

                //add .buildmsi.zip folder 
                if(File.Exists($@"{GlobalVariables.GetSvnArchivePath()}\{svnCheckInObject.PackageName}\{svnCheckInObject.PackageVersion}.buildmsi.zip"))
                {
                    arguments = @$"add .\{svnCheckInObject.PackageVersion}.buildmsi.zip --force";
                    SvnProcessObject svnAddBuildmsi = SvnProcess.StartSvnProcess(workingDir, arguments);

                    if (svnAddBuildmsi.ExitCode != 0)
                    {
                        string errorMessage = $@"Das Hinzufügen der neuen Dateien (svn add) aus dem Paket [{svnCheckInObject.PackageName}] ist fehlgeschlagen.";
                        ApplicationError.ShowApplicationError(errorMessage);
                        return;
                    }
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

            if(svnCheckInObject.Sccm)
            {
                string driveOMessage =
                $@"SCCM wurde als Umgebung angegeben, soll das Paket [{svnCheckInObject.PackageName} {svnCheckInObject.PackageVersion}] ins Laufwek O kopiert werden?";
                MessageBoxResult driveOResult = ApplicationInfoYesNo.GetApplicationInfoYesNo(driveOMessage);

                if (driveOResult == MessageBoxResult.Yes)
                {
                    //Create Package Folder inside drive O 
                    var sourceDirectoryInfo = new DirectoryInfo($@"{GlobalVariables.GetSvnArchivePath()}\{svnCheckInObject.PackageName}\{svnCheckInObject.PackageVersion}");
                    var targetDirectoryInfo = new DirectoryInfo($@"{GlobalVariables.GetDriveO()}\{svnCheckInObject.PackageName}\{svnCheckInObject.PackageVersion}");

                    if(CopyFiles.StartCopyFiles(sourceDirectoryInfo,targetDirectoryInfo))
                    {
                        ApplicationSuccess.ShowApplicationSuccess($@"Das Paket {svnCheckInObject.PackageName} {svnCheckInObject.PackageVersion} wurde erfolgreich ins Laufwerk O kopiert.");
                    }
                    else
                    {
                        ApplicationWarning.ShowApplicationWarning($@"Das Paket {svnCheckInObject.PackageName} {svnCheckInObject.PackageVersion} konnte nicht ins Laufwerk O kopiert werden. Das Paket muss manuell kopiert werden.");
                    }
                }
            }

            //Delete Package Folder 
            string infoMessage =
$@"Das Paket [{svnCheckInObject.PackageName}] wurde erfolgreich hinzugefügt bzw. aktualisiert. {Environment.NewLine} 
Soll das Paket [{svnCheckInObject.PackageName}] aus dem Ordner [{GlobalVariables.GetSvnArchivePath()}] entfernt werden?"; 
            MessageBoxResult result = ApplicationInfoYesNo.GetApplicationInfoYesNo(infoMessage);

            if(result == MessageBoxResult.Yes)
            {
                //Delete Package directory via cmd command to circumvent System.UnauthorizedAccessException on .svn folder
                string deletePackagePath = GlobalVariables.GetSvnArchivePath() + @"\" + svnCheckInObject.PackageName;
                CmdProcess.StartCmdProcess($@"/c rmdir /s/q {deletePackagePath}");

                if (Directory.Exists(deletePackagePath))
                {
                    string warningMessage = "Der Ordner [" + deletePackagePath + "] existiert noch. Beim löschen des Ordners ist ein Fehler aufgetreten. Der Ordner muss manuell gelöscht werden.";
                    ApplicationWarning.ShowApplicationWarning(warningMessage);
                }
                else
                {
                    string successMessage = "Der Ordner [" + deletePackagePath + "] wurde erfolgreich entfernt.";
                    ApplicationSuccess.ShowApplicationSuccess(successMessage);
                }
            }

            //Set form default values 
            DefaultValues();
            return; 

        }

        //Reset all fields
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DefaultValues();
            return;
        }
    }
}
