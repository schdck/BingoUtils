﻿using BingoUtils.Domain.Entities;
using BingoUtils.Helpers.BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.Views.Windows;
using BingoUtils.UI.Shared.Languages;
using BingoUtils.UI.Shared.UserControls.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class GameViewModel : ViewModelBase
    {
        private Random RandomGenerator = new Random();

        private bool _HasPrevious;
        private bool _HasNext;
        
        private int _CurrentQuestion;

        private string _CurrentQuestionTitle;
        private string _CurrentQuestionImagePath;
        private string _PreviousQuestionTitle;
        private string _PreviousQuestionImagePath;
        private string _QuestionProgress;

        private QuestionDisplayerViewModel _PreviousQuestionDataContext;
        private QuestionDisplayerViewModel _CurrentQuestionDataContext;

        private List<Question> _Questions;

        public bool HasPrevious
        {
            get
            {
                return _HasPrevious;
            }
            private set
            {
                Set(ref _HasPrevious, value);
            }
        }

        public bool HasNext
        {
            get
            {
                return _HasNext;
            }
            private set
            {
                Set(ref _HasNext, value);
            }
        }

        public int QuestionsCount
        {
            get
            {
                return Questions.Count;
            }
        }

        public int CurrentQuestion
        {
            get
            {
                return _CurrentQuestion;
            }
            set
            {
                Set(ref _CurrentQuestion, value);
            }
        }

        public string CurrentQuestionTitle
        {
            get
            {
                return _CurrentQuestionTitle;
            }
            private set
            {
                Set(ref _CurrentQuestionTitle, value);
            }
        }
        
        public string CurrentQuestionImagePath
        {
            get
            {
                return _CurrentQuestionImagePath;
            }
            private set
            {
                Set(ref _CurrentQuestionImagePath, value);
            }
        }

        public string PreviousQuestionTitle
        {
            get
            {
                return _PreviousQuestionTitle;
            }
            private set
            {
                Set(ref _PreviousQuestionTitle, value);
            }
        }

        public string PreviousQuestionImagePath
        {
            get
            {
                return _PreviousQuestionImagePath;
            }
            private set
            {
                Set(ref _PreviousQuestionImagePath, value);
            }

        }

        public QuestionDisplayerViewModel PreviousQuestionDataContext
        {
            get
            {
                return _PreviousQuestionDataContext;
            }
            set
            {
                Set(ref _PreviousQuestionDataContext, value);
            }
        }
        public QuestionDisplayerViewModel CurrentQuestionDataContext
        {
            get
            {
                return _CurrentQuestionDataContext;
            }
            set
            {
                Set(ref _CurrentQuestionDataContext, value);
            }
        }

        public string QuestionProgress
        {
            get
            {
                return _QuestionProgress;
            }
            private set
            {
                Set(ref _QuestionProgress, value);
            }
        }

        public List<Question> Questions
        {
            get
            {
                return _Questions;
            }
            private set
            {
                Set(ref _Questions, value);
            }
        }

        public TransitionType AnimationToBeUsed { get; private set; }

        public ICommand PreviousQuestionCommand { get; private set; }
        public ICommand NextQuestionCommand { get; private set; }
        public ICommand PlayQuestionTitleCommand { get; private set; }
        public ICommand StopQuestionTitleCommand { get; private set; }
        public ICommand ShowTitleImageCommand { get; private set; }
        public ICommand ShowAnswerImageCommand { get; private set; }
                        
        private Visibility _PlayQuestionTitleButtonViasibility;
        private Visibility _StopQuestionTitleCommandVisibility;

        public Visibility PlayQuestionTitleButtonVisibility
        {
            get
            {
                return _PlayQuestionTitleButtonViasibility;
            }
            private set
            {
                Set(ref _PlayQuestionTitleButtonViasibility, value);
            }
        }

        public Visibility StopQuestionTitleCommandVisibility
        {
            get
            {
                return _StopQuestionTitleCommandVisibility;
            }
            private set
            {
                Set(ref _StopQuestionTitleCommandVisibility, value);
            }
        }

        public GameViewModel()
        {
            MessengerInstance.Register<StartNewGameMessage>(this, GetGameData);

            PlayQuestionTitleButtonVisibility = Visibility.Visible;
            StopQuestionTitleCommandVisibility = Visibility.Hidden;

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            PreviousQuestionCommand = new RelayCommand(PreviousQuestion);

            NextQuestionCommand = new RelayCommand(NextQuestion);

            PlayQuestionTitleCommand = new RelayCommand(() =>
            {
                PlayQuestionTitleButtonVisibility = Visibility.Hidden;
                StopQuestionTitleCommandVisibility = Visibility.Visible;

                AudioPlayer.PlaySpeech(CurrentQuestionTitle);
            });

            StopQuestionTitleCommand = new RelayCommand(() =>
            {
                PlayQuestionTitleButtonVisibility = Visibility.Visible;
                StopQuestionTitleCommandVisibility = Visibility.Hidden;

                AudioPlayer.StopSpeach();
            });

            ShowTitleImageCommand = new RelayCommand<Question>((x) => 
            {
                DisplayImage(x.TitleImagePath);
            });

            ShowAnswerImageCommand = new RelayCommand<Question>((x) =>
            {
                DisplayImage(x.AnswerImagePath);
            });
        }

        private void DisplayImage(string path)
        {
            DisplayImageWindow imageDisplayer = new DisplayImageWindow(path);
            imageDisplayer.Show();
        }

        private void StartNewGame()
        {
            AudioPlayer.AddSpeakCompletedHandler(() => StopQuestionTitleCommand.Execute(null));

            CurrentQuestion = -1;
            NextQuestion();
        }

        private void PreviousQuestion()
        {
            StopQuestionTitleCommand.Execute(null);
            CurrentQuestion--;
            CurrentQuestionTitle = Questions[CurrentQuestion].Title;
            CurrentQuestionImagePath = Questions[CurrentQuestion].TitleImagePath;

            if (CurrentQuestion - 1 >= 0)
            {
                PreviousQuestionTitle = Questions[CurrentQuestion - 1].Title;

                if(Questions[CurrentQuestion - 1].IsTitleImageImportant)
                {
                    PreviousQuestionImagePath = Questions[CurrentQuestion - 1].TitleImagePath;
                }
                else
                {
                    PreviousQuestionImagePath = null;
                }

                HasPrevious = true;
            }
            else
            {
                PreviousQuestionTitle = string.Empty;
                PreviousQuestionImagePath = null;
                HasPrevious = false;
            }
            HasNext = CurrentQuestion + 1 < Questions.Count;

            PreviousQuestionDataContext = new QuestionDisplayerViewModel()
            {
                QuestionTitle = PreviousQuestionTitle,
                QuestionImagePath = PreviousQuestionImagePath
            };
            CurrentQuestionDataContext = new QuestionDisplayerViewModel()
            {
                QuestionTitle = CurrentQuestionTitle,
                QuestionImagePath = CurrentQuestionImagePath
            };

            UpdateQuestionProgress();
        }

        private void NextQuestion()
        {
            StopQuestionTitleCommand.Execute(null);

            PreviousQuestionTitle = CurrentQuestionTitle;
            CurrentQuestion++;
            CurrentQuestionTitle = Questions[CurrentQuestion].Title;

            if (CurrentQuestion > 0 && Questions[CurrentQuestion - 1].IsTitleImageImportant)
            {
                PreviousQuestionImagePath = Questions[CurrentQuestion - 1].TitleImagePath;
            }
            else
            {
                PreviousQuestionImagePath = null;
            }

            CurrentQuestionImagePath = Questions[CurrentQuestion].TitleImagePath;

            HasNext = CurrentQuestion + 1 < Questions.Count;
            HasPrevious = CurrentQuestion > 0;

            PreviousQuestionDataContext = new QuestionDisplayerViewModel()
            {
                QuestionTitle = PreviousQuestionTitle,
                QuestionImagePath = PreviousQuestionImagePath
            };
            CurrentQuestionDataContext = new QuestionDisplayerViewModel()
            {
                QuestionTitle = CurrentQuestionTitle,
                QuestionImagePath = CurrentQuestionImagePath
            };

            UpdateQuestionProgress();
        }

        private void UpdateQuestionProgress()
        {
            QuestionProgress = string.Format("{0} ({1}/{2})", LanguageLocator.Instance.CurrentLanguage.GAME_CURRENT_QUESTION.ToUpper(), CurrentQuestion + 1, Questions.Count);
        }

        private void GetGameData(StartNewGameMessage startNewGameMessage)
        {
            if (startNewGameMessage.Target == this)
            {
                Questions = startNewGameMessage.QuestionList.OrderBy(x => RandomGenerator.Next()).ToList();
                StartNewGame();
            }
        }
    }
}
