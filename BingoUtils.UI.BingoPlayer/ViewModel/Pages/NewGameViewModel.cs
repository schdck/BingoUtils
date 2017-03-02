﻿using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.Resources;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Input;

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

        private double _DefaultContainerBackground;
        private double _FileContainerBackground;

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
                    path = Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject), string.Format("{0}.csv", AvaliableMatters.ElementAt(SelectedIndexMatter)));
                }


                if (!File.Exists(path))
                {
                    throw (new Exception("Erro ao abrir arquivo"));
                }

                using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding("WINDOWS-1252")))
                {
                    try
                    {
                        string line = reader.ReadLine(); // Pular a linha de cabeçalho

                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] values = line.Split(';');

                            if (string.IsNullOrWhiteSpace(values[0]) || string.IsNullOrWhiteSpace(values[1]))
                            {
                                break;
                            }

                            questionList.Add(new Question(values[0], values[1]));
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
                        string file = temp[1] + ".csv";

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

        private IEnumerable<string> GetAvaliableMatters()
        {
            IEnumerable<string> files;

            try
            {
                files = Directory.GetFiles(
                            Path.Combine(GamesDirectory, AvaliableSubjects.ElementAt(SelectedIndexSubject)))
                                .Where((x) => (Path.GetExtension(x) == ".csv"));
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

                DefaultContainerBackground = 1.0;
                FileContainerBackground = 0.5;

                IsSelectingFrom = "Iniciar novo jogo a partir dos modelos";
            }
            else if (choice == "File")
            {
                HasSelectedValidOption = File.Exists(SelectedFilePath);

                IsSelectingFrom = "Iniciar novo jogo a partir do arquivo";
                DefaultContainerBackground = 0.5;
                FileContainerBackground = 1.0;
            }
            else
            {
                HasSelectedValidOption = false;
                DefaultContainerBackground = 1.0;
                FileContainerBackground = 1.0;
                IsSelectingFrom = "Selecione se deseja iniciar o novo jogo a partir de um modelo ou de um arquivo";
            }
        }
    }
}