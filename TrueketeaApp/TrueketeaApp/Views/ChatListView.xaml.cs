using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class ChatListView : ContentPage
    {
        public ChatListView()
        {
            InitializeComponent();
            BindingContext = new ChatListViewModel(this);
        }
    }
}
