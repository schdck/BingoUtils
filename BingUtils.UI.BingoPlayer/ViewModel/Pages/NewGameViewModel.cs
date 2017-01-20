using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.Helpers.Event_Args;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.Resources;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class NewGameViewModel : ViewModelBase
    {
        private readonly string GamesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Bingo", "Jogos");

        private bool _HasSelectedOption;

        private int _SelectedIndexSubject;
        private int _SelectedIndexMatter;

        private string _IsSelectingFrom;
        private string _FilePath;

        private Brush _DefaultContainerBackground;
        private Brush _FileContainerBackground;

        private IEnumerable<string> _AvaliableSubjects;
        private IEnumerable<string> _AvaliableMatters;

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
                AvaliableMatters = GetAvaliableMatters();
                ChangeActiveChoice("Default");
            }
        }
        public int SelectedIndexMatter
        {
            get
            {
                return _SelectedIndexMatter;
            }
            set
            {
                Set(ref _SelectedIndexMatter, value);
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

        public Brush DefaultContainerBackground
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
        public Brush FileContainerBackground
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
                AvaliableMatters = GetAvaliableMatters();
            }
        }
        public IEnumerable<string> AvaliableMatters
        {
            get
            {
                return _AvaliableMatters;
            }
            set
            {
                Set(ref _AvaliableMatters, value);
            }
        }

        public NewGameViewModel()
        {
            DefaultContainerBackground = Brushes.Transparent;
            FileContainerBackground = Brushes.Transparent;

            StartNewgameCommand = new RelayCommand(() =>
            {
                var newGame = new Game();
                var questionList = new List<Question>();
                string path;

                if(_FileContainerBackground == Brushes.LightGreen) // Carregar jogo do arquivo do usuário
                {
                    path = SelectedFilePath;

                    CopyFileIfNew(SelectedFilePath);
                }
                else // Carregar jogo da ComboBox
                {
                    path = Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject), string.Format("{0}.csv", AvaliableMatters.ElementAt(SelectedIndexMatter)));
                }


                if (!File.Exists(path))
                {
                    throw (new Exception("Erro ao abrir arquivo"));
                }

                using (StreamReader reader = new StreamReader(path))
                {
                    try
                    {
                        reader.ReadLine(); // Pular a linha de cabeçalho

                        while (!reader.EndOfStream)
                        {
                            string[] values = reader.ReadLine().Split(';');

                            questionList.Add(new Question(values[0], values[1]));
                        }
                    }
                    catch
                    {
                        throw (new Exception("Erro ao abrir arquivo 2"));
                    }
                }

                MessengerInstance.Send(new StartNewGameMessage(newGame.DataContext as GameViewModel, newGame, questionList));
            });
            RefreshAvaliableBingos = new RelayCommand(() =>
            {
                AvaliableSubjects = GetAvaliableSubjects();
            });

            AvaliableSubjects = GetAvaliableSubjects();
            SetActiveChoice = new RelayCommand<string>((x) => ChangeActiveChoice(x));
        }

        private IEnumerable<string> GetAvaliableSubjects()
        {
            if(!Directory.Exists(GamesDirectory))
            {
                Directory.CreateDirectory(GamesDirectory);
            }

            var folders = new List<string>();

            foreach(string dir in Directory.GetDirectories(GamesDirectory))
            {
                folders.Add(new DirectoryInfo(dir).Name);
            }

            return folders;
        }

        private IEnumerable<string> GetAvaliableMatters()
        {
            var files = Directory.GetFiles(
                            Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject)))
                                .Where((x) => (Path.GetExtension(x) == ".csv"));

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
            string newFilePath = Path.Combine(newFileFolder, string.Format("{0}.csv", gameInfo[1]));

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
                HasSelectedValidOption = SelectedIndexSubject >= 0 && AvaliableSubjects.FirstOrDefault() != null && SelectedIndexMatter >= 0 && AvaliableMatters.FirstOrDefault() != null;

                DefaultContainerBackground = Brushes.LightGreen;
                FileContainerBackground = Brushes.Transparent;

                IsSelectingFrom = "Você está iniciando um novo jogo a partir dos modelos já definidos";
            }
            else
            {
                HasSelectedValidOption = File.Exists(SelectedFilePath);

                IsSelectingFrom = "Você está iniciando um novo jogo a partir de um arquivo";
                DefaultContainerBackground = Brushes.Transparent;
                FileContainerBackground = Brushes.LightGreen;
            }
        }
    }
}