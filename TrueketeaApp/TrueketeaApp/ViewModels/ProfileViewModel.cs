using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TrueketeaAdmin.Services.Security;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Services;
using TrueketeaApp.Services.Firestore;
using TrueketeaApp.Services.Messages;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        B8Clases B8 = new B8Clases();
        ContentPage MyView;
        Warnings wg = new Warnings();
        public Command ReturnCommand { get; }
        EntryValidation validation = new EntryValidation();
        private readonly Encrypting Encrypt = new Encrypting();
        private readonly FBStorage storage = new FBStorage();
        public Command AddPhotoCommand { get; }
        public Command DeleteCommand { get; }
        public Command ModifyCommand { get; }

       
        public ObservableCollection<ProvincesModel> Provinces { get; private set; }
        public ObservableCollection<RegisterData> UserInfo { get; private set; }
        private string PahtNewPhoto = "";

       

        string _Selectgender;
        public string SelectGender
        {
            get => _Selectgender;
            set => SetProperty(ref _Selectgender, value);
        }


        ProvincesModel _SelectProv;
        public ProvincesModel SelectProv
        {
            get => _SelectProv;
            set => SetProperty(ref _SelectProv, value);
        }

        public ProfileViewModel(ContentPage view)
        {
            MyError = "";
            IsVisibleName = false;
            IsVisbleMail = false;
            IsVisblePass = false;
            IsVisibleProvince = false;
            IsVisibleAppUser = false;
            
            LastConnection = "Última entrada: " + B8.DBLookupEx("view_users", "CONVERT(varchar(30),Last_Conection, 103)", "Email", ViewModelLocator.MyEmail);
            UserPhoto = ViewModelLocator.MyPhoto;
            UserName = ViewModelLocator.MyName;
            MyView = view;
            ReturnCommand = new Command(ReturnWindow);
            AddPhotoCommand = new Command(this.AddPhoto);
            ModifyCommand = new Command(this.UpdateUser);
            DeleteCommand = new Command(this.DeleteCount);
            GetProvinces();
            GetUserData();
        }

        public async void ReturnWindow(object obj)
        {
            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PopAsync();
        }



        private async void AddPhoto()
        {

            var camera_options = new Plugin.Media.Abstractions.StoreCameraMediaOptions();
            
            var photo =
                await Plugin.Media.CrossMedia.Current
                .TakePhotoAsync(camera_options);

            if (photo != null)
            {
                PahtNewPhoto = photo.Path;
                UserPhoto = photo.Path;
            }

        }

        private void GetProvinces()
        {
            Provinces = new ObservableCollection<ProvincesModel>(DataService.GetAllProvinces());
        }


        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Province { get; set; }

        public string AppUser { get; set; }

        public string Gender { get; set; }


       


        private void GetUserData()
        {
            
            UserInfo = new ObservableCollection<RegisterData>(DataService.GetDataUser(ViewModelLocator.MyEmail));

            foreach(var item in UserInfo)
            {
                Name = item.UserName;
                Email = item.UserEmail;
                Password = Encrypt.Decrypt(item.Password, Email);
                Province = item.Province;
                AppUser = item.AppUser;
                Gender = item.Gender;
                //SelectProv.Desc = item.Province;
               // SelectGender = item.Gender;
            }

        }

        private async void UpdateUser()
        {
            string sql = "";
            string name = B8.GetItemList(Name, 1, " ");
            string surname = B8.GetItemList(Name, 2, " ");
            string pass = Encrypt.Encrypt(Password,Email);
            string code = "";

         
            code = B8.DBLookupEx("SpainLocation", "Id_Province", "Province_Name", Province);
          

            if (validation.CheckPass(Password) == false)
            {
                IsVisblePass = true;
                MyError = validation.ValidationError;
                return;
            }

            if (string.IsNullOrEmpty(Name))
            {
                MyError = "El campo Nombre es obligatorio.";
                IsVisibleName = true;
                return;
            }

            if (string.IsNullOrEmpty(code))
            {
                MyError = "El campo Provincia es obligatorio.";
                IsVisibleProvince = true;
                return;
            }

            if (string.IsNullOrEmpty(AppUser))
            {
                MyError = "El campo AppUser es obligatorio.";
                IsVisibleAppUser = true;
                return;
            }

            sql = $"Update Users set Name = '{name}',Surname = '{name}',Password='{pass}',AppUser='{AppUser}',Location='{code}'";

            if (string.IsNullOrEmpty(PahtNewPhoto) == false)
            {
                PahtNewPhoto = await storage.Addphoto(PahtNewPhoto.Replace("File: /", ""), AppConstant.Constants.UserLoginId, "Profile", "New_Profile.png");
                AppConstant.Constants.UserProfilePhoto = PahtNewPhoto;
                sql = sql + $" ,PhotoPath='{PahtNewPhoto}'";
            }

            if (string.IsNullOrEmpty(SelectGender) == false)
            {
                string gender;
                if (SelectGender.Equals("Homber"))
                {
                    gender = "1";
                }
                else
                {
                    gender = "0";
                }
                sql = sql + $" ,Gender = '{gender}'";
            }

            sql = sql + $" Where Email = '{Email}'";

            int affectedRow = 0;

            if (0 != ViewModelLocator.sql.DbExecute(sql, ref affectedRow))
            {
                await wg.ToastWarning("Error al actualizar el perfil", MyView);
                B8.Log_Error("UpdateUser", "ProfileViewModel", ViewModelLocator.sql.ClsLastError, sql);
            }
            else
            {
                await wg.ToastSuccess("Perfil actualizado.", MyView);
            }

        }

        private async void DeleteCount()
        {
            bool respuesta = await wg.AlertYesNoClicked("¿Estas seguro que quieres eliminar tu cuenta?");

            if (respuesta == true)
            {
                B8.UpdateExpress("Login_Users", "User_Email", ViewModelLocator.MyEmail, "Active", "0");

                await NavigationService.NavigateToAsync<LoginViewModel>();
                await wg.ToastInfo("Cuenta Eliminada", MyView);
            }

        }

    }
}
