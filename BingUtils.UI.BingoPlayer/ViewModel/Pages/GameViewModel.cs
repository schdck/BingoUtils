﻿using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.Helpers.BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Messages;
using BingoUtils.UI.BingoPlayer.Resources;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class GameViewModel : ViewModelBase
    {
        private List<Question> _Questions;
        private string _CurrentQuestionTitle;
        private string _PreviousQuestionTitle;
        private string _QuestionProgress;
        private bool _HasPrevious;
        private bool _HasNext;
        private bool _HasLoadedGameData;

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
        public int QuestionsCount
        {
            get
            {
                return Questions.Count;
            }
        }

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

        public TransitionType AnimationToBeUsed { get; private set; }

        public ICommand PreviousQuestionCommand { get; private set; }
        public ICommand NextQuestionCommand { get; private set; }
        public ICommand PlayQuestionTitleCommand { get; private set; }
        public ICommand StopQuestionTitleCommand { get; private set; }

        public Visibility PlayQuestionTitleButtonVisibility { get; private set; }
        public Visibility StopQuestionTitleCommandVisibility { get; private set; }

        public int CurrentQuestion { get; private set; }

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

        private void GetGameData(StartNewGameMessage startNewGameMessage)
        {
            if(!_HasLoadedGameData)
            {
                _HasLoadedGameData = true;
                Questions = startNewGameMessage.QuestionList.OrderBy(x => new Random().Next()).ToList();
                StartNewGame();
            }
        }

        void StartNewGame()
        {
            AudioPlayer.AddSpeakCompletedHandler(() => StopQuestionTitleCommand.Execute(null));

            CurrentQuestion = -1;
            NextQuestion();
        }

        void PreviousQuestion()
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

        void NextQuestion()
        {
            StopQuestionTitleCommand.Execute(null);

            PreviousQuestionTitle = CurrentQuestionTitle;
            CurrentQuestionTitle = Questions[++CurrentQuestion].Title;

            HasNext = CurrentQuestion + 1 < Questions.Count;
            HasPrevious = CurrentQuestion > 0;

            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Count;
        }
    }
}
