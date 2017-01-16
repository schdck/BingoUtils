﻿using BingoUtils.Domain.Entities;
using BingoUtils.UI.BingoPlayer.ViewModel.Pages;
using BingoUtils.UI.BingoPlayer.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.Messages
{
    public class StartNewGameMessage
    {
        public GameViewModel Target { get; private set; }
        public Game GamePage { get; private set; }
        public List<Question> QuestionList { get; private set; }

        public StartNewGameMessage(GameViewModel target, Game gamePage, List<Question> questionList)
        {
            Target = target;
            GamePage = gamePage;
            QuestionList = questionList;
        }
    }
}
