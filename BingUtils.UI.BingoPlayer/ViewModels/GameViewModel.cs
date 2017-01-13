using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingoUtils.Helpers.BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Resources;
using MahApps.Metro.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace BingoUtils.UI.BingoPlayer.ViewModels
{
    public class GameViewModel : DefaultViewModel
    {
        public List<Question> QuestionsList { get; private set; }

        public string CurrentQuestionTitle { get; private set; }
        public string PreviousQuestionTitle { get; private set; }
        public string QuestionProgress { get; private set; }

        [DoNotNotify]
        public int QuestionsCount
        {
            get
            {
                return QuestionsResources.Questions.Length;
            }
        }

        [DoNotNotify]
        public string[] Questions
        {
            get
            {
                return QuestionsResources.Questions;
            }
            private set
            {
                QuestionsResources.Questions = value;
            }
        }

        public bool HasPrevious { get; private set; }
        public bool HasNext { get; private set; }

        public TransitionType AnimationToBeUsed { get; private set; }

        public SimpleDelegateCommand PreviousQuestionCommand { get; private set; }
        public SimpleDelegateCommand NextQuestionCommand { get; private set; }
        public SimpleDelegateCommand PlayQuestionTitleCommand { get; private set; }
        public SimpleDelegateCommand StopQuestionTitleCommand { get; private set; }

        public Visibility PlayQuestionTitleButtonVisibility { get; private set; }
        public Visibility StopQuestionTitleCommandVisibility { get; private set; }

        public int CurrentQuestion { get; private set; }

        public GameViewModel()
        {
            PlayQuestionTitleButtonVisibility = Visibility.Visible;
            StopQuestionTitleCommandVisibility = Visibility.Hidden;

            PreviousQuestionCommand = new SimpleDelegateCommand((x) => PreviousQuestion());
            NextQuestionCommand = new SimpleDelegateCommand((x) => NextQuestion());

            PlayQuestionTitleCommand = new SimpleDelegateCommand((x) =>
            {
                PlayQuestionTitleButtonVisibility = Visibility.Hidden;
                StopQuestionTitleCommandVisibility = Visibility.Visible;

                AudioPlayer.PlaySpeech(CurrentQuestionTitle);
            });

            StopQuestionTitleCommand = new SimpleDelegateCommand((x) =>
            {
                PlayQuestionTitleButtonVisibility = Visibility.Visible;
                StopQuestionTitleCommandVisibility = Visibility.Hidden;

                AudioPlayer.StopSpeach();
            });

            StartNewGame();
        }

        void StartNewGame()
        {
            Questions = Questions.OrderBy(x => new Random().Next()).ToArray();

            new Random().Shuffle(Questions);

            AudioPlayer.AddSpeakCompletedHandler(() => StopQuestionTitleCommand.Execute(null));

            CurrentQuestion = -1;
            NextQuestion();
        }

        void PreviousQuestion()
        {
            StopQuestionTitleCommand.Execute(null);

            CurrentQuestionTitle = Questions[--CurrentQuestion];

            if (CurrentQuestion - 1 >= 0)
            {
                PreviousQuestionTitle = Questions[CurrentQuestion - 1];
                HasPrevious = true;
            }
            else
            {
                PreviousQuestionTitle = string.Empty;
                HasPrevious = false;
            }
            HasNext = CurrentQuestion + 1 < Questions.Length;
            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Length;
        }

        void NextQuestion()
        {
            StopQuestionTitleCommand.Execute(null);

            PreviousQuestionTitle = CurrentQuestionTitle;
            CurrentQuestionTitle = Questions[++CurrentQuestion];

            HasNext = CurrentQuestion + 1 < Questions.Length;
            HasPrevious = CurrentQuestion > 0;

            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Length;
        }
    }
}
