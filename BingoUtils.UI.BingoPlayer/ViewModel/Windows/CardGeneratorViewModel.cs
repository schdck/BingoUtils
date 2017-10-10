using BingoUtils.Domain.Entities;
using BingoUtils.Domain.Enums;
using BingoUtils.Helpers;
using BingoUtils.UI.BingoPlayer.Views.Windows;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class CardGeneratorViewModel : BaseViewModel
    {
        private const double MAX_SIMILARITY = 75;

        private int _SelectedIndexSubject;
        private int _SelectedIndexTopic;
        private double? _AmountOfCards;
        private double? _AmountOfQuestionsPerCard;
        private string _ErrorText;
        private IEnumerable<string> _AvaliableSubjects;
        private IEnumerable<string> _AvaliableTopics;
        private DistributorState _CurrentDistributorStatus;
        private List<Question> _GameQuestions;
        private BackgroundWorker _WorkerToDistribute;
        
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
            }
        }
        public double? AmountOfCards
        {
            get
            {
                return _AmountOfCards;
            }
            set
            {
                Set(ref _AmountOfCards, value);
            }
        }
        public double? AmountOfQuestionsPerCard
        {
            get
            {
                return _AmountOfQuestionsPerCard;
            }
            set
            {
                Set(ref _AmountOfQuestionsPerCard, value);
            }
        }
        public string ErrorText
        {
            get
            {
                return _ErrorText;
            }
            set
            {
                Set(ref _ErrorText, value);
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
                AvaliableTopics = GameHelper.GetAvaliableTopicsForSubject(AvaliableSubjects.ElementAt(SelectedIndexSubject));
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
        
        public DistributorState CurrentDistributorStatus
        {
            get
            {
                return _CurrentDistributorStatus;
            }
            set
            {
                Set(ref _CurrentDistributorStatus, value);
            }
        }

        public ICommand DistributeQuestionsCommand { get; private set; }
        public ICommand GenerateCardsCommand { get; private set; }
        public ICommand GeneratedCardsCommand { get; set; }

        public CardGeneratorViewModel()
        {
            _WorkerToDistribute = new BackgroundWorker();

            InitializeCommands();
            AvaliableSubjects = GameHelper.GetAvaliableSubjects();
        }

        private void InitializeCommands()
        {
            DistributeQuestionsCommand = new RelayCommand(() =>
            {
                _GameQuestions = GameHelper.LoadGame(AvaliableSubjects.ElementAt(SelectedIndexSubject), AvaliableTopics.ElementAt(SelectedIndexTopic), "Cartela");

                switch (ValidateInputs())
                {
                    case 1:
                        CurrentDistributorStatus = DistributorState.Waiting;
                        ErrorText = "A quantidade de questões não pode ser zero.";
                        return;
                    case 2:
                        CurrentDistributorStatus = DistributorState.Waiting;
                        ErrorText = "A quantidade de cartelas não pode ser zero.";
                        return;
                    case 3:
                        CurrentDistributorStatus = DistributorState.Waiting;
                        ErrorText = "A quantidade de questões por cartela não pode ser zero.";
                        return;
                    case 4:
                        CurrentDistributorStatus = DistributorState.Waiting;
                        ErrorText = "A quantidade de questões por cartela não pode ser maior que a quantidade de questões.";
                        return;
                }
                
                DistributeQuestions();
            });
        }

        private void DistributeQuestions()
        {
            bool succeeded = false;
            Cartela[] cartelas = null;

            _WorkerToDistribute.DoWork += (s, e) =>
            {
                succeeded = DistributeQuestions(out cartelas);
            };

            _WorkerToDistribute.RunWorkerCompleted += (s, e) =>
            {
                CurrentDistributorStatus = succeeded ? DistributorState.Success : DistributorState.Error;

                SaveFileDialog dialog = new SaveFileDialog()
                {
                    AddExtension = true,
                    DefaultExt = ".pdf",
                    Filter = "PDF|.pdf"
                };

                if (dialog.ShowDialog() == true)
                {
                    var window = new PdfGeneratorWindow(_GameQuestions, cartelas, dialog.FileName);

                    window.Closed += (sender, eargs) =>
                    {
                        GeneratedCardsCommand?.Execute(null);
                    };

                    window.Show();
                }
                
            };

            CurrentDistributorStatus = DistributorState.Working;

            _WorkerToDistribute.RunWorkerAsync();
        }

        /* This method needs improvements
         * TODO
         * - Implement a logic to distribute the questions using all of them and using them in a similar amount of times
        */
        private bool DistributeQuestions(out Cartela[] cartelas)
        {
            Random r = new Random();
            cartelas = new Cartela[(int) AmountOfCards];
            int i = 0;

            do
            {
                cartelas[i] = new Cartela(i, (int) AmountOfQuestionsPerCard);

                while(!cartelas[i].IsFull)
                {
                    int val = r.Next(Convert.ToInt32(_GameQuestions.Count)) + 1;
                    cartelas[i].AddQuestion(val);
                }

                double maxSemelhanca = 0;

                for(int j = i - 1; j >= 0; j--)
                {
                    double temp = cartelas[i].GetSemelhanca(cartelas[j]);

                    if(temp > maxSemelhanca)
                    {
                        maxSemelhanca = temp;
                    }
                }

                if(maxSemelhanca <= MAX_SIMILARITY)
                {
                    i++;
                }

            } while(i < AmountOfCards);

            return true;
        }

        private int ValidateInputs()
        {
            if(_GameQuestions.Count <= 0)
            {
                return 1;
            }
            else if (AmountOfCards == null || AmountOfCards <= 0)
            {
                return 2;
            }
            else if(AmountOfQuestionsPerCard == null || AmountOfCards <= 0)
            {
                return 3;
            }
            else if (_GameQuestions.Count < AmountOfQuestionsPerCard)
            {
                return 4;
            }
            return 0;
        }
    }
}