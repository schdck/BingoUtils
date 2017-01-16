using BingoUtils.Domain.Entities;
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
                ChangeActiveChoice("Default");
            }
        }
        public int SelectedndexMatter
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
        public string[] AvaliableSubjects
        {
            get
            {
                return GetAvaliableSubjects();
            }
            set
            {
                RaisePropertyChanged();
            }
        }
        public string[] AvaliableMatters
        {
            get
            {
                return GetAvaliableMatters();
            }
            set
            {
                RaisePropertyChanged();
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

        public NewGameViewModel()
        {
            StartNewgameCommand = new RelayCommand(() =>
            {
                var newGame = new Game();
                var questionList = new List<Question>();

                if(!File.Exists(SelectedFilePath))
                {
                    throw (new Exception("Erro ao abrir arquivo"));
                }

                using (StreamReader reader = new StreamReader(SelectedFilePath))
                {
                    try
                    {
                        while (!reader.EndOfStream)
                        {
                            string[] values = reader.ReadLine().Split(';');

                            questionList.Add(new Question(values[0], values[1]));
                        }
                    }
                    catch
                    {
                        throw (new Exception("Erro ao abrir arquivo"));
                    }
                }

                MessengerInstance.Send(new StartNewGameMessage(newGame.DataContext as GameViewModel, newGame, questionList));
            });
            RefreshAvaliableBingos = new RelayCommand(() =>
            {
                AvaliableSubjects = null;
                AvaliableMatters = null;
            });


            SetActiveChoice = new RelayCommand<string>((x) => ChangeActiveChoice(x));
        }

        private void ChangeActiveChoice(string choice)
        {
            if (choice == "Default")
            {
                //HasSelectedValidOption = !string.IsNullOrEmpty(SelectedIndexSubject) && !string.IsNullOrEmpty(SelectedndexMatter);

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

        private string[] GetAvaliableSubjects()
        {
            if(!Directory.Exists(GamesDirectory))
            {
                Directory.CreateDirectory(GamesDirectory);
            }

            var folders = new List<string>();

            foreach(string dir in Directory.GetDirectories(GamesDirectory))
            {
                folders.Add(Path.GetDirectoryName(dir));
            }

            return folders.ToArray();
        }

        private string[] GetAvaliableMatters()
        {
            return new string[] { "oi", "tchau" };
        }
    }
}