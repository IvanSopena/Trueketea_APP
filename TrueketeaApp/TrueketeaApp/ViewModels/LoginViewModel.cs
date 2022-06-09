using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using TrueketeaApp.AuthHelpers;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Auth;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {


        B8Clases B8 = new B8Clases();
        EntryValidation validation;
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        ContentPage myView;
        Entry email;
        Entry pass;
        Account account;
        AccountStore store;
        public static RegisterData reg = new RegisterData();
        User user;

        private string emailAddress;
        public string EmailAddress
        {
            get => emailAddress;
            set => SetProperty(ref emailAddress, value);
        }

        private string _err;
        public string Error
        {
            get => _err;
            set => SetProperty(ref _err, value);
        }

        ////************************hay que borrar esto******************
        public Command TempCommand { get; set; }

        //***************************************************************

        public LoginViewModel(ContentPage view)
        {
            IsVisbleMail = false;
            IsVisblePass = false;

            store = AccountStore.Create();

            myView = view;
            email = myView.FindByName<Entry>("EMAIL");
            email.Unfocused += EmailEntry_Unfocused;
            pass = myView.FindByName<Entry>("Password");


            ////************************hay que borrar esto******************

            //this.TempCommand = new Command(this.TempFunction);

            //***************************************************************



            this.LoginCommand = new Command(this.LoginClicked);
            this.GoogleCommand = new Command(this.LoginGoogle);
            this.FBCommand = new Command(this.LoginFB);


            ForgotPasswordCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LostPassViewModel>();
            });

            validation = new EntryValidation();



        }


        ////************************hay que borrar esto******************

        //async private void TempFunction(object obj)
        //{
        //    IsBusy = true;
        //    AppConstant.Constants.UserLoginEmail = "Alfredo@gmail.com";
        //    AppConstant.Constants.UserLoginId = "3";
        //    AppConstant.Constants.UserDirection = "";
        //    AppConstant.Constants.UserName = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", AppConstant.Constants.UserLoginEmail);
        //    AppConstant.Constants.UserProfilePhoto = "https://firebasestorage.googleapis.com/v0/b/trueketea-bd250.appspot.com/o/System%2Fno_photo.jpeg?alt=media&token=6a5466f5-7d02-4375-b4bf-c7472557daeb";
        //    B8.UpdateExpress("Login_Users", "User_Email", AppConstant.Constants.UserLoginEmail, "Connected", "1");

        //    ChageCommonsVariables();


        //    //var navigationPage = Application.Current.MainPage as NavigationPage;
        //    //await navigationPage.PushAsync(new HomeView());
        //    await NavigationService.NavigateToAsync<HomeViewModel>();


        //    IsBusy = false;
        //}

        //***************************************************************


        


        private void EmailEntry_Unfocused(object sender, FocusEventArgs e)
        {

            if (validation.Email_validacion(email.Text) == false)
            {
                IsVisbleMail = true;
                Error = "El Email no es correcto";
            }
            else
            {
                IsVisbleMail = false;
            }
        }

        public bool AreFieldsValid()
        {
            bool isEmailValid = validation.Verify_Email(email.Text);
            bool isPasswordValid = validation.Verify_Pass(pass.Text, email.Text);
            return isEmailValid && isPasswordValid;
        }


        public Command LoginCommand { get; set; }
        public Command ForgotPasswordCommand { get; set; }

        public Command GoogleCommand { get; set; }

        public Command FBCommand { get; set; }


        async private void LoginClicked(object obj)
        {
            IsBusy = true;
            if (this.AreFieldsValid())
            {
                string thisDay = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss");
                B8.UpdateExpress("Login_Users", "User_Email", email.Text, "Last_Conection", "'Conectado'");

                AppConstant.Constants.UserLoginEmail = email.Text;
                AppConstant.Constants.UserLoginId = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "User_Id", "User_Email", email.Text);
                AppConstant.Constants.UserDirection = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Direction", "Email", email.Text);
                AppConstant.Constants.UserProfilePhoto = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "PhotoPath", "Email", email.Text);
                AppConstant.Constants.UserName = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", email.Text);
                B8.UpdateExpress("Login_Users", "User_Email", email.Text, "Connected", "1");
                IsBusy = false;
                ChageCommonsVariables();
                await NavigationService.NavigateToAsync<HomeViewModel>();
               // var navigationPage = Application.Current.MainPage as NavigationPage;
                //await navigationPage.PushAsync(new HomeView());

            }
            else
            {

                Warnings wg = new Warnings();
                await wg.ToastWarning("Email o Contraseña incorrectos", myView);
                IsBusy = false;
            }
            //await NavigationService.NavigateToAsync<HomeViewModel>();
        }

        private void LoginGoogle(object obj)
        {
            //coigo poporciondo por https://github.com/xamarin/Essentials/tree/develop/Samples/Sample.Server.WebAuthenticator & https://www.youtube.com/watch?v=ZCPENg5wP8o
            string clientId = null;
            string redirectUri = null;

            IsBusy = true;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = AppConstant.Constants.iOSClientId;
                    redirectUri = AppConstant.Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = AppConstant.Constants.AndroidClientId;
                    redirectUri = AppConstant.Constants.AndroidRedirectUrl;
                    break;
            }

            account = store.FindAccountsForService(AppConstant.Constants.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                AppConstant.Constants.Scope,
                new Uri(AppConstant.Constants.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(AppConstant.Constants.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);


        }
        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {

            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            user = null;
            if (e.IsAuthenticated)
            {
                // If the user is authenticated, request their basic user data from Google
                // UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
                var request = new OAuth2Request("GET", new Uri(AppConstant.Constants.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();

                if (response != null)
                {
                    // Deserialize the data and store it in the account store
                    // The users email address will be used to identify data in SimpleDB
                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);

                }

                if (user != null)
                {

                    this.ExternalReg();
                    IsBusy = false;

                    // NavigationService.NavigateToAsync<HomeViewModel>();

                }



            }

        }

        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }

        void OnAuthErrorFB(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompletedFB;
                authenticator.Error -= OnAuthErrorFB;
            }

            Debug.WriteLine("Authentication error: " + e.Message);
        }


        private void LoginFB(object obj)
        {




            string clientId = null;
            string redirectUri = null;

            IsBusy = true;

            clientId = AppConstant.Constants.FBIdApp;
            redirectUri = $"fb{clientId}://authorize";//AppConstant.Constants.FBRedirectUrl;



            //// account = store.FindAccountsForService(AppConstant.Constants.AppName).FirstOrDefault();


            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                null,
                new Uri(AppConstant.Constants.FBAuthorizeUrl),
                new Uri(redirectUri),
                new Uri(AppConstant.Constants.FBAccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompletedFB;
            authenticator.Error += OnAuthError;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        async void OnAuthCompletedFB(object sender, AuthenticatorCompletedEventArgs e)
        {

            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompletedFB;
                authenticator.Error -= OnAuthErrorFB;
            }

            user = null;
            if (e.IsAuthenticated)
            {

                var request = new OAuth2Request("GET", new Uri(AppConstant.Constants.FBUserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();

                if (response != null)
                {

                    string userJson = await response.GetResponseTextAsync();
                    user = JsonConvert.DeserializeObject<User>(userJson);

                }

                if (user != null)
                {


                    this.ExternalReg();
                    IsBusy = false;

                    //await NavigationService.NavigateToAsync<HomeViewModel>();

                }



            }

        }




        private async void ExternalReg()
        {
            string email = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "User_Email", "User_Email", user.Email);

            if (string.IsNullOrEmpty(email))
            {
                Warnings waning = new Warnings();

                reg.UserId = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "Max(User_Id) +1 ", "1", "1");
                reg.RolId = 0;
                reg.UserName = user.Name;
                reg.UserEmail = user.Email;
                reg.Password = "";
                reg.LastConnection = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss");
                reg.Device = DeviceInfo.Platform.ToString();
                reg.Photo = user.Picture;


                int insert = 0;
                string query = "";

                query = $"Insert into { _dbContext.TableOwner}.Login_Users (User_Id,Rol_Id,User_Name," +
                    $"User_Email,Password,Last_Conection,Device,Active,Photo)" +
                    $"Values ('{reg.UserId}','0','{reg.UserName}','{reg.UserEmail}','{reg.Password}',convert(datetime,'{reg.LastConnection}',103),'{reg.Device}','1','{user.Picture}')";

                if (0 != _dbContext.DbExecute(query, ref insert))
                {
                    B8.Log_Error("LoginViewModel", "ExternalReg", _dbContext.DbLastError, query);

                    await waning.ToastWarning("Error en el sistema de validación.Intentalo mas tarde.", myView);
                }
                else
                {
                    AppConstant.Constants.UserLoginId = reg.UserId;
                    AppConstant.Constants.UserName = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", reg.UserEmail);
                    AppConstant.Constants.UserLoginEmail = reg.UserEmail;
                    AppConstant.Constants.UserProfilePhoto = reg.Photo;
                    B8.UpdateExpress("Login_Users", "User_Email", reg.UserEmail, "Connected", "1");
                    ChageCommonsVariables();


                    query = $"Insert into { _dbContext.TableOwner}.ChatRelationship (User1,User2,Conversation_id," +
                       $"MsgType,MsgStatus,LastMsg)" +
                       $"Values ('{reg.UserId}','0','6279ee72d15a035968357a24','Text','New','Bienvenido')";

                    if (0 != _dbContext.DbExecute(query, ref insert))
                    {
                        B8.Log_Error(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, _dbContext.DbLastError, query);
                        //await waning.Warning("Error en la validacion del email");
                        //await waning.ToastWarning("Error en el sistema de validación.Intentalo mas tarde.", myView);
                    }


                    await NavigationService.NavigateToAsync<HomeViewModel>();
                }
            }
            else
            {
                AppConstant.Constants.UserLoginId = B8.DBLookupEx(_dbContext.TableOwner + ".Login_Users", "User_Id", "User_Email", user.Email);
                AppConstant.Constants.UserLoginEmail = user.Email;
                AppConstant.Constants.UserName = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", user.Email);
                AppConstant.Constants.UserDirection = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Direction", "Email", user.Email);
                AppConstant.Constants.UserProfilePhoto = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "PhotoPath", "Email", user.Email);
                B8.UpdateExpress("Login_Users", "User_Email", user.Email, "Connected", "1");
                ChageCommonsVariables();
                await NavigationService.NavigateToAsync<HomeViewModel>();
            }

        }

    }
}
