using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class MyArticlesView : ContentPage
    {
        public MyArticlesView()
        {
            InitializeComponent();
            BindingContext = new MyArticlesViewModel();
        }
    }
}
