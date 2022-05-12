using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace TrueketeaApp.ViewModels
{
    public class DetailPageViewModel: ViewModelBase
    {
        
        public Command Fav_command { get; set; }
        public Command Chat_command { get; set; }

        public Command ViewUser_command { get; set; }
        public Command Vote_command { get; set; }
        public Command ShowOPtions_command { get; set; }
        public Command Share_command { get; set; }
        public Command ReportUser_command { get; set; }
        public Command ReportProduct_command { get; set; }

        public ObservableCollection<ImageItem> popularFashion { get; set; }
        private B8Clases B8 = new B8Clases();
        private Warnings wg = new Warnings();
        private readonly DBContext _dbContext = ViewModelLocator.sql;

        public string Photo { get; set; }
        public string id_user { get; set; }
        public double Longitud { get; set; }
        public double Latitud { get; set; }
        public string Precio { get; set; }
        public int _Likes;
        public int Likes
        {
            get => _Likes;
            set => SetProperty(ref _Likes, value);
        }
        public string NombreProducto { get; set; }
        public string User_Name { get; set; }
        public string Categoria { get; set; } 
        public string Descripcion { get; set; }
        public string ShortDesc { get; set; }
        public string Estado { get; set; }
        public string EstadoActual { get; set; }
        private string User_Email { get; set; }


        string _votos;

        public string Votos
        {
            get => _votos;
            set => SetProperty(ref _votos, value);
        }

        private List<ProductModel> producto = new List<ProductModel>();
     
        ContentPage myView;
        Xamarin.Forms.Maps.Map map;

        public DetailPageViewModel(ContentPage view) 
        {
            myView = view;

            map = myView.FindByName<Xamarin.Forms.Maps.Map>("LocationProduct");
            producto = ViewModelLocator.mongo.DetailProduct(AppConstant.Constants.ProductId, "Products");

            IsVisibleOptions = false;

            this.Fav_command = new Command(this.LikeProduct);
            this.Chat_command = new Command(this.OpenChat);
            this.Vote_command = new Command(this.VoteUser);
            this.ShowOPtions_command = new Command(this.ShoWMenu);
            this.Share_command = new Command(this.ShareOptions);
            this.ReportUser_command = new Command(this.ReportUser);
            this.ReportProduct_command = new Command(this.ReportProduct);
            this.ViewUser_command = new Command(this.ViewFriend);

            Photo = AppConstant.Constants.UserProfilePhoto;
            
            Carga_fotos();
            CargaDatos();
        }


        public void ShoWMenu (object obj)
        {
            if (IsVisibleOptions == true)
            {
                IsVisibleOptions = false;
            }
            else
            {
                IsVisibleOptions = true;
            }

        }

        public async void ShareOptions(object obj)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Trueketea",
                Text = "Estado compartido de un producto",
                Title = "Producto de Trueketea",
                Uri = "https://trueketea.es/article_Share"
            });

        }

        public void Carga_fotos()
        {
            string f1 = "";
            string f2 = "";
            string f3 = "";
            string f4 = "";
            int count = 0;

            foreach (var item in producto)
            {
                f1 = item.Foto1.ToString();
                f2 = item.Foto2.ToString();
                f3 = item.Foto3.ToString();
                f4 = item.Foto4.ToString();

            }

            if (f2 != "")
            {
                count = 2;
            }
            if (f3 != "")
            {
                count = 3;
            }
            if (f4 != "")
            {
                count = 4;
            }

            switch (count)
            {

                case 0:
                    popularFashion = new ObservableCollection<ImageItem>
                    {
                        new ImageItem { visible1=true, color1= "#0bbf2f",  visible2=false, color2= "#ffffff",  visible3=false, color3= "#ffffff",  visible4=false, color4= "#ffffff", image= ImageSource.FromUri(new Uri(f1)) }
                       
                    };
                    break;

                case 2:
                    popularFashion = new ObservableCollection<ImageItem>
                    {
                        new ImageItem { visible1=true, color1= "#0bbf2f",  visible2=true, color2= "#ffffff",  visible3=false, color3= "#ffffff",  visible4=false, color4= "#ffffff", image= ImageSource.FromUri(new Uri(f1)) },
                        new ImageItem { visible1=true, color1= "#ffffff",  visible2=true, color2= "#0bbf2f",  visible3=false, color3= "#ffffff",  visible4=false, color4= "#ffffff", image=ImageSource.FromUri(new Uri(f2)) }
                    };
                    break;

                case 3:
                    popularFashion = new ObservableCollection<ImageItem>
                    {
                        new ImageItem { visible1 = true, color1 = "#0bbf2f", visible2 = true, color2 = "#ffffff", visible3 = true, color3 = "#ffffff", visible4 = false, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f1)) },
                        new ImageItem { visible1 = true, color1 = "#ffffff", visible2 = true, color2 = "#0bbf2f", visible3 = true, color3 = "#ffffff", visible4 = false, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f2)) },
                        new ImageItem { visible1 = true, color1 = "#ffffff", visible2 = true, color2 = "#ffffff", visible3 = true, color3 = "#0bbf2f", visible4 = false, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f3)) }
                    };
                    break;
                case 4:
                    popularFashion = new ObservableCollection<ImageItem>
                    {
                        new ImageItem { visible1 = true, color1 = "#0bbf2f", visible2 = true, color2 = "#ffffff", visible3 = true, color3 = "#ffffff", visible4 = true, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f1)) },
                        new ImageItem { visible1 = true, color1 = "#ffffff", visible2 = true, color2 = "#0bbf2f", visible3 = true, color3 = "#ffffff", visible4 = true, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f2)) },
                        new ImageItem { visible1 = true, color1 = "#ffffff", visible2 = true, color2 = "#ffffff", visible3 = true, color3 = "#0bbf2f", visible4 = true, color4 = "#ffffff", image = ImageSource.FromUri(new Uri(f3)) },
                        new ImageItem { visible1 = true, color1 = "#ffffff", visible2 = true, color2 = "#ffffff", visible3 = true, color3 = "#ffffff", visible4 = true, color4 = "#0bbf2f", image = ImageSource.FromUri(new Uri(f3)) }
                    };
                    break;
            }
        }

        public void CargaDatos()
        {

            

            foreach (var item in producto)
            {
                Longitud = item.Longitud;
                Latitud = item.Latitud;
                Precio = "Tasado en " + item.Precio + "€";
                Likes = item.Likes;
                NombreProducto = item.NombreProducto;
                User_Name = item.User_Name;
                User_Email = B8.DBLookupEx("view_users", "Email", "User_id", item.User_id.ToString());
                Categoria = item.Categoria;
                Descripcion = item.Descripcion;
                ShortDesc = item.ShortDesc;
                Estado = item.Estado;
                EstadoActual = item.EstadoActual;
                id_user = item.User_id.ToString();

            }

            Votos = B8.DBLookupEx("Users","ISNULL(Vote,'0')","User_id",id_user);

            Pin pin_product = new Pin()
            {
                Type = PinType.Generic,
                Label = NombreProducto,
                Position = new Position(Latitud, Longitud)
            };

            map.Pins.Add(pin_product);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin_product.Position, Distance.FromMeters(500)));

        }

        public async void ViewFriend()
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new UserInfoView(User_Email));
        }

        public async void OpenChat(object obj)
        {
            bool st = false;
            string photoUser = Photo ;
            string usr = User_Name;
            string usr_id = id_user;

            if(B8.DBLookupEx("view_users", "IIF(Connected > 0, CAST(1 AS BIT), CAST(0 AS BIT))", "User_id",usr_id) == "1")
            {
                st = true;
            }

            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new ChatConversationView(photoUser, usr, st, usr_id));
        }

        public async void ReportUser(object obj)
        {
            string sql = "";
            string thisDay = DateTime.Today.ToString("dd/MM/yyyy");
            int afectRow = 0;
            string id = B8.DBLookupEx("Internal_Emails", "Max(id_Notify) + 1", "1", "1");

            sql = $"Insert into {_dbContext.TableOwner}.Internal_Emails (id_Notify,id_Emisor,id_Receptor,Message,status_id,Read_Email,SendDate,Subject) ";
            sql = sql + $" values('{id}','{AppConstant.Constants.UserLoginId}','1','Notificacion de denuncia del producto id {AppConstant.Constants.ProductId}'," +
                $"'1','0',convert(datetime,{thisDay},103) , 'Denuncia de Perfil') ";

            if (0 != _dbContext.DbExecute(sql, ref afectRow))
            {
                B8.Log_Error("LoginViewModel", "ExternalReg", _dbContext.DbLastError, sql);

                await wg.ToastWarning("Error al reportar el aviso.Intentalo mas tarde.", myView);
            }
            else
            {
                await wg.ToastInfo("Reporte enviado.", myView);
            }

        }

        public async void ReportProduct(object obj)
        {
            string sql = "";
            string thisDay = DateTime.Today.ToString("dd/MM/yyyy");
            int afectRow = 0;
            string id = B8.DBLookupEx("Internal_Emails", "Max(id_Notify) + 1", "1", "1");

            sql = $"Insert into {_dbContext.TableOwner}.Internal_Emails (id_Notify,id_Emisor,id_Receptor,Message,status_id,Read_Email,SendDate,Subject) ";
            sql = sql + $" values('{id}','{AppConstant.Constants.UserLoginId}','1','Notificacion de denuncia del producto id {AppConstant.Constants.ProductId}'," +
                $"'1','0',convert(datetime,{thisDay},103), 'Denuncia de Producto' )";

            if (0 != _dbContext.DbExecute(sql, ref afectRow))
            {
                B8.Log_Error("LoginViewModel", "ExternalReg", _dbContext.DbLastError, sql);

                await wg.ToastWarning("Error al reportar el aviso.Intentalo mas tarde.", myView);
            }
            else
            {
                await wg.ToastInfo("Reporte enviado.", myView);
            }

        }


        public void VoteUser(object obj)
        {
            int i = Convert.ToInt32(Votos) + 1;
            Votos = i.ToString();
            B8.UpdateExpress("Users","User_id",id_user,"Vote", Votos);
        }

        public void LikeProduct(object obj)
        {
            ProductModel prd = new ProductModel();
            Likes = Likes + 1 ;

            foreach(var item in producto)
            {
                prd.Latitud = item.Latitud;
                prd.Longitud = item.Longitud;
                prd.Categoria = item.Categoria;
                prd.Descripcion = item.Descripcion;
                prd.Direccion = item.Direccion;
                prd.Estado = item.Estado;
                prd.EstadoActual = item.EstadoActual;
                prd.IdProducto = item.Id.ToString();
                prd.Likes = Likes;
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

            ViewModelLocator.mongo.UpdateProduct(prd);
        }

    }
}
