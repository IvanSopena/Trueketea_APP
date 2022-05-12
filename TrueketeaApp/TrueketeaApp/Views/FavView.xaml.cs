using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class FavView : ContentPage
    {
        public FavView()
        {
            InitializeComponent();
            BindingContext = new FavViewModel(this);
        }
    }
}
