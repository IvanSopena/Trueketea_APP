using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class InformationViewModel
    {
        public Command TermsCommand { get; set; }
        public Command PrivacyCommand { get; set; }
        public InformationViewModel()
        {
            this.TermsCommand = new Command(this.ShowTerms);
            this.PrivacyCommand = new Command(ShowPrivacy);
        }

        private async void ShowTerms(object obj)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new TermsView());
        }

        private async void ShowPrivacy(object obj)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new TermsView());
        }

    }
}
