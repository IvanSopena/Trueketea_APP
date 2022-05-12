using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class ProfileView : ContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel(this);
        }
    }
}
