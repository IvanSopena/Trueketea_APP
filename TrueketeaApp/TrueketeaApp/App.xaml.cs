using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TrueketeaApp;
using TrueketeaApp.Views;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.ViewModels;
using Syncfusion.Licensing;
using TrueketeaApp.Services;
using Xamarin.Essentials;
using TrueketeaApp.Services.Messages;

namespace TrueketeaApp
{
    public partial class App : Application
    {
        DBContext sql = ViewModelLocator.sql;
        B8Clases B8 = new B8Clases();
        Warnings wg = new Warnings();
        public App()
        { 
            //Register Syncfusion license
            SyncfusionLicenseProvider.RegisterLicense("NTc1NDg2QDMxMzkyZTM0MmUzMGJ4Q2VKOFFSam9IMGtzbDFEU3QrYmNCVnJMckptYjcwL05vWDF4VmdNYWc9");

            InitializeComponent();
            MainPage = new NavigationPage(new BienvenidaView()); //




        }

        protected override void OnStart()
        {

            sql.Conexion("tkadmin", "DoVbypml+NkLNHWt4DR+AQ==", "192.168.1.51", "Trueketea"); //"192.168.142.128" 172.16.47.128


        }

        protected override void OnSleep()
        {
            //Cuando la app se va a segundo plano.

            //No hay ningún método para 
            //    la finalización de aplicaciones.
            //    En circunstancias normales(es decir, si no se produce un bloqueo), 
            //    la finalización de la aplicación se producirá 
            //    desde el estado OnSleep, sin que se muestren notificaciones en el código.

            string thisDay = DateTime.Today.ToString("dd/MM/yyyy");
            string user = AppConstant.Constants.UserLoginEmail;
            var currentVersion = VersionTracking.CurrentVersion;
            B8.UpdateExpress("Login_Users", "User_Email", user, "Last_Conection", "convert(varchar(30),'{thisDay}')");
            B8.UpdateExpress("Login_Users", "User_Email", user, "AppVersion", currentVersion);
            B8.UpdateExpress("Login_Users", "User_Email", user, "Connected", "0");
            sql.DbClose();
        }

        protected override void OnResume()
        {
            //Se llamacuando la app vuelve de segundo plano
            if (sql.IsOpen == false)
            {
                sql.Conexion("tkadmin", "DoVbypml+NkLNHWt4DR+AQ==", "192.168.1.51", "Trueketea");
            }
            else
            {
                string user = AppConstant.Constants.UserLoginEmail;
                B8.UpdateExpress("Login_Users", "User_Email", user, "Connected", "1");
            }
        }

       


    }
}
