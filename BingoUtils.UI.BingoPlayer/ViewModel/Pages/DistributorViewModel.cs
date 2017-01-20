using BingoUtils.Domain.Enums;
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

        public double? AmoutOfQuestions
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

        public double? AmoutOfCards
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

        public double? AmoutOfQuestionsPerCard
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
                case 0:

                    return;
                case 1:

                    return;
                case 2:

                    return;
                case 3:

                    return;
            }

            bool succeeded = false;

            _WorkerToDistribute.DoWork += (s, e) =>
            {
                System.Threading.Thread.Sleep(5000);
            };

            _WorkerToDistribute.RunWorkerCompleted += (s, e) =>
            {
                CurrentDistributorStatus = succeeded ? DistributorState.Success : DistributorState.Error;
            };

            CurrentDistributorStatus = DistributorState.Working;

            _WorkerToDistribute.RunWorkerAsync();
        }

        private int ValidateInputs()
        {
            return 5;
        }
    }
}