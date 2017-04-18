using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Ehb.Dijlezonen.Kassa.App.Shared.Model
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        protected ViewModelBase()
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
            NotifyPropertyChanged(propertyName);
            field = value;
            NotifyPropertyChanged(propertyName);
            post?.Invoke();
            return true;
        }

        public void NotifyPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression unaryExpression)
            {
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression)lambda.Body;
            }

            NotifyPropertyChanged(memberExpression.Member.Name);
        }
    }
}
