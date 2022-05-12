using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Views;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using TrueketeaApp.Services.Messages;
using Plugin.LocalNotifications;

namespace TrueketeaApp.ViewModels
{
    public class MainViewModel : ViewModelBase 
    {
        private NotiffyModel updNoty = new NotiffyModel();
        private string catname = "Todos";
        public Command NavigateToDetailPageCommand { get; }
        public Command SelectGroupCommand { get; }
        public Command AddFavoriteCommand { get; }
        public Command RefreshCommand { private set; get; }
        public Command AddProductCommand { get; set; }
        public ObservableCollection<Group> Groups { get; private set; }
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> MyProducts { get; private set; }
        private const int SecondsToNotify = 10;
        private ObservableCollection<NotiffyModel> notifications = new ObservableCollection<NotiffyModel>();

        ContentPage myView;

        public MainViewModel(ContentPage view)
        {
            myView = view;
            AddFavoriteCommand = new Command<Item>(async (model) => await AddFav(model));
            NavigateToDetailPageCommand = new Command<Item>(async (model) => await ExecuteNavigateToDetailPageCommand(model));
            SelectGroupCommand = new Command<Group>((model) => ExecuteSelectGroupCommand(model));
            RefreshCommand = new Command(async () => await RefreshItems());
            AddProductCommand = new Command(this.AddProduct);
            GetGroups();
            GetItems();


            Device.StartTimer(new TimeSpan(0, 0, 20), () =>
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    MyNotifications = new ObservableCollection<NotiffyModel>(DataService.GetNotiffies());

                    foreach(var noty in MyNotifications)
                    {
                        if(noty.Receptor.Equals("all") || noty.Receptor.Equals(AppConstant.Constants.UserLoginId))
                        {

                            if(noty.Tipo == 1 && AppConstant.Constants.ActualView.Equals(""))
                            {
                                CrossLocalNotifications.Current.Show("Trueketea", noty.Texto, 0, DateTime.Now.AddSeconds(SecondsToNotify));
                            }
                            if(noty.Tipo==0)
                            {
                                CrossLocalNotifications.Current.Show("Trueketea", noty.Texto, 0, DateTime.Now.AddSeconds(SecondsToNotify));
                            }

                            updNoty.Id = noty.Id;
                            updNoty.Receptor = noty.Receptor;
                            updNoty.Tipo = noty.Tipo;
                            updNoty.Texto = noty.Texto;
                            updNoty.Estado = 1;
                            updNoty.IdProducto = noty.IdProducto;
                            updNoty.Chat = noty.Chat;

                            ViewModelLocator.mongo.UpdateNotification(updNoty);

                        }
                    }

                });
                return true; // runs again, or false to stop
            });


        }

        public ObservableCollection<NotiffyModel> MyNotifications
        {
            get
            {
                return this.notifications;
            }

            set
            {
                this.SetProperty(ref this.notifications, value);
            }
        }


        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        public static readonly BindableProperty FilterTextProperty = BindableProperty.Create(
           nameof(FilterText),
           typeof(string),
           typeof(MainViewModel),
           default(string),
           propertyChanged: OnFilterTextChanged);


        private static void OnFilterTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var page = (MainViewModel)bindable;
            string filterText = (string)newvalue;

            if (string.IsNullOrEmpty(filterText))
            {
                page.FilteredItems = page.Items;
            }
            else
            {
                page.FilteredItems = page.Items.Where(item =>
                {
                    return item.description.Split().Any(substring => substring.StartsWith(filterText, StringComparison.CurrentCultureIgnoreCase));
                });
            }
        }

        public static readonly BindableProperty FilteredItemsProperty = BindableProperty.Create(
            nameof(FilteredItems),
            typeof(IEnumerable<Item>),
            typeof(MainViewModel),
            default(IEnumerable<Item>));

        public IEnumerable<Item> FilteredItems
        {
            get => (IEnumerable<Item>)GetValue(FilteredItemsProperty);
            set => SetValue(FilteredItemsProperty, value);
        }

        async private void AddProduct(object obj)
        {
           
            await NavigationService.NavigateToAsync<AddProductViewModel>();
            
        }
        void GetGroups()
        {
            Groups = new ObservableCollection<Group>(DataService.GetGroups());
        }

        public void GetItems()
        {

            Items = new ObservableCollection<Item>(DataService.GetItems());
            FilteredItems = Items;
        }

        private async Task ExecuteNavigateToDetailPageCommand(Item model)
        {
          
            AppConstant.Constants.ProductId = model.Id;
            await NavigationService.NavigateToAsync<DetailPageViewModel>();
        }

        private async Task RefreshItems()  
        {
            
            Items.Clear();
            if (catname.Equals("Todos"))
            {
                Items = new ObservableCollection<Item>(DataService.GetItems());
            }
            else
            {
                Items = new ObservableCollection<Item>(DataService.GetItemsCategory(catname));
            }

            FilteredItems = Items;
            IsRefreshing = false;
        }

       
        private void ExecuteSelectGroupCommand(Group model)
        {
            var index = Groups
                .ToList()
                .FindIndex(p => p.description == model.description);

            if (index > -1)
            {
                UnselectGroupItems();

                Groups[index].selected = true;
                Groups[index].backgroundColor = Color.FromHex("#0bbf2f");
                catname = model.description;
                RefreshItems();
            }
        }

        void UnselectGroupItems()
        {
            Groups.ForEach((item) =>
            {
                item.selected = false;
                item.backgroundColor = Color.FromHex("#F2F3F4");
            });

        }

        private async Task AddFav(Item model)
        {
            Warnings wg = new Warnings();
            List<ProductModel> producto = new List<ProductModel>();
            ProductModel prd = new ProductModel();

            if (model.backgroundColor == Color.Red)
            {
                await wg.ToastInfo("Este producto ya esta en u lista de favoritos", myView);
            }
            else
            {
                producto = ViewModelLocator.mongo.DetailProduct(model.Id, "Products");

                foreach (var item in producto)
                {
                    prd.Id = item.Id;
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
                    prd.Fav_User_Id = Convert.ToInt32(AppConstant.Constants.UserLoginId);

                }

                if (ViewModelLocator.mongo.InsertProduct(prd, "Favorites") == true)
                {
                    model.backgroundColor = Color.Red;
                    await wg.ToastSuccess("Producto agrgado a favoritos", myView);
                }
                else
                {
                    await wg.ToastWarning("UPS!! No hemos podido agregarlo a favoritos", myView);
                }
            }
        }

    }
}
