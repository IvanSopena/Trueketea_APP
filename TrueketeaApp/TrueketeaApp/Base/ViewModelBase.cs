using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.Service;
using TrueketeaApp.Services.Navigation;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;

namespace TrueketeaApp.PageModels.Base
{
    public class ViewModelBase : ExtendedBindableObject
    {
        //private string _votesimg;
        //public string VotesImg
        //{
        //    get { return _votesimg; }
        //    set { SetProperty(ref _votesimg, value); }
        //}

        string _comments;

        public string Comment
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }

        string _note;

        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }


        string _buttontext;

        public string ButtonText
        {
            get => _buttontext;
            set => SetProperty(ref _buttontext, value);
        }

        string _Myerror;

        public string MyError
        {
            get => _Myerror;
            set => SetProperty(ref _Myerror, value);
        }

        bool _name;
        public bool IsVisibleName
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        bool _province;
        public bool IsVisibleProvince
        {
            get => _province;
            set => SetProperty(ref _province, value);
        }

        bool _isvisibleappuser;
        public bool IsVisibleAppUser
        {
            get => _isvisibleappuser;
            set => SetProperty(ref _isvisibleappuser, value);
        }


        string _catError;

        public string CatError
        {
            get => _catError;
            set => SetProperty(ref _catError, value);
        }

        string _lastconnection;

        public string LastConnection
        {
            get => _lastconnection;
            set => SetProperty(ref _lastconnection, value);
        }

        string _userphoto;

        public string UserPhoto
        {
            get => _userphoto;
            set => SetProperty(ref _userphoto, value);
        }

        string _username;

        public string UserName
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        string _userEmail;

        public string USerEmail
        {
            get => _userEmail;
            set => SetProperty(ref _userEmail, value);
        }

        string _title;

        // Título de la página, configurable en el PageModel

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        bool _isLoading;

        // Marcar para notificar a la página sobre la actividad de la red

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }


      

        bool  _isVisbleMail;

        public bool IsVisbleMail
        {
            get => _isVisbleMail;
            set => SetProperty(ref _isVisbleMail, value);
        }

        bool _isVisbleCat;

        public bool IsVisbleCat
        {
            get => _isVisbleCat;
            set => SetProperty(ref _isVisbleCat, value);
        }

        bool _isVisblePass;

        public bool IsVisblePass
        {
            get => _isVisblePass;
            set => SetProperty(ref _isVisblePass, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                SetProperty(ref _isBusy, value);
            }
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(); }
        }


        private bool _isVisibleOptions;
        public bool IsVisibleOptions
        {
            get => _isVisibleOptions;
            set { _isVisibleOptions = value; OnPropertyChanged(); }
        }


        //Realiza la inicialización del modelo de página de forma asincrónica
        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.CompletedTask;
        }


        protected readonly INavigationService NavigationService;

        public ViewModelBase()
        {
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }


        private Command<object> backButtonCommand;

        public Command<object> BackButtonCommand
        {
            get
            {
                return this.backButtonCommand ?? (this.backButtonCommand = new Command<object>(this.BackButtonClicked));
            }
        }

        public ObservableCollection<RegisterData> _UserFavs;

        public ObservableCollection<RegisterData> UserInfo
        {

            get
            {
                return this._UserFavs;
            }

            set
            {
                if (this._UserFavs == value)
                {
                    return;
                }

                this.SetProperty(ref this._UserFavs, value);
            }


            //get => _UserFavs;
            //set => SetProperty(ref _UserFavs, value);
        }

        public ObservableCollection<Item> _MyFavs;

        public ObservableCollection<Item> FavoriteProducts
        {


            get
            {
                return this._MyFavs;
            }

            set
            {
                if (this._MyFavs == value)
                {
                    return;
                }

                this.SetProperty(ref this._MyFavs, value);
            }


            //get => _MyFavs;
            //set => SetProperty(ref _MyFavs, value);
        }

        //public void GetMyFavorites()
        // {
        //public ObservableCollection<RegisterData> UserInfo { get; private set; }
        //     FavoriteProducts = new ObservableCollection<Item>(DataService.GetUserItems(AppConstant.Constants.UserLoginId, "Favorites"));

        // }

        private void BackButtonClicked(object obj)
        {

            if(AppConstant.Constants.ActualView.Equals("chat"))
            {
                AppConstant.Constants.ActualView = "";
            }


            if (Device.RuntimePlatform == Device.UWP && Application.Current.MainPage.Navigation.NavigationStack.Count > 1)
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
            else if (Device.RuntimePlatform != Device.UWP && Application.Current.MainPage.Navigation.NavigationStack.Count > 0)
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public void ChageCommonsVariables()
        {

            ViewModelLocator.MyName = AppConstant.Constants.UserName;
            ViewModelLocator.MyPhoto = AppConstant.Constants.UserProfilePhoto;
            ViewModelLocator.MyEmail = AppConstant.Constants.UserLoginEmail;
            ViewModelLocator.MyId = AppConstant.Constants.UserLoginId;
        }
    }
}
