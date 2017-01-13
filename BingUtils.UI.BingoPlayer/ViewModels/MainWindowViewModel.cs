using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.Helpers.BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Pages;
using BingoUtils.UI.BingoPlayer.Resources;
using MahApps.Metro.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace BingoUtils.UI.BingoPlayer.ViewModels
{
    [ImplementPropertyChanged]
    public class MainWindowViewModel : DefaultViewModel
    {
        public ObservableCollection<MetroTabItem> TabControlItems { get; private set; }

        public MainMenu MainMenu { get; private set; }
        public List<Game> Games { get; private set; }

        public MainWindowViewModel()
        {
            TabControlItems = new ObservableCollection<MetroTabItem>();
            Games = new List<Game>();

            MainMenu = new MainMenu();
            Games.Add(new Game());

            AddTabControlItem("Menu", MainMenu);
            AddTabControlItem("Game1", Games[0]);
            AddTabControlItem("New game", new StartNewGame());
        }

        private void AddTabControlItem(string header, Page content)
        {
            TabControlItems.Add(new MetroTabItem()
            {
                Header = header,
                Content = new Frame()
                {
                    Content = content
                }
            });

        }
    }
}
