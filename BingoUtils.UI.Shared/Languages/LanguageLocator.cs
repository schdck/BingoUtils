using BingoUtils.Domain.Entities;
using BingoUtils.UI.Shared.Languages.Dictionaries;

namespace BingoUtils.UI.Shared.Languages
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
