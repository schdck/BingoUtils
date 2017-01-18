using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingoUtils.UI.BingoPlayer.ViewModel.Pages
{
    public class DistributorViewModel : BaseViewModel
    {
        private double _MaxSemelhanca = 75;
        private double? _AmoutOfQuestions;
        private double? _AmoutOfCards;
        private double? _AmoutOfQuestionsPerCard;
        private UIElement _CurrentStatusElement;

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

        public UIElement CurrentStatusElement
        {
            get
            {
                return _CurrentStatusElement;
            }
            private set
            {
                Set(ref _CurrentStatusElement, value);
            }
        }

        public DistributorViewModel()
        {
            CurrentStatusElement = ["Status_Error"] as UIElement;
        }
    }
} // Resources["StackPanel_FileSelected"];