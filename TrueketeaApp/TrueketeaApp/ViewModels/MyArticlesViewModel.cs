using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class MyArticlesViewModel : ViewModelBase
    {
        public Command RefreshCommand { private set; get; }

        public Command SelectGroupCommand { get; }
        public ObservableCollection<Item> MyProducts { get; private set; }

        public MyArticlesViewModel()
        {
            SelectGroupCommand = new Command<Item>((model) => ExecuteSelectGroupCommand(model));
            RefreshCommand = new Command(async () => await RefreshItems());
            GetMyProducts();
        }

        void GetMyProducts()
        {
            MyProducts = new ObservableCollection<Item>(DataService.GetUserItems(AppConstant.Constants.UserLoginId, "Products"));

        }

        private async Task RefreshItems()
        {

            GetMyProducts();

            IsRefreshing = false;
        }

        private async void ExecuteSelectGroupCommand(Item model)
        {
            AppConstant.Constants.ProductId = model.Id;
            // await NavigationService.NavigateToAsync<DetailPageViewModel>();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new DetailPageView());
        }
    }
}
