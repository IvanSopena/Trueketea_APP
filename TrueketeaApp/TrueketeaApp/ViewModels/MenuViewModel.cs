using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.AppConstant;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        Warnings wg = new Warnings();
        B8Clases B8 = new B8Clases();
        
        public Command TruequesCommand { get; set; }

        public Command ProfileCommand { get; set; }
        public Command InfoCommand { get; set; }
        public Command ReportIncidentCommand { get; set; }
        public Command CloseCommand { get; set; }
        public string MyVotes { get; set; }

        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }



        ContentPage myview;

        public MenuViewModel(ContentPage view)
        {

            MyVotes = B8.DBLookupEx("view_users", "Vote", "Email", ViewModelLocator.MyEmail);
            ShowVotes();
            UserName = ViewModelLocator.MyName;
            UserPhoto = ViewModelLocator.MyPhoto;
            USerEmail = ViewModelLocator.MyEmail;
            myview = view;
            this.TruequesCommand = new Command(this.Trueques);
            this.ProfileCommand = new Command(this.ShowProfile);
            this.InfoCommand = new Command(InfoTapped);
            this.CloseCommand = new Command(CloseTapped);
            this.ReportIncidentCommand = new Command(ReportIncident);
        }


        private async void ShowProfile(object obj)
        {
            await NavigationService.NavigateToAsync<ProfileViewModel>();
        }

        async private void Trueques(object obj)
        {
            await NavigationService.NavigateToAsync<TruequeViewModel>();
        }

        private async void InfoTapped(object obj)
        {
            await NavigationService.NavigateToAsync<InformationViewModel>();
        }

        private async void ReportIncident(object obj)
        {
            await NavigationService.NavigateToAsync<IncidentReportViewModel>();
        }

        private async void CloseTapped(object obj)
        {
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await wg.ToastInfo("Sesión Cerrada", myview);
        }

        private void ShowVotes()
        {

            if(Convert.ToInt32(MyVotes) == 0)
            {
                Star1 = "startlight";
                Star2 = "startlight";
                Star3 = "startlight";
                Star4 = "startlight";
                Star5 = "startlight";
            }

            if (Convert.ToInt32(MyVotes) <= 10)
            {
                Star1 = "start";
                Star2 = "startlight";
                Star3 = "startlight";
                Star4 = "startlight";
                Star5 = "startlight";
            }

            if (Convert.ToInt32(MyVotes) > 10 && Convert.ToInt32(MyVotes) <= 20)
            {
                Star1 = "start";
                Star2 = "start";
                Star3 = "startlight";
                Star4 = "startlight";
                Star5 = "startlight";
            }

            if (Convert.ToInt32(MyVotes) > 20 && Convert.ToInt32(MyVotes) <= 30)
            {
                Star1 = "start";
                Star2 = "start";
                Star3 = "start";
                Star4 = "startlight";
                Star5 = "startlight";
            }

            if (Convert.ToInt32(MyVotes) > 30 && Convert.ToInt32(MyVotes) <= 40)
            {
                Star1 = "start";
                Star2 = "start";
                Star3 = "start";
                Star4 = "start";
                Star5 = "startlight";
            }

            if (Convert.ToInt32(MyVotes) > 40 )
            {
                Star1 = "start";
                Star2 = "start";
                Star3 = "start";
                Star4 = "start";
                Star5 = "start";
            }

        }

    }
}
