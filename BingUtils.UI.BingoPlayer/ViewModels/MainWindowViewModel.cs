using BingoUtils.Domain.Entities;
using BingoUtils.Helpers;
using BingUtils.UI.BingoPlayer.Resources;
using MahApps.Metro.Controls;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace BingUtils.UI.BingoPlayer.ViewModels
{
    public class MainWindowViewModel : DefaultViewModel
    {
        public List<Question> QuestionsList { get; private set; }

        public Visibility GameComponentsVisibility { get; private set; }
        public Visibility StartGameComponentsVisibility { get; private set; }

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

        public SimpleDelegateCommand StartGameCommand { get; private set; }
        public SimpleDelegateCommand PreviousQuestionCommand { get; private set; }
        public SimpleDelegateCommand NextQuestionCommand { get; private set; }
        
        public int CurrentQuestion { get; private set; }

        private int _DisciplinaSelectedIndex;
        public int DisciplinaSelectedIndex 
        { 
            get
            {
                return _DisciplinaSelectedIndex;
            }
            set
            {
                if(value != DisciplinaSelectedIndex)
                {
                    _DisciplinaSelectedIndex = value;

                    if (value < QuestionsResources.AvaliableConteudosPerDisciplina.Count)
                    {
                        AvaliableConteudos = QuestionsResources.AvaliableConteudosPerDisciplina[DisciplinaSelectedIndex];
                    }
                    else
                    {
                        AvaliableConteudos = new string[] { };
                    }
                }
            }
        }
        public int ConteudoSelectedIndex { get; set; }

        public string[] AvaliableDisciplinas { get; private set; }
        public string[] AvaliableConteudos { get; private set; }

        public MainWindowViewModel()
        {
            GameComponentsVisibility = Visibility.Collapsed;

            StartGameCommand = new SimpleDelegateCommand((x) => StartNewGame());
            PreviousQuestionCommand = new SimpleDelegateCommand((x) => PreviousQuestion());
            NextQuestionCommand = new SimpleDelegateCommand((x) => NextQuestion());

            AvaliableDisciplinas = QuestionsResources.AvaliableDisciplinas;
            AvaliableConteudos = QuestionsResources.AvaliableConteudosPerDisciplina[0];
        }

        void StartNewGame()
        {
            GameComponentsVisibility = Visibility.Visible;
            StartGameComponentsVisibility = Visibility.Collapsed;

            Questions = Questions.OrderBy(x => new Random().Next()).ToArray();

            new Random().Shuffle(Questions);

            CurrentQuestion = -1;
            NextQuestion();
        }

        void PreviousQuestion()
        {
            CurrentQuestionTitle = Questions[--CurrentQuestion];

            if(CurrentQuestion - 1 >= 0)
            {
                PreviousQuestionTitle = Questions[CurrentQuestion - 1];
                HasPrevious = true;
            }
            else
            {
                PreviousQuestionTitle = String.Empty;
                HasPrevious = false;
            }
            HasNext = CurrentQuestion + 1 < Questions.Length;
            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Length;
        }

        void NextQuestion()
        {
            PreviousQuestionTitle = CurrentQuestionTitle;
            CurrentQuestionTitle = Questions[++CurrentQuestion];

            HasNext = CurrentQuestion + 1 < Questions.Length;
            HasPrevious = CurrentQuestion > 0;

            QuestionProgress = (CurrentQuestion + 1) + "/" + Questions.Length;
        }
    }
}
