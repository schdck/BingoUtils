using BingoUtils.Domain.Entities;
using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using System.Collections.Generic;

namespace BingoUtils.UI.BingoPlayer.Messages
{
    public class StartNewGameMessage
    {
        public GameViewModel Target { get; private set; }
        public Game GamePage { get; private set; }
        public GameAnswers GameAnswersPage { get; private set; }
        public List<Question> QuestionList { get; private set; }

        public StartNewGameMessage(GameViewModel target, Game gamePage, GameAnswers gameAnswersPage, List<Question> questionList)
        {
            Target = target;
            GamePage = gamePage;
            GameAnswersPage = gameAnswersPage;
            QuestionList = questionList;
        }
    }
}
