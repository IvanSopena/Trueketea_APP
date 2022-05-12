using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TrueketeaApp.PageModels.Base
{
    public class ExtendedBindableObject : BindableObject
    {
        
        // Simplifica el proceso de actualizar una propiedad enlazable y llamar a INotifyPropertyChanged
        
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }
    }
}
