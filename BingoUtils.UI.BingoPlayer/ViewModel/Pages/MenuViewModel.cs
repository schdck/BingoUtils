using BingoUtils.Domain.Enums;
using BingoUtils.UI.BingoPlayer.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class MenuViewModel : ViewModelBase
    {
        public ICommand LaunchStartNewGameCommand { get; private set; }
        public ICommand LaunchCreateNewGameCommand { get; private set; }
        public ICommand LaunchDistributorCommand { get; private set; }
        public ICommand LaunchHelpCommand { get; private set; }
        public ICommand LaunchAboutCommand { get; private set; }

        public MenuViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LaunchStartNewGameCommand = new RelayCommand(() => BroadcastLaunchMessage(Activity.ActivityNewGame));

            LaunchCreateNewGameCommand = new RelayCommand(() => BroadcastLaunchMessage(Activity.ActivityCreateGame));

            LaunchDistributorCommand = new RelayCommand(() => BroadcastLaunchMessage(Activity.ActivityDistributor));

            LaunchHelpCommand = new RelayCommand(() => BroadcastLaunchMessage(Activity.ActivityHelp));

            LaunchAboutCommand = new RelayCommand(() => BroadcastLaunchMessage(Activity.ActivityAbout));
        }

        private void BroadcastLaunchMessage(Activity activity)
        {
            MessengerInstance.Send(new LaunchActivityMessage(activity));
        }
    }
}