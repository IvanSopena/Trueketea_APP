using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TrueketeaAdmin.Services.Security;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase 
    {

        private readonly ContentPage myView;
        private readonly Entry email;
        private readonly Entry pass;
        private readonly Entry UserName;
        private readonly Entry ValiPass;
        private readonly CheckBox check;
        private readonly EntryValidation validation = new EntryValidation();
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        private readonly B8Clases B8 = new B8Clases();
        public static RegisterData reg = new RegisterData();
        private readonly Encrypting Encrypt = new Encrypting();
        private Warnings wg = new Warnings();

        private string _err;
        public string Error
        {
            get => _err;
            set => SetProperty(ref _err, value);
        }

        public RegisterViewModel(ContentPage view)
        {
            IsVisbleMail = false;

            myView = view;
            email = myView.FindByName<Entry>("Mail");
            UserName = myView.FindByName<Entry>("User");
            pass = myView.FindByName<Entry>("Password");
            ValiPass = myView.FindByName<Entry>("VerifyPassword");
            check = myView.FindByName<CheckBox>("checkBox");

            this.RegisterCommand = new Command(this.ButtonClicked);
            this.Terms = new Command(this.NavToTerms);

        }

        public Command RegisterCommand { get; set; }
        public Command Terms { get; set; }

        async private void NavToTerms(object obj)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new TermsView());
            
        }



        async private void  ButtonClicked(object obj)
        {
            if(check.IsChecked == true)
            {
                IsBusy = true;

                if (validation.Register_Validation(UserName.Text, email.Text, pass.Text, ValiPass.Text) == true)
                {

                    reg.Active = 1;
                    reg.UserId = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "Max(User_Id) +1 ", "1", "1");
                    reg.RolId = 0;
                    reg.UserName = UserName.Text;
                    reg.UserEmail = email.Text;
                    reg.Password = Encrypt.Encrypt(pass.Text, email.Text);
                    reg.LastConnection = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss");
                    reg.Device = DeviceInfo.Platform.ToString();
                    reg.Photo = "No_Photo";

                    string query;
                    int insert = 0;
                    string code = B8.ValidationCode();

                    EmailMessages msg = new EmailMessages(reg.UserEmail);

                    query = $"Insert into ValidationEmails (Email,Code,Result) Values('{ reg.UserEmail}','{code}',0)";
                    if (0 != _dbContext.DbExecute(query, ref insert))
                    {
                        B8.Log_Error(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, _dbContext.DbLastError, query);
                        Error = "Error: Fallo al insertar el usuario";
                        await wg.ToastError(Error, myView);
                    }
                    if (insert > 0)
                    {
                        msg.Send_Email(2, code);
                        await NavigationService.NavigateToAsync<ValidarViewModel>();
                    }

                }
                else
                {
                    Error = validation.ValidationError;
                    IsVisbleMail = true;
                    IsBusy = false;
                }
            }
            else
            {
                await wg.ToastInfo("Debe aceptar los términos de uso.", myView);
            }
            

        }

      
    }
}
