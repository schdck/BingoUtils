using BingoUtils.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoUtils.UI.BingoPlayer.Languages.Dictionaries
{
    public class EN_US : LanguageDictionary
    {
        public override string GAME_CURRENT_QUESTION
        {
            get
            {
                return "Current Question";
            }
        }

        public override string GAME_PREVIOUS_QUESTION
        {
            get
            {
                return "Previous Question";
            }
        }

        public override string GAME_REPRODUCE_QUESTION_TITLE
        {
            get
            {
                return "Reproduce question title";
            }
        }

        public override string GAME_STOP_REPRODUCE_QUESTION_TITLE
        {
            get
            {
                return "Stop question title reproduction";
            }
        }

        public override string HEADER_ABOUT
        {
            get
            {
                return "About";
            }
        }

        public override string HEADER_DISTRIBUTOR
        {
            get
            {
                return "Distributor";
            }
        }

        public override string HEADER_GAME
        {
            get
            {
                return "Game";
            }
        }

        public override string HEADER_HELP
        {
            get
            {
                return "Help";
            }
        }

        public override string HEADER_MENU
        {
            get
            {
                return "Menu";
            }
        }

        public override string HEADER_NEW_GAME
        {
            get
            {
                return "New game";
            }
        }

        public override string MENU_ABOUT
        {
            get
            {
                return "About";
            }
        }

        public override string MENU_CREATE_NEW_GAME
        {
            get
            {
                return "Create new game";
            }
        }

        public override string MENU_DISTRIBUTE_QUESTIONS
        {
            get
            {
                return "Distribute questions";
            }
        }

        public override string MENU_HELP
        {
            get
            {
                return "Help";
            }
        }

        public override string MENU_START_NEW_GAME
        {
            get
            {
                return "Start new game";
            }
        }

        public override string OTHER_SELECT_FILE_TEXT
        {
            get
            {
                return "Drag and drop a file or click here to select it";
            }
        }

        public override string START_NEW_GAME_FROM_FILE
        {
            get
            {
                return "Start new game from file";
            }
        }

        public override string START_NEW_GAME_FROM_MODEL
        {
            get
            {
                return "Start new game from model";
            }
        }

        public override string START_NEW_GAME_NOT_SELECTED
        {
            get
            {
                return "Select wheter you want to start the game from the model or the file";
            }
        }

        public override string START_NEW_GAME_SELECT_MATTER
        {
            get
            {
                return "Select the matter";
            }
        }

        public override string START_NEW_GAME_SELECT_SUBJECT
        {
            get
            {
                return "Select the subject";
            }
        }
    }
}
