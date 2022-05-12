using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class BienvenidaViewModel: ViewModelBase
    {

        private readonly DBContext _dbContext = ViewModelLocator.sql;
        public Command SingInCommand { get; set; }
        public Command SingUpCommand { get; set; }

        public BienvenidaViewModel()
        {

            if(_dbContext.IsOpen==false)
            {
                this.ViewMessge();
            }
           

            SingInCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LoginViewModel>();
            });

            SingUpCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<RegisterViewModel>();
            });
        }

        private async void ViewMessge()
        {
            Warnings wg = new Warnings();
            wg.Warning(_dbContext.DbLastError);
           
        }


       

       

    }
}
