using BingoUtils.Domain.Entities;
using BingoUtils.UI.BingoPlayer.Languages.Dictionaries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingoUtils.UI.BingoPlayer.Languages
{
    public class LanguageLocator
    {
        private const string DictionariesLocation = "/Languages/Dictionaries/";

        private static LanguageLocator _Instance;

        public static LanguageLocator Instance
        {
            get
            {
                return _Instance == null ? (_Instance = new LanguageLocator()) : _Instance;    
            }
        }

        public LanguageDictionary CurrentLanguage { get; private set; } = new EN_US();

        private LanguageLocator() { }
    }
}
