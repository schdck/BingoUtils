using BingoUtils.UI.Shared.Languages;
using BingoUtils.UI.Shared.Properties;
using BingoUtils.UI.Shared.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BingoUtils.UI.Shared.UserControls
{
    /// <summary>
    /// Interaction logic for LanguageSelector.xaml
    /// </summary>
    public partial class LanguageSelector : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _SelectedLanguage;

        public string SelectedLanguage
        {
            get
            {
                return _SelectedLanguage;
            }
            set
            {
                _SelectedLanguage = value;

                UserSettings.UserLanguage = SelectedLanguage;

                if(SelectedLanguage == LanguageLocator.Instance.CurrentLanguageName)
                {
                    SavedVisibility = Visibility.Visible;
                    MustRestartVisibility = Visibility.Collapsed;
                }
                else
                {
                    SavedVisibility = Visibility.Collapsed;
                    MustRestartVisibility = Visibility.Visible;
                }

                NotifyPropertyChanged();
                NotifyPropertyChanged("SavedVisibility");
                NotifyPropertyChanged("MustRestartVisibility");
            }
        }

        public Visibility SavedVisibility { get; private set; }

        public Visibility MustRestartVisibility { get; private set; }

        public LanguageSelector()
        {
            SelectedLanguage = UserSettings.UserLanguage;
            
            if(SavedVisibility == Visibility.Visible)
            {
                SavedVisibility = Visibility.Hidden;
            }

            DataContext = this;

            InitializeComponent();
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
