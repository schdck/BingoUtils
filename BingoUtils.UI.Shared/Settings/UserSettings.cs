﻿using BingoUtils.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BingoUtils.UI.Shared.Settings
{
    public class UserSettings
    {
        public static string UserLanguage
        {
            get
            {
                return Properties.Settings.Default.UserLanguage;
            }
            set
            {
                Properties.Settings.Default.UserLanguage = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
