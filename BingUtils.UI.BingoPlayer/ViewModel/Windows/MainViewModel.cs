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
        private ObservableCollection<MetroTabItem> _TabControlItemsBingo;
        private ObservableCollection<MetroTabItem> _TabControlItemsAnswer;
        private List<Game> _Games;

        public ObservableCollection<MetroTabItem> TabControlItemsBingo
        {
            get
            {
                return _TabControlItemsBingo;
            }
            private set
            {
                Set(ref _TabControlItemsBingo, value);
            }
        }
        public ObservableCollection<MetroTabItem> TabControlItemsAnswer
        {
            get
            {
                return _TabControlItemsAnswer;
            }
            private set
            {
                Set(ref _TabControlItemsAnswer, value);
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
            TabControlItemsBingo = new ObservableCollection<MetroTabItem>();
            TabControlItemsAnswer = new ObservableCollection<MetroTabItem>();

            Games = new List<Game>();

            AddBingoTabControlItem("Menu", new MainMenu());

            MessengerInstance.Register<LaunchActivityMessage>(this, LaunchActivity);
            MessengerInstance.Register<StartNewGameMessage>(this, AddGame);
        }

        private void LaunchNewGame()
        {
            if(TabControlItemsBingo.Count > 1 && (TabControlItemsBingo[1].Content as Frame).Content is NewGame)
            {
                // Então uma guia de novo jogo já foi criada
            }
            else
            {
                AddBingoTabControlItem("Novo jogo", new NewGame(), 1);
            }
            
            TabControlSelectedIndex = 1;
        }

        private void LaunchActivity(LaunchActivityMessage activityMessage)
        {
            switch (activityMessage.Activity)
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

        private void AddBingoTabControlItem(object header, object content, int index = 0)
        {
            TabControlItemsBingo.Insert(index, new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = false,
                Content = new Frame()
                {
                    Content = content
                }
            });

            TabControlItemsAnswer.Insert(index, new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = false,
                Content = new Frame()
                {
                    Content = new TextBlock()
                    {
                        Text = "Use a janela principal para alterar esta página.",
                        IsEnabled = false,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        FontSize = 16
                    },
                }
            });
        }

        private void AddGame(StartNewGameMessage startNewGameMessage)
        {
            var header = string.Format("Jogo #{0}", ++LaunchedGames);

            TabControlItemsBingo.Add(new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = false,
                Content = new Frame()
                {
                    Content = startNewGameMessage.GamePage
                }
            });

            TabControlItemsAnswer.Add(new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = false,
                Content = new Frame()
                {
                    Content = startNewGameMessage.GameAnswersPage
                }
            });

            TabControlSelectedIndex = TabControlItemsBingo.Count - 1;
        }
    }
}
