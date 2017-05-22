using BingoUtils.Domain.Entities;
using BingoUtils.UI.Shared.Languages.Dictionaries;
using BingoUtils.UI.Shared.Settings;
using System;
using System.Collections.Generic;

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

        public IEnumerable<string> AvaliableLanguages
        {
            get
            {
                foreach(string key in LanguageMapper.Keys)
                {
                    yield return key;
                }
            }
        }

        public Dictionary<string, Type> LanguageMapper { get; private set; }

        public string CurrentLanguageName
        {
            get
            {
                return GetLanguageNameByType(CurrentLanguage.GetType());
            }
        }

        public LanguageDictionary CurrentLanguage { get; private set; }

        private LanguageLocator()
        {
            LanguageMapper = new Dictionary<string, Type>();

            LanguageMapper.Add("English (US)", typeof(EN_US));
            LanguageMapper.Add("Português (BR)", typeof(PT_BR));

            CurrentLanguage = GetLanguageByName(UserSettings.UserLanguage);
        }

        public string GetLanguageNameByType(Type t)
        {
            foreach(KeyValuePair<string, Type> p in LanguageMapper)
            {
                if(p.Value == t)
                {
                    return p.Key;
                }
            }

            return string.Empty;
        }

        public LanguageDictionary GetLanguageByName(string name)
        {
            foreach(KeyValuePair<string, Type> p in LanguageMapper)
            {
                if(p.Key == name)
                {
                    return Activator.CreateInstance(p.Value) as LanguageDictionary;
                }
            }
            return new EN_US();
        }
    }
}
