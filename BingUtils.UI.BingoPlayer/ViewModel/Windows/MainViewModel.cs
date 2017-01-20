using BingoUtils.Domain.Entities;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Command;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.Domain.Enums;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Windows
{
    public class MainViewModel : BaseViewModel
    {
        private int LaunchedGames;
        private int _TabControlSelectedIndex;
        private ObservableCollection<MetroTabItem> _TabControlItems;
        private List<Game> _Games;

        public ObservableCollection<MetroTabItem> TabControlItems
        {
            get
            {
                return _TabControlItems;
            }
            private set
            {
                Set(ref _TabControlItems, value);
            }
        }
        public List<Game> Games
        {
            get
            {
                return _Games;
            }
            set
            {
                Set(ref _Games, value);
            }
        }
        public int TabControlSelectedIndex
        {
            get
            {
                return _TabControlSelectedIndex;
            }
            set
            {
                Set(ref _TabControlSelectedIndex, value);
            }
        }

        public MainViewModel()
        {
            TabControlItems = new ObservableCollection<MetroTabItem>();
            Games = new List<Game>();

            AddTabControlItem("Menu", new MainMenu());

            MessengerInstance.Register<LaunchActivityMessage>(this, LaunchActivity);
            MessengerInstance.Register<StartNewGameMessage>(this, StartNewGame);
        }

        private void AddTabControlItem(object header, object content, int index = -1)
        {
            if(index < 0)
            {
                TabControlItems.Add(new MetroTabItem()
                {
                    Header = header,
                    CloseButtonEnabled = false,
                    Content = new Frame()
                    {
                        Content = content
                    }
                });
            }
            else
            {
                TabControlItems.Insert(index, new MetroTabItem()
                {
                    Header = header,
                    CloseButtonEnabled = false,
                    Content = new Frame()
                    {
                        Content = content
                    }
                });
            }
        }

        private void StartNewGame(StartNewGameMessage startNewGameMessage)
        {
            AddTabControlItem(string.Format("Jogo #{0}", ++LaunchedGames), startNewGameMessage.GamePage);
            TabControlSelectedIndex = TabControlItems.Count - 1;
        }

        private void LaunchActivity(LaunchActivityMessage activityMessage)
        {
            switch(activityMessage.Activity)
            {
                case Activity.ActivityNewGame:
                    LaunchNewGame();
                    break;
                case Activity.ActivityCreateGame:

                    break;
                case Activity.ActivityDistributor:

                    break;
                case Activity.ActivityHelp:

                    break;
                case Activity.ActivityAbout:

                    break;
            }
        }

        private void LaunchNewGame()
        {
            if(TabControlItems.Count > 1 && (TabControlItems[1].Content as Frame).Content is NewGame)
            {
                // Então uma guia de novo jogo já foi criada
            }
            else
            {
                AddTabControlItem("Novo jogo", new NewGame(), 1);
            }
            
            TabControlSelectedIndex = 1;
        }
    }
}
