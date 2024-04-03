using System.Windows;

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

            // ###################################################
            // ############### FORM DEFAULT VALUES ############### 
            // ###################################################


            //Combo Box cbChooseOption
            cbChooseOption.Items.Add("Neue Paketversion");
            cbChooseOption.Items.Add("Neues Paket");
            cbChooseOption.Items.Add("Bestehende Paketversion anpassen");

            // Combo Box 
            cbChooseFolder.IsEnabled = false;
        }
    }
}