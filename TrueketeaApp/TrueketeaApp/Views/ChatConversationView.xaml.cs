using System;
using System.Collections.Generic;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.Views
{
    public partial class ChatConversationView : ContentPage
    {
        public ChatConversationView(string foto, string user, bool status,string usr_id)
        {
            InitializeComponent();
            BindingContext = new ConversationViewModel(this, foto, user, status, usr_id);
        }
    }
}
