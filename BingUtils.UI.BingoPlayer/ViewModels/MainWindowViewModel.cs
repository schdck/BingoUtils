using BingoUtils.Domain.Entities;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingUtils.UI.BingoPlayer.ViewModels
{
    public class MainWindowViewModel : DefaultViewModel
    {
        public List<Question> QuestionsList { get; private set; }

        public string CurrentQuestionTitle { get; private set; }
        public string PreviousQuestioTitle { get; private set; }
        public string QuestionProgress { get; private set; }

        public TransitionType AnimationToBeUsed { get; private set; }

        public SimpleDelegateCommand RefreshQuestionCommand { get; private set; }

        public MainWindowViewModel()
        {
            CurrentQuestionTitle = "Seja bem-vindo.\nPara iniciar um jogo clique no botão abaixo.";
            QuestionProgress = "1/60";

            RefreshQuestionCommand = new SimpleDelegateCommand((x) => CurrentQuestionTitle = "Olá");
        }
    }
}
