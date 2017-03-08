using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingoUtils.UI.BingoPlayer.Languages
{
    public static class LanguageMapper
    {
        private const string DictionariesLocation = "/Languages/Dictionaries/";

        public static ResourceDictionary CurrentLanguage { get; private set; }

        public static readonly ResourceDictionary PTBR = 
            new ResourceDictionary() { Source = new Uri(Path.Combine(DictionariesLocation, "PT-BR"), UriKind.Relative) };
        
        public static readonly ResourceDictionary ENUS =
            new ResourceDictionary() { Source = new Uri(Path.Combine(DictionariesLocation, "EN-US"), UriKind.Relative) };

        static LanguageMapper()
        {

        }
    }
}
