using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class ChatViewModel: ViewModelBase
    {
        public Command ReturnCommand { get; }

        public ChatViewModel(ContentPage view)
        {
            ReturnCommand = new Command(ReturnWindow);
        }



        public async void ReturnWindow(object obj)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PopAsync();
        }

    }
}
