using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class TruequeView : ContentPage
    {
        public TruequeView()
        {
            InitializeComponent();
            BindingContext = new TruequeViewModel(this);
        }
    }
}
