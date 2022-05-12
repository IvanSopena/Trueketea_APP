using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Services;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{

    public class UserInfoViewModel: ViewModelBase
    {
        B8Clases B8 = new B8Clases();
        ContentPage MyView;
        Warnings wg = new Warnings();

        public string MyVotes { get; set; }
        public string LastConnection { get; set; }
        public string Star1 { get; set; }
        public string Star2 { get; set; }
        public string Star3 { get; set; }
        public string Star4 { get; set; }
        public string Star5 { get; set; }
        private string clave;
        public Command AddFriendCommand { get; }
        public string Articles { get; set; }
        public string Comments { get; set; }

        public Command RefreshCommand { private set; get; }
        public ObservableCollection<Item> MyProducts { get; private set; }
        public ObservableCollection<CommentsModel> MyComments { get; private set; }

        public Command AddCommentCommand { get; }

        public UserInfoViewModel(ContentPage View, string userID)
        {
            MyView = View;
            clave = B8.DBLookupEx("view_users","User_id","Email",userID);

            if (B8.DBLookupEx("FavProfiles","count(Fav_id)","User_Id",AppConstant.Constants.UserLoginId,"Fav_Id",clave) != "0")
            {
                ButtonText = "Siguiendo";
            }
            else
            {
                ButtonText = "Seguir";
            }

            MyVotes = B8.DBLookupEx("view_users", "Vote", "User_id", clave);
            ShowVotes();
            UserName = B8.DBLookupEx("view_users", "Name", "User_id", clave);
            UserPhoto = B8.DBLookupEx("view_users", "PhotoPath", "User_id", clave);
            LastConnection = "Última conexión: " + B8.DBLookupEx("view_users", "Last_Conection", "User_id", clave);
            USerEmail = userID;
            RefreshCommand = new Command(async () => await RefreshItems());
            GetMyProducts();
            GetMyComments();
            AddCommentCommand = new Command(this.AddNewComment);
            AddFriendCommand = new Command(this.AddNewFriend);
            AppConstant.Constants.ActualView = "info";

        }

        private async Task RefreshItems()
        {

            GetMyProducts();
            GetMyComments();

            IsRefreshing = false;
        }

        void GetMyProducts()
        {
            MyProducts = new ObservableCollection<Item>(DataService.GetUserItems(clave, "Products"));
            Articles = MyProducts.Count + " Articulos Subidos";

        }

        void GetMyComments()
        {
            MyComments = new ObservableCollection<CommentsModel>(DataService.GetComments(clave));
            Comments = MyComments.Count + " Comentarios";
        }

        async private void AddNewComment(object obj)
        {
            //await NavigationService.NavigateToAsync<CommentsViewModel>();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new CommentsView(clave));
        }

        async private void AddNewFriend(object obj)
        {
            if(ButtonText.Equals("Seguir"))
            {
                if (DataService.AddFriend(AppConstant.Constants.UserLoginId, clave) == true)
                {
                    await wg.ToastSuccess("Añadido a mis favoritos", MyView);
                    ButtonText = "Siguiendo";
                }
                else
                {
                    await wg.ToastWarning("No se ha podido añadir a mis favoritos", MyView);
                }
            }

        }

        private void ShowVotes()
        {

            if (Convert.ToInt32(MyVotes) == 0)
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

            if (Convert.ToInt32(MyVotes) > 40)
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
