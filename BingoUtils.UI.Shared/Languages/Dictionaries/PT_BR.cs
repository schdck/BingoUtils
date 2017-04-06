using System.Windows.Controls;
using BingoUtils.Domain.Entities;
using BingoUtils.UI.Shared.Languages.Help;

namespace BingoUtils.UI.Shared.Languages.Dictionaries
{
    public class PT_BR : LanguageDictionary
    {
        public override string GAME_CURRENT_QUESTION
        {
            get
            {
                return "Questão atual";
            }
        }

        public override string GAME_PREVIOUS_QUESTION
        {
            get
            {
                return "Questão anterior";
            }
        }

        public override string GAME_REPRODUCE_QUESTION_TITLE
        {
            get
            {
                return "Reproduzir título da questão";
            }
        }

        public override string GAME_STOP_REPRODUCE_QUESTION_TITLE
        {
            get
            {
                return "Parar reprodução";
            }
        }

        public override string HEADER_ABOUT
        {
            get
            {
                return "Sobre";
            }
        }

        public override string HEADER_CREATE_GAME
        {
            get
            {
                return "Criar um jogo";
            }
        }

        public override string HEADER_DISTRIBUTOR
        {
            get
            {
                return "Distribuidor";
            }
        }

        public override string HEADER_DISTRIBUTOR_RESULT
        {
            get
            {
                return "Resultado do distribuidor";
            }
        }

        public override string HEADER_GAME
        {
            get
            {
                return "Jogo";
            }
        }

        public override string HEADER_HELP
        {
            get
            {
                return "Ajuda";
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
                return "Novo jogo";
            }
        }

        public override string HEADER_SETTINGS
        {
            get
            {
                return "Configurações";
            }
        }

        public override UserControl HELP_INTRO
        {
            get
            {
                return new Intro_PT_BR();
            }
        }

        public override string MENU_ABOUT
        {
            get
            {
                return "Sobre";
            }
        }

        public override string MENU_CREATE_NEW_GAME
        {
            get
            {
                return "Criar novo jogo";
            }
        }

        public override string MENU_DISTRIBUTE_QUESTIONS
        {
            get
            {
                return "Distribuir questões";
            }
        }

        public override string MENU_HELP
        {
            get
            {
                return "Ajuda";
            }
        }

        public override string MENU_SETTINGS
        {
            get
            {
                return "Configurações";
            }
        }

        public override string MENU_START_NEW_GAME
        {
            get
            {
                return "Começar novo jogo";
            }
        }

        public override string OTHER_SELECT_FILE_TEXT
        {
            get
            {
                return "Arraste um arquivo ou clique aqui para selecioná-lo";
            }
        }

        public override string OTHER_UNKNOWN_VERSION
        {
            get
            {
                return "Versão desconhecida";
            }
        }

        public override string OTHER_USE_MAIN_WINDOW
        {
            get
            {
                return "Use a janela principal para gerenciar esta guia";
            }
        }

        public override string START_NEW_GAME_FROM_FILE
        {
            get
            {
                return "Começar novo jogo a partir do arquivo";
            }
        }

        public override string START_NEW_GAME_FROM_MODEL
        {
            get
            {
                return "Começar novo jogo a partir do modelo";
            }
        }

        public override string START_NEW_GAME_NOT_SELECTED
        {
            get
            {
                return "Selecione se deseja começar o jogo a partir do modelo ou do arquivo";
            }
        }

        public override string START_NEW_GAME_SELECT_MATTER
        {
            get
            {
                return "Selecione o assunto";
            }
        }

        public override string START_NEW_GAME_SELECT_SUBJECT
        {
            get
            {
                return "Selecione a disciplina";
            }
        }
    }
}
