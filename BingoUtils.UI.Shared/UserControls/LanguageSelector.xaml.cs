using BingoUtils.UI.Shared.Languages;
using BingoUtils.UI.Shared.Settings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
