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
    public class FavViewModel: ViewModelBase
    {
        private int selectedIndex;

        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                this.selectedIndex = value;
                this.ItemSelected(value);
            }
        }

        private B8Clases B8 = new B8Clases();
        public Command SelectGroupCommand { get; }
        public Command Nav { get; }
        public Command DeleteFavCommand { get; }

        public Command SelectUserCommand { get;}

        ContentPage myView;

        public FavViewModel(ContentPage view)
        {
            Title = "Artículos Favoritos";
            myView = view;
            DeleteFavCommand = new Command<Item>(async (model) => await DelFav(model));
            Nav = new Command(async () => await ShowProfiles());
            SelectGroupCommand = new Command<Item>((model) => ExecuteSelectGroupCommand(model));
            SelectUserCommand = new Command<RegisterData>((model) => SelectUserInfoCommand(model));
            GetMyFavorites();
            GetMyFriends();
        }

        void GetMyFriends()
        {
            UserInfo = new ObservableCollection<RegisterData>(DataService.GetProfilesFav());
        }

        void GetMyFavorites()
        {

            FavoriteProducts = new ObservableCollection<Item>(DataService.GetUserItems(AppConstant.Constants.UserLoginId, "Favorites"));

        }

        private async Task ShowProfiles()
        {
            await NavigationService.NavigateToAsync<ProfileFavViewModel>();
        }


        private async Task RefreshWindow()
        {
            await NavigationService.NavigateToAsync<FavViewModel>();
        }

        private async void SelectUserInfoCommand(RegisterData model)
        {
            //AppConstant.Constants.UserIdInfo = model.UserEmail;
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new UserInfoView(model.UserId));
        }

        private async void ExecuteSelectGroupCommand(Item model)
        {
            AppConstant.Constants.ProductId = model.Id;

            string views = B8.DBLookupEx("ShowProducts", "ViewsCount + 1", "Product_Id", model.Id.ToString());

            B8.UpdateExpress("ShowProducts", "Product_Id", model.Id.ToString(), "ViewsCount", views);

            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new DetailPageView());
        }

        private async Task RefreshItems()
        {
            
            GetMyFavorites();

            IsRefreshing = false;
        }

        private async Task DelFav(Item model) 
        {
            Warnings wg = new Warnings();
            List<ProductModel> producto = new List<ProductModel>();
            ProductModel prd = new ProductModel();

            producto = ViewModelLocator.mongo.DetailProduct(model.Id, "Favorites");

            foreach (var item in producto)
            {
                prd.Latitud = item.Latitud;
                prd.Longitud = item.Longitud;
                prd.Categoria = item.Categoria;
                prd.Descripcion = item.Descripcion;
                prd.Direccion = item.Direccion;
                prd.Estado = item.Estado;
                prd.EstadoActual = item.EstadoActual;
                prd.IdProducto = item.Id.ToString();
                prd.Likes = item.Likes;
                prd.NombreProducto = item.NombreProducto;
                prd.Precio = item.Precio;
                prd.USerPic = item.USerPic;
                prd.User_id = item.User_id;
                prd.User_Name = item.User_Name;
                prd.ShortDesc = item.ShortDesc;
                prd.Foto1 = item.Foto1;
                prd.Foto2 = item.Foto2;
                prd.Foto3 = item.Foto3;
                prd.Foto4 = item.Foto4;
                prd.Id = item.Id;

            }

            if (ViewModelLocator.mongo.DeleteCollect(prd, "Favorites") == true)
            {
                    model.backgroundColor = Color.Black;
                    await wg.ToastSuccess("Producto eliminado a favoritos", myView);

                FavoriteProducts.Remove(model);

                   
            }
            else
            {
                  await wg.ToastWarning("UPS!! No hemos podido eliminar el producto", myView);
            }
            
        }

        private void ItemSelected(int selecction)
        {
            if(selecction == 1)
            {
                Title = "Perfiles Favoritos";
                UserInfo.Clear();
                GetMyFriends();
            }
            else
            {
                Title = "Artículos Favoritos";
                FavoriteProducts.Clear();
                GetMyFavorites();
            }
        }

    }
}
