using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace RepoTools.View.UserControls
{
    /// <summary>
    /// Interaction logic for ComboBoxWithLabel.xaml
    /// </summary>
    public partial class ComboBoxWithLabel : UserControl, INotifyPropertyChanged
    {
        public ComboBoxWithLabel()
        {
            DataContext = this; 
            InitializeComponent();
        }

        private string? labelText;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string? LabelText
        {
            get { return labelText; }
            set 
            { 
                if(!(String.IsNullOrEmpty(value)))
                {
                    labelText = value;
                    OnPropertyChanged();
                }
                else
                {
                    labelText = "Platzhalter Text";
                    OnPropertyChanged();
                }
                
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

    }
}
