using BingoUtils.Domain.Entities;
using BingoUtils.Helpers.BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Messages;
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
        private bool _HasPrevious;
        private bool _HasNext;

        private int _CurrentQuestion;

        private string _CurrentQuestionTitle;
        private string _PreviousQuestionTitle;
        private string _QuestionProgress;

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

            CurrentQuestionTitle = Questions[--CurrentQuestion].Title;

            if (CurrentQuestion - 1 >= 0)
            {
                PreviousQuestionTitle = Questions[CurrentQuestion - 1].Title;
                HasPrevious = true;
            }
            else
            {
                PreviousQuestionTitle = string.Empty;
                HasPrevious = false;
            }
            HasNext = CurrentQuestion + 1 < Questions.Count;
            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Count;
        }

        private void NextQuestion()
        {
            StopQuestionTitleCommand.Execute(null);

            PreviousQuestionTitle = CurrentQuestionTitle;
            CurrentQuestionTitle = Questions[++CurrentQuestion].Title;

            HasNext = CurrentQuestion + 1 < Questions.Count;
            HasPrevious = CurrentQuestion > 0;

            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Count;
        }

        private void GetGameData(StartNewGameMessage startNewGameMessage)
        {
            if (startNewGameMessage.Target == this)
            {
                Questions = startNewGameMessage.QuestionList.OrderBy(x => new Random().Next()).ToList();
                StartNewGame();
            }
        }
    }
}
