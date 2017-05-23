using System.Windows.Controls;

namespace BingoUtils.Domain.Entities
{
    public abstract class LanguageDictionary
    {
        // TAB TITLES (HEADER)
        public abstract string HEADER_MENU { get; }
        public abstract string HEADER_CREATE_GAME { get; }
        public abstract string HEADER_NEW_GAME { get; }
        public abstract string HEADER_GAME { get; }
        public abstract string HEADER_DISTRIBUTOR { get; }
        public abstract string HEADER_DISTRIBUTOR_RESULT { get; }
        public abstract string HEADER_HELP { get; }
        public abstract string HEADER_ABOUT { get; }
        public abstract string HEADER_SETTINGS { get; }

        // Main menu
        public abstract string MENU_START_NEW_GAME { get; }
        public abstract string MENU_CREATE_NEW_GAME { get; }
        public abstract string MENU_DISTRIBUTE_QUESTIONS { get; }
        public abstract string MENU_SETTINGS { get; }
        public abstract string MENU_HELP { get; }
        public abstract string MENU_ABOUT { get; }

        // Start new game
        public abstract string START_NEW_GAME_FROM_MODEL { get; }
        public abstract string START_NEW_GAME_FROM_FILE { get; }
        public abstract string START_NEW_GAME_NOT_SELECTED { get; }
        public abstract string START_NEW_GAME_SELECT_SUBJECT { get; }
        public abstract string START_NEW_GAME_SELECT_TOPIC { get; }

        // Create Game
        public abstract string CREATE_TITLE { get; }
        public abstract string CREATE_SUBJECT_REQUIRED { get; }
        public abstract string CREATE_TOPIC_REQUIRED { get; }
        public abstract string CREATE_SAVE_GAME_AT_DEFAULTS { get; }

        // Game
        public abstract string GAME_CURRENT_QUESTION { get; }
        public abstract string GAME_PREVIOUS_QUESTION { get; }
        public abstract string GAME_REPRODUCE_QUESTION_TITLE { get; }
        public abstract string GAME_STOP_REPRODUCE_QUESTION_TITLE { get; }

        // Generic
        public abstract string GENERIC_CLOSE { get; }
        public abstract string GENERIC_QUESTIONS { get; }
        public abstract string GENERIC_SAVE { get; }

        // Other
        public abstract string OTHER_LANGUAGE { get; }
        public abstract string OTHER_RESTART_TO_APPLY { get; }
        public abstract string OTHER_SAVED { get; }
        public abstract string OTHER_SELECT_FILE_TEXT { get; }
        public abstract string OTHER_SELECT_LANGUAGE { get; }
        public abstract string OTHER_USE_MAIN_WINDOW { get; }
        public abstract string OTHER_UNKNOWN_VERSION { get; }

        // UserControls
        // QuestionHolder
        public abstract string QUESTION_HOLDER_QUESTION_TITLE { get; }
        public abstract string QUESTION_HOLDER_QUESTION_ANSWER { get; }

        // Pages
        public abstract UserControl HELP_INTRO { get; }
    }
}
