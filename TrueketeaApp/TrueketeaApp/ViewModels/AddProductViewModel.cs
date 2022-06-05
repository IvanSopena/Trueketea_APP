using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Firestore;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views.customviews;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class AddProductViewModel : ViewModelBase
    {
        
        private readonly FBStorage storage = new FBStorage();
        public Command AddPhotoCommand { get; }

        public Command AddCommand { get; }
        public Command PriceCommand { get; }
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        public ObservableCollection<PriceModel> Tasacion { get; private set; }
        public ObservableCollection<ProductStatus> Status { get; private set; }
        public ObservableCollection<Group> Groups { get; private set; }

        int fotos = 3;

        string price = "0";
        bool f1_change = false;
        bool f2_change = false;
        bool f3_change = false;
        bool f4_change = false;

        ContentPage myView;
        Image F1;
        Image F2;
        Image F3;
        Image F4;

        Group _selectCat;
        public Group SelectCat
        {
            get => _selectCat;
            set => SetProperty(ref _selectCat, value);
        }

        ProductStatus _selectStatus;
        public ProductStatus SelectStatus
        {
            get => _selectStatus;
            set => SetProperty(ref _selectStatus, value);
        }

        string _productName;
        public string ProductName
        {
            get => _productName;
            set => SetProperty(ref _productName, value);
        }


        public AddProductViewModel(ContentPage view)
        {
            myView = view;
            IsVisbleCat = false;
            this.GetCategories();
            this.GetStatus();
            this.AddPhotoCommand = new Command(this.AddPhoto);
            this.PriceCommand = new Command(this.PriceProduct);
            this.AddCommand = new Command(this.AddProduct);

        }

        private void GetCategories()
        {

            Groups = new ObservableCollection<Group>(DataService.GetGroups());

        }

        private void GetStatus()
        {

            Status = new ObservableCollection<ProductStatus>(DataService.GetProductStatus());
            

        }


        private async void AddPhoto()
        {

            var camera_options = new Plugin.Media.Abstractions.StoreCameraMediaOptions();
            //camera_options.SaveToAlbum = true;

            var photo =
                await Plugin.Media.CrossMedia.Current
                .TakePhotoAsync(camera_options);

            if (photo != null)
            {
                if (fotos > -1)
                {
                    
                    switch (fotos)
                    {
                        case 1:
                            F3 = myView.FindByName<Image>("Foto3");
                            F3.Source = photo.Path;
                            f3_change = true;
                            break;

                        case 2:
                            F2 = myView.FindByName<Image>("Foto2");
                            F2.Source = photo.Path;
                            f2_change = true;
                            break;

                        case 3:
                            F1 = myView.FindByName<Image>("Foto1");
                            F1.Source = photo.Path;
                            f1_change = true;
                            break;
                        case 0:
                            F4 = myView.FindByName<Image>("Foto4");
                            F4.Source = photo.Path;
                            f4_change = true;
                            break;
                    }

                 

                    fotos = fotos - 1;
                }
                else
                {
                    Warnings wg = new Warnings();
                    await wg.ToastInfo("Solo se admiten hasta 4 fotos", myView);
                }

            }


        }


        private async void PriceProduct()
        {
            Warnings wg = new Warnings();
            B8Clases B8 = new B8Clases();
            
            try
            {

                if(ProductName.Equals(""))
                {
                    await wg.ToastWarning("Debe poner un nombre de producto antes de realizar la tasación.", myView);
                }
                else
                {
                    string product = ProductName;
                    string cat = "" + SelectCat.description;
                    cat = B8.DBLookupEx("Categories", "Id_Categorie", "Categorie_Desc", cat);
                    price = B8.DBLookupEx("ProductPrice", "Max(Price)", "Categorie_id", cat, $"Product_Name like '%{product}%' and '1'", "1");

                    await wg.ToastInfo($"Producto tasado en {price}€", myView);

                }  

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                await wg.ToastWarning("Debe seleccionar la categoria antes de realizar la tasación.", myView);
            }


        }

        private async void AddProduct()
        {
            IsBusy = true;
            Warnings wg = new Warnings();
            B8Clases B8 = new B8Clases();
            EntryValidation validation = new EntryValidation();
            ProductModel prd = new ProductModel();

            try
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromMinutes(1)));

                if (validation.Product_Validation(SelectCat.description, myView.FindByName<Entry>("Title").Text, myView.FindByName<Entry>("Desc").Text, SelectStatus.StatusName) == false)
                {
                    await wg.ToastWarning("Funcionalidad en Desarrollo", myView);
                }
                else
                {
                    if (location == null)
                    {
                        prd.Latitud = 0;
                        prd.Latitud = 0;
                    }
                    else
                    {
                        prd.Latitud = location.Longitude;
                        prd.Latitud = location.Latitude;
                    }

                    prd.Categoria = SelectCat.description;
                    prd.Descripcion = myView.FindByName<Entry>("DescLarge").Text;
                    prd.Direccion = AppConstant.Constants.UserDirection;
                    prd.Estado = "Subido";
                    prd.EstadoActual = SelectStatus.StatusName;
                    prd.IdProducto = "0";
                    prd.Likes = 0;

                    if (string.IsNullOrEmpty(ProductName))
                    {
                        await wg.ToastWarning("Es necesario que proporcione el nombre del producto.", myView);
                    }
                    else
                    {
                        prd.NombreProducto = ProductName;
                    }

                    if(price == "0")
                    {
                        PriceProduct();
                        prd.Precio = Convert.ToDecimal(price);
                    }
                    else
                    {
                        prd.Precio = Convert.ToDecimal(price); 
                    }
                    prd.USerPic = AppConstant.Constants.UserProfilePhoto;
                    prd.User_id = Convert.ToInt32(AppConstant.Constants.UserLoginId);
                    prd.User_Name = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", AppConstant.Constants.UserLoginEmail);
                    prd.ShortDesc = myView.FindByName<Entry>("Desc").Text;

                    if(f1_change == true)
                    {
                        prd.Foto1 = await storage.Addphoto(F1.Source.ToString().Replace("File: /", ""), AppConstant.Constants.UserLoginId, prd.NombreProducto, prd.NombreProducto +"_1.png");
                    }

                    if (f2_change == true)
                    {
                        prd.Foto2 = await storage.Addphoto(F2.Source.ToString().Replace("File: /", ""), AppConstant.Constants.UserLoginId, prd.NombreProducto, prd.NombreProducto + "_2.png");
                    }
                    else
                    {
                        prd.Foto2 = "";
                    }

                    if (f3_change == true)
                    {
                        prd.Foto3 = await storage.Addphoto(F3.Source.ToString().Replace("File: /", ""), AppConstant.Constants.UserLoginId, prd.NombreProducto, prd.NombreProducto + "_3.png");
                    }
                    else
                    {
                        prd.Foto3 = "";
                    }

                    if (f4_change == true)
                    {
                        prd.Foto4 = await storage.Addphoto(F4.Source.ToString().Replace("File: /", ""), AppConstant.Constants.UserLoginId, prd.NombreProducto, prd.NombreProducto + "_4.png");
                    }
                    else
                    {
                        prd.Foto4 = "";
                    }

                    if (ViewModelLocator.mongo.InsertProduct(prd, "Products") == true)
                    {

                        B8.AddHistory(prd.Id.ToString(), prd.NombreProducto, prd.User_id.ToString(), "1", SelectCat.id);
                        await wg.ToastSuccess("Producto Añadido con exito", myView);
                    }
                    else
                    {
                        await wg.ToastWarning("El producto no ha podido ser añadido.", myView);
                    }
                }

            }
            catch(Exception ex)
            {
                string msg = ex.Message.ToString(); 
                
                await wg.ToastError(msg, myView);
                B8.Log_Error("AddProduct", "AddProductViewModel", msg);
            }



            IsBusy = false;
        }

    }
}
