﻿using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BingoUtils.Domain.Entities
{
    
    public abstract class DefaultViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
