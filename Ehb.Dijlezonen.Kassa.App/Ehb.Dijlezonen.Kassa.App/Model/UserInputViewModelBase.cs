using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class UserInputViewModelBase : INotifyPropertyChanged
    {

        protected UserInputViewModelBase()
        {
        }

        private event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }

            remove
            {
                PropertyChanged -= value;
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        
        protected bool Set<T>(ref T field, T value, Action post = null, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            post?.Invoke();
            return true;
        }
    }
}
