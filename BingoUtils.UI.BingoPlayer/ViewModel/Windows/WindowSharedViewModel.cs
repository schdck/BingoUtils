using BingoUtils.Domain.Entities;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.Domain.Enums;
using System;
using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using BingoUtils.UI.Shared.Languages;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Windows
{
    public class WindowSharedViewModel : BaseViewModel
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

        public WindowSharedViewModel()
        {
            TabControlItemsBingo = new ObservableCollection<MetroTabItem>();
            TabControlItemsAnswer = new ObservableCollection<MetroTabItem>();

            Games = new List<Game>();

            AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_MENU, new MainMenu(), false);

            MessengerInstance.Register<LaunchActivityMessage>(this, LaunchActivity);
            MessengerInstance.Register<StartNewGameMessage>(this, AddGame);
            MessengerInstance.Register<LaunchFinishedDistributionMessage>(this, ShowDistributionCompleteTab);
        }

        private void ShowDistributionCompleteTab(LaunchFinishedDistributionMessage message)
        {
            var viewModel = new DistributorResultViewModel(message.Cartelas, message.MaxSemelhanca);
            var page = new DistributorResult(viewModel);

            AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_DISTRIBUTOR_RESULT, page, true);
        }

        private void LaunchNewGame()
        {
            if(TabControlItemsBingo.Count > 1 && (TabControlItemsBingo[1].Content as Frame).Content is NewGame)
            {
                TabControlSelectedIndex = 1;
            }
            else
            {
                AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_NEW_GAME, new NewGame(), true, 1);
            }            
        }

        private void LaunchActivity(LaunchActivityMessage activityMessage)
        {
            switch (activityMessage.Activity)
            {
                case Activity.ActivityNewGame:
                    LaunchNewGame();
                    break;
                case Activity.ActivityCreateGame:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_CREATE_GAME, new CreateGame(new CreateGameViewModel()), true);
                    break;
                case Activity.ActivityDistributor:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_DISTRIBUTOR, new Distributor(), true);
                    break;
                case Activity.ActivityHelp:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_HELP, new Help(), true);
                    break;
                case Activity.ActivityAbout:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_ABOUT, new About(), true);
                    break;
            }
        }

        private void AddBingoTabControlItem(object header, object content, bool canClose, int index = 0)
        {
            int indexExistent = GetIndexWhereHeaderIs(header);

            if (indexExistent > 0)
            {
                TabControlSelectedIndex = indexExistent;
                return;
            }

            var itemBingo = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = canClose,
                Content = new Frame()
                {
                    Content = content
                }
            };

            var itemAnswers = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = canClose,
                Content = new Frame()
                {
                    Content = new TextBlock()
                    {
                        Text = LanguageLocator.Instance.CurrentLanguage.OTHER_USE_MAIN_WINDOW,
                        IsEnabled = false,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        FontSize = 16
                    },
                }
            };

            if (canClose)
            {
                var command = new SimpleDelegateCommand(() =>
                {
                    TabControlItemsBingo.Remove(itemBingo);
                    TabControlItemsAnswer.Remove(itemAnswers);
                });

                itemBingo.CloseTabCommand = command;
                itemAnswers.CloseTabCommand = command;
            }

            TabControlItemsBingo.Insert(index, itemBingo);
            TabControlItemsAnswer.Insert(index, itemAnswers);
            TabControlSelectedIndex = index;
        }

        private void AddBingoTabControlItem(object header, object content, bool canClose)
        {
            int indexExistent= GetIndexWhereHeaderIs(header);

            if(indexExistent > 0)
            {
                TabControlSelectedIndex = indexExistent;
                return;
            }

            var itemBingo = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = canClose,
                Content = new Frame()
                {
                    Content = content
                }
            };

            var itemAnswers = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = canClose,
                Content = new Frame()
                {
                    Content = new TextBlock()
                    {
                        Text = LanguageLocator.Instance.CurrentLanguage.OTHER_USE_MAIN_WINDOW,
                        IsEnabled = false,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                        FontSize = 16
                    },
                }
            };

            if (canClose)
            {
                var command = new SimpleDelegateCommand(() =>
                {
                    TabControlItemsBingo.Remove(itemBingo);
                    TabControlItemsAnswer.Remove(itemAnswers);
                });

                itemBingo.CloseTabCommand = command;
                itemAnswers.CloseTabCommand = command;
            }

            TabControlItemsBingo.Add(itemBingo);
            TabControlItemsAnswer.Add(itemAnswers);
            TabControlSelectedIndex = TabControlItemsBingo.Count - 1;
        }

        private void AddGame(StartNewGameMessage startNewGameMessage)
        {
            var header = string.Format("{0} #{1}", LanguageLocator.Instance.CurrentLanguage.HEADER_GAME, ++LaunchedGames);

            var itemBingo = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = true,
                Content = new Frame()
                {
                    Content = startNewGameMessage.GamePage
                }
            };

            var itemAnswers = new MetroTabItem()
            {
                Header = header,
                CloseButtonEnabled = true,
                Content = new Frame()
                {
                    Content = startNewGameMessage.GameAnswersPage
                }
            };

            var command = new SimpleDelegateCommand(() =>
            {
                TabControlItemsBingo.Remove(itemBingo);
                TabControlItemsAnswer.Remove(itemAnswers);
            });

            itemBingo.CloseTabCommand = command;
            itemAnswers.CloseTabCommand = command;

            TabControlItemsBingo.Add(itemBingo);
            TabControlItemsAnswer.Add(itemAnswers);
            TabControlSelectedIndex = TabControlItemsBingo.Count - 1;
        }

        private int GetIndexWhereHeaderIs(object header)
        {
            for(int i = 0; i < TabControlItemsBingo.Count; i++)
            {
                if(Equals(TabControlItemsBingo[i].Header, header))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
