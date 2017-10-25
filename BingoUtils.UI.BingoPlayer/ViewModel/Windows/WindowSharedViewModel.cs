using BingoUtils.Domain.Enums;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using BingoUtils.UI.BingoPlayer.Views.Windows;
using BingoUtils.UI.Shared.Languages;
using BingoUtils.UI.Shared.Views.UserControls;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Windows
{
    public class WindowSharedViewModel : BaseViewModel
    {
        public static int LaunchedGames;

        public ICommand LaunchChangeLanguageWindow { get; private set; }

        public ObservableCollection<MetroTabItem> TabControlItemsBingo { get; private set; }
        public ObservableCollection<MetroTabItem> TabControlItemsAnswer { get; private set; }
        public List<Game> Games { get; set; }
        public int TabControlSelectedIndex { get; set; }

        public WindowSharedViewModel()
        {
            TabControlItemsBingo = new ObservableCollection<MetroTabItem>();
            TabControlItemsAnswer = new ObservableCollection<MetroTabItem>();

            Games = new List<Game>();

            AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_MENU, new MainMenu(), false);

            MessengerInstance.Register<LaunchActivityMessage>(this, LaunchActivity);
            MessengerInstance.Register<StartNewGameMessage>(this, AddGame);

            LaunchChangeLanguageWindow = new RelayCommand(() =>
            {
                var content = new StackPanel();

                var child = new ChildWindow()
                {
                    Content = content,
                    ShowTitleBar = false
                };

                var closeButton = new Button() { Margin = new Thickness(0, 0, 0, 5), Width = 300 };

                var titleBar = new TextBlock() { Height = child.TitleBarHeight, TextAlignment = TextAlignment.Center, FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(5) };

                content.Children.Add(titleBar);
                content.Children.Add(new LanguageSelector());
                content.Children.Add(closeButton);

                closeButton.Click += (s, e) => child.Close();

                BindingOperations.SetBinding(closeButton, ContentControl.ContentProperty, new Binding("CurrentLanguage.GENERIC_CLOSE") { Source = LanguageLocator.Instance, Converter = Application.Current.FindResource("ToUpperConverter") as IValueConverter });
                BindingOperations.SetBinding(titleBar, TextBlock.TextProperty, new Binding("CurrentLanguage.OTHER_SELECT_LANGUAGE") { Source = LanguageLocator.Instance });

                MainWindow.Instance.ShowChildWindowAsync(child);
            });
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
                case Activity.ActivityCardGenerator:
                    var dataContext = new CardGeneratorViewModel();

                    var window = new CardGeneratorWindow()
                    {
                        DataContext = dataContext
                    };

                    dataContext.GeneratedCardsCommand = new RelayCommand(() =>
                    {
                        MessageBox.Show("Cartelas geradas com sucesso");
                    });

                    MainWindow.Instance.ShowChildWindowAsync(window);
                    break;
                case Activity.ActivityHelp:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_HELP, new Help(), true);
                    break;
                case Activity.ActivityAbout:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_ABOUT, new About(), true);
                    break;
                case Activity.ActivitySettings:
                    AddBingoTabControlItem(LanguageLocator.Instance.CurrentLanguage.HEADER_SETTINGS, new Settings(), true);
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
                var command = new RelayCommand(() =>
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
                var command = new RelayCommand(() =>
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

            var command = new RelayCommand(() =>
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
