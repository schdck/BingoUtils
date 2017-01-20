using BingoUtils.Domain.Entities;
using BingoUtils.Domain.Enums;
using BingoUtils.UI.BingoPlayer.Messages;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class DistributorViewModel : BaseViewModel
    {
        private double _MaxSemelhanca = 75;
        private double? _AmoutOfQuestions;
        private double? _AmoutOfCards;
        private double? _AmoutOfQuestionsPerCard;
        private string _ErrorText;

        private DistributorState _CurrentDistributorStatus;

        private BackgroundWorker _WorkerToDistribute;

        public double MaxSemelhanca
        {
            get
            {
                return _MaxSemelhanca;
            }
            set
            {
                Set(ref _MaxSemelhanca, value);
            }
        }

        public double? AmountOfQuestions
        {
            get
            {
                return _AmoutOfQuestions;
            }
            set
            {
                Set(ref _AmoutOfQuestions, value);
            }
        }

        public double? AmountOfCards
        {
            get
            {
                return _AmoutOfCards;
            }
            set
            {
                Set(ref _AmoutOfCards, value);
            }
        }

        public double? AmountOfQuestionsPerCard
        {
            get
            {
                return _AmoutOfQuestionsPerCard;
            }
            set
            {
                Set(ref _AmoutOfQuestionsPerCard, value);
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

        public DistributorViewModel()
        {
            _WorkerToDistribute = new BackgroundWorker();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DistributeQuestionsCommand = new RelayCommand(DistributeQuestions);
        }

        private void DistributeQuestions()
        {
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
                    ErrorText = "A máxima semelhança não pode ser zero.";
                    return;
                case 5:
                    CurrentDistributorStatus = DistributorState.Waiting;
                    ErrorText = "A quantidade de questões por cartela não pode ser maior que a quantidade de questões.";
                    return;
            }

            bool succeeded = false;
            Cartela[] cartelas = null;

            _WorkerToDistribute.DoWork += (s, e) =>
            {
                succeeded = DistributeQuestions(out cartelas);
            };

            _WorkerToDistribute.RunWorkerCompleted += (s, e) =>
            {
                CurrentDistributorStatus = succeeded ? DistributorState.Success : DistributorState.Error;

                MessengerInstance.Send(new LaunchFinishedDistributionMessage(cartelas, (int)MaxSemelhanca));
            };

            CurrentDistributorStatus = DistributorState.Working;

            _WorkerToDistribute.RunWorkerAsync();
        }

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
                    int val = r.Next(Convert.ToInt32(AmountOfQuestions.Value)) + 1;
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

                if(maxSemelhanca <= MaxSemelhanca)
                {
                    i++;
                }

            } while(i < AmountOfCards);

            return true;
        }

        private int ValidateInputs()
        {
            
            if(AmountOfQuestions == null || AmountOfQuestions <= 0)
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
            else if(MaxSemelhanca <= 0)
            {
                return 4;
            }
            else if (AmountOfQuestions < AmountOfQuestionsPerCard)
            {
                return 5;
            }
            return 0;
        }
    }
}