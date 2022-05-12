using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class TruequeViewModel : ViewModelBase
    {
        ContentPage myView;
        public Command RefreshCommand { private set; get; }
        public Command SelectGroupCommand { get; }
        public Command DeleteFavCommand { get; }
        public ObservableCollection<TruequeModel> MyTrueques { get; private set; }


        public TruequeViewModel(ContentPage view)
        {
            Title = "Mis Trueques";
            myView = view;
            SelectGroupCommand = new Command<TruequeModel>((model) => ExecuteSelectGroupCommand(model));
            DeleteFavCommand = new Command<TruequeModel>(async (model) => await DelFav(model));
            RefreshCommand = new Command(async () => await RefreshItems());
            GetMyProducts();
            AppConstant.Constants.ActualView = "trueques";
        }

        void GetMyProducts()
        {
            MyTrueques = new ObservableCollection<TruequeModel>(DataService.GetTrueques(AppConstant.Constants.UserLoginId, "Trueques"));
        }


        private async Task DelFav(TruequeModel model)
        {
            Warnings wg = new Warnings();
            

            if (ViewModelLocator.mongo.DeleteTrueque(model) == true)
            {
                
                await wg.ToastSuccess("Producto eliminado correctamente.", myView);

                MyTrueques.Remove(model);


            }
            else
            {
                await wg.ToastWarning("UPS!! No hemos podido eliminar el producto", myView);
            }

        }

        private async Task RefreshItems()
        {

            GetMyProducts();

            IsRefreshing = false;
        }

        private async void ExecuteSelectGroupCommand(TruequeModel model)
        {
            AppConstant.Constants.ProductId = ObjectId.Parse(model.IdProducto);
           
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new DetailPageView());
        }
    }
}
