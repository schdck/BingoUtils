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
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class NewGameViewModel : ViewModelBase
    {
        private readonly string GamesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bingo", "Jogos");
        private readonly string ExtractedFilesDirectory = Path.Combine(Path.GetTempPath(), "BingoTemp");

        private bool _HasSelectedOption;

        private int _SelectedIndexSubject;
        private int _SelectedIndexTopic;

        private string _IsSelectingFrom;
        private string _FilePath;

        private double _DefaultContainerBackground;
        private double _FileContainerBackground;

        private IEnumerable<string> _AvaliableSubjects;
        private IEnumerable<string> _AvaliableTopics;

        public ICommand StartNewgameCommand { get; private set; }
        public ICommand SetActiveChoice { get; private set; }
        public ICommand RefreshAvaliableBingos { get; private set; }

        public bool HasSelectedValidOption
        {
            get
            {
                return _HasSelectedOption;
            }
            set
            {
                Set(ref _HasSelectedOption, value);
            }
        }

        public int SelectedIndexSubject
        {
            get
            {
                return _SelectedIndexSubject;
            }
            set
            {
                Set(ref _SelectedIndexSubject, value);
                AvaliableTopics = GetAvaliableTopics();
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

        public string IsSelectingFrom
        {
            get
            {
                return _IsSelectingFrom;
            }
            set
            {
                Set(ref _IsSelectingFrom, value);
            }
        }

        public string SelectedFilePath
        {
            get
            {
                return _FilePath;
            }
            set
            {
                Set(ref _FilePath, value);
                ChangeActiveChoice("File");
            }
        }

        public double DefaultContainerBackground
        {
            get
            {
                return _DefaultContainerBackground;
            }
            set
            {
                Set(ref _DefaultContainerBackground, value);
            }
        }
        public double FileContainerBackground
        {
            get
            {
                return _FileContainerBackground;
            }
            set
            {
                Set(ref _FileContainerBackground, value);
            }
        }

        public IEnumerable<string> AvaliableSubjects
        {
            get
            {
                return _AvaliableSubjects;
            }
            set
            {
                Set(ref _AvaliableSubjects, value);
                AvaliableTopics = GetAvaliableTopics();
            }
        }
        public IEnumerable<string> AvaliableTopics
        {
            get
            {
                return _AvaliableTopics;
            }
            set
            {
                Set(ref _AvaliableTopics, value);
            }
        }

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

                string path;

                if(_FileContainerBackground == 1) // Carregar jogo do arquivo do usuário
                {
                    path = SelectedFilePath;

                    CopyFileIfNew(SelectedFilePath);
                }
                else // Carregar jogo da ComboBox
                {
                    path = Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject), string.Format("{0}.zip", AvaliableTopics.ElementAt(SelectedIndexTopic)));
                }


                if (!File.Exists(path))
                {
                    throw (new Exception("Erro ao abrir arquivo"));
                }

                string currentGameExtractedFilesDirectory = Path.Combine(ExtractedFilesDirectory, WindowSharedViewModel.LaunchedGames.ToString());

                if (Directory.Exists(currentGameExtractedFilesDirectory))
                {
                    Directory.Delete(currentGameExtractedFilesDirectory, true);
                }

                ZipFile.ExtractToDirectory(path, currentGameExtractedFilesDirectory);
                

                using (StreamReader reader = new StreamReader(Path.Combine(currentGameExtractedFilesDirectory, "Game.csv"), Encoding.GetEncoding("WINDOWS-1252")))
                {
                    try
                    {
                        string line = reader.ReadLine(); // Pular a linha de cabeçalho

                        while ((line = reader.ReadLine()) != null && !string.IsNullOrEmpty(line))
                        {
                            string[] values = line.Split(';');

                            string questionTitle = values[0],
                                   questionAnswer = values[1],
                                   titleImagePath = null,
                                   answerImagePath = null;

                            if (!string.IsNullOrEmpty(values[2]))
                            {
                                titleImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[2]);
                            }

                            if (!string.IsNullOrEmpty(values[4]))
                            {
                                answerImagePath = Path.Combine(currentGameExtractedFilesDirectory, "img", values[4]);
                            }

                            questionList.Add(new Question(questionTitle, questionAnswer, titleImagePath, values[3] == "true", answerImagePath));
                        }
                    }
                    catch
                    {
                        throw (new Exception("Erro ao abrir arquivo"));
                    }
                }

                MessengerInstance.Send(new StartNewGameMessage(viewModel, newGame, newGameAnswers, questionList));
            });
            RefreshAvaliableBingos = new RelayCommand(() =>
            {
                AvaliableSubjects = GetAvaliableSubjects();
            });
            SetActiveChoice = new RelayCommand<string>((x) => ChangeActiveChoice(x));

            AvaliableSubjects = GetAvaliableSubjects();
        }

        private IEnumerable<string> GetAvaliableSubjects()
        {
            if(!Directory.Exists(GamesDirectory))
            {
                Directory.CreateDirectory(GamesDirectory);

                CreateDefaultGamesFiles();
            }

            var folders = new List<string>();

            foreach(string dir in Directory.GetDirectories(GamesDirectory))
            {
                folders.Add(new DirectoryInfo(dir).Name);
            }

            return folders;
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

                        string path = Path.Combine(GamesDirectory, temp[0]);
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

        private IEnumerable<string> GetAvaliableTopics()
        {
            IEnumerable<string> files; 

            try
            {
                files = Directory.GetFiles(
                            Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject)))
                                .Where((x) => (Path.GetExtension(x) == ".zip"));
            }
            catch
            {
                return null;
            }
            

            List<string> fileNamesWithoutExtension = new List<string>();

            foreach(string s in files)
            {
                fileNamesWithoutExtension.Add(Path.GetFileNameWithoutExtension(s));
            }

            return fileNamesWithoutExtension;
        }

        private void CopyFileIfNew(string selectedFilePath)
        {
            string[] gameInfo;

            using (StreamReader reader = new StreamReader(selectedFilePath))
            {
                gameInfo = reader.ReadLine().Split(';'); // Linha de cabeçalho
            }

            string newFileFolder = Path.Combine(GamesDirectory, gameInfo[0]);
            string newFilePath = Path.Combine(newFileFolder, string.Format("{0}.zip", gameInfo[1]));

            if(!Directory.Exists(newFileFolder))
            {
                Directory.CreateDirectory(newFileFolder);
            }

            if (File.Exists(newFilePath))
            {
                if (FileHelper.FileCompare(selectedFilePath, newFilePath))
                {
                    return;
                }
                newFilePath += "(2)";
            }
            File.Copy(selectedFilePath, newFilePath);
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