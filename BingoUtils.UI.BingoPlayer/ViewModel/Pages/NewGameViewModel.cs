using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.Resources;
using BingoUtils.UI.BingoPlayer.ViewModel.Windows;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using BingoUtils.UI.Shared.Languages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class NewGameViewModel : ViewModelBase
    {
        private int _SelectedIndexSubject;
        private int _SelectedIndexTopic;

        private IEnumerable<string> _AvaliableSubjects;

        public ICommand StartNewgameCommand { get; private set; }
        public ICommand SetActiveChoice { get; private set; }
        public ICommand RefreshAvaliableBingos { get; private set; }

        public bool HasSelectedValidOption { get; set; }

        public int SelectedIndexSubject
        {
            get
            {
                return _SelectedIndexSubject;
            }
            set
            {
                Set(ref _SelectedIndexSubject, value);
                AvaliableTopics = GameHelper.GetAvaliableTopicsForSubject(AvaliableSubjects.ElementAt(SelectedIndexSubject));
                ChangeActiveChoice("Default");
            }
        }
        public int SelectedIndexTopic
        {
            get
            {
                return _SelectedIndexTopic;
            }
            set
            {
                Set(ref _SelectedIndexTopic, value);
                ChangeActiveChoice("Default");
            }
        }

        public string IsSelectingFrom { get; set; }

        public string SelectedFilePath { get; set; }

        public double DefaultContainerBackground { get; set; }
        public double FileContainerBackground { get; set; }

        public IEnumerable<string> AvaliableSubjects
        {
            get
            {
                return _AvaliableSubjects;
            }
            set
            {
                Set(ref _AvaliableSubjects, value);
                AvaliableTopics = GameHelper.GetAvaliableTopicsForSubject(AvaliableSubjects.ElementAt(SelectedIndexSubject));
            }
        }
        public IEnumerable<string> AvaliableTopics { get; set; }

        public NewGameViewModel()
        {
            DefaultContainerBackground = 1;
            FileContainerBackground = 1;

            StartNewgameCommand = new RelayCommand(() =>
            {
                var viewModel = SimpleIoc.Default.GetInstance<GameViewModel>(Guid.NewGuid().ToString());
                var newGame = new Game(viewModel);
                var newGameAnswers = new GameAnswers(viewModel);
                var questionList = new List<Question>();
                
                if(FileContainerBackground == 1) // Load the file that the user chose
                {
                    questionList = GameHelper.LoadGame(SelectedFilePath, WindowSharedViewModel.LaunchedGames.ToString());
                }
                else // Load the game selected in the comboboxes
                {
                    questionList = GameHelper.LoadGame(AvaliableSubjects.ElementAt(SelectedIndexSubject), AvaliableTopics.ElementAt(SelectedIndexTopic), WindowSharedViewModel.LaunchedGames.ToString());
                }
               
                MessengerInstance.Send(new StartNewGameMessage(viewModel, newGame, newGameAnswers, questionList));
            });
            RefreshAvaliableBingos = new RelayCommand(() =>
            {
                if (!Directory.Exists(GameHelper.GamesDirectory))
                {
                    Directory.CreateDirectory(GameHelper.GamesDirectory);

                    CreateDefaultGamesFiles();
                }

                AvaliableSubjects = GameHelper.GetAvaliableSubjects();
            });
            SetActiveChoice = new RelayCommand<string>((x) => ChangeActiveChoice(x));

            RefreshAvaliableBingos.Execute(null);
        }
        
        private void CreateDefaultGamesFiles()
        {
            foreach (string s in ResourceMapper.ResourceFiles)
            {
                var resourceName = @"BingoUtils.UI.BingoPlayer.Resources." + s;
                var assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
                    {
                        string result = reader.ReadToEnd();
                        string[] temp = s.Split('.');

                        string path = Path.Combine(GameHelper.GamesDirectory, temp[0]);
                        string file = temp[1] + ".zip";

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (StreamWriter writer = new StreamWriter(Path.Combine(path, file), false, Encoding.GetEncoding(1252)))
                        {
                            writer.Write(result);
                        }
                    }
                }
            }
        }

        private string GetResourceName(string resource)
        {
            return "";
        }

        private void ChangeActiveChoice(string choice)
        {
            if (choice == "Default")
            {
                IsSelectingFrom = LanguageLocator.Instance.CurrentLanguage.START_NEW_GAME_FROM_MODEL;

                HasSelectedValidOption = SelectedIndexSubject >= 0 && AvaliableSubjects.FirstOrDefault() != null && SelectedIndexTopic >= 0 && AvaliableTopics.FirstOrDefault() != null;

                DefaultContainerBackground = 1.0;
                FileContainerBackground = 0.5;
            }
            else if (choice == "File")
            {
                IsSelectingFrom = LanguageLocator.Instance.CurrentLanguage.START_NEW_GAME_FROM_FILE;

                HasSelectedValidOption = File.Exists(SelectedFilePath);

                DefaultContainerBackground = 0.5;
                FileContainerBackground = 1.0;
            }
            else
            {
                IsSelectingFrom = LanguageLocator.Instance.CurrentLanguage.START_NEW_GAME_NOT_SELECTED;

                HasSelectedValidOption = false;
                DefaultContainerBackground = 1.0;
                FileContainerBackground = 1.0;
            }
        }
    }
}