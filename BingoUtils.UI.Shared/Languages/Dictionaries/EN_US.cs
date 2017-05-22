using System;
using System.Windows.Controls;
using BingoUtils.Domain.Entities;
using BingoUtils.UI.Shared.Languages.Help;

namespace BingoUtils.UI.Shared.Languages.Dictionaries
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

        public override string GENERIC_CLOSE
        {
            get
            {
                return "Close";
            }
        }

        public override string HEADER_ABOUT
        {
            get
            {
                return "About";
            }
        }

        public override string HEADER_CREATE_GAME
        {
            get
            {
                return "Create game";
            }
        }

        public override string HEADER_DISTRIBUTOR
        {
            get
            {
                return "Distributor";
            }
        }

        public override string HEADER_DISTRIBUTOR_RESULT
        {
            get
            {
                return "Distributor result";
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

        public override string HEADER_SETTINGS
        {
            get
            {
                return "Settings";
            }
        }

        public override UserControl HELP_INTRO
        {
            get
            {
                return new Intro_EN_US();
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

        public override string MENU_SETTINGS
        {
            get
            {
                return "Settings";
            }
        }

        public override string MENU_START_NEW_GAME
        {
            get
            {
                return "Start new game";
            }
        }

        public override string OTHER_LANGUAGE
        {
            get
            {
                return "Language";
            }
        }

        public override string OTHER_RESTART_TO_APPLY
        {
            get
            {
                return "Restart to apply changes.";
            }
        }

        public override string OTHER_SAVED
        {
            get
            {
                return "Saved.";
            }
        }

        public override string OTHER_SELECT_FILE_TEXT
        {
            get
            {
                return "Drag and drop a file or click here to select it";
            }
        }

        public override string OTHER_SELECT_LANGUAGE
        {
            get
            {
                return "SELECT YOUR LANGUAGE";
            }
        }

        public override string OTHER_UNKNOWN_VERSION
        {
            get
            {
                return "Unknown version";
            }
        }

        public override string OTHER_USE_MAIN_WINDOW
        {
            get
            {
                return "Use the main window to manage this tab";
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
