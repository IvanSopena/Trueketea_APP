using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
   public class ValidarViewModel : ViewModelBase
    {
        private readonly ContentPage myView;
        private readonly Entry C1;
        private readonly Entry C2;
        private readonly Entry C3;
        private readonly Entry C4;
        private readonly Entry C5;
        private readonly Entry C6;
        private RegisterData reg = RegisterViewModel.reg;
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        private readonly B8Clases B8 = new B8Clases();
        private Warnings waning = new Warnings();
       

        public ValidarViewModel(ContentPage view)
        {
            IsVisbleMail = false;
            
            myView = view;
            C1 = myView.FindByName<Entry>("C1");
            C2 = myView.FindByName<Entry>("C2");
            C3 = myView.FindByName<Entry>("C3");
            C4 = myView.FindByName<Entry>("C4");
            C5 = myView.FindByName<Entry>("C5");
            C6 = myView.FindByName<Entry>("C6");

            this.ValidateCommand = new Command(this.ButtonClicked);

        }

        public Command ValidateCommand { get; set; }

        async private void ButtonClicked(object obj)
        {
            IsBusy = true;

            string code = B8.DBLookupEx(_dbContext.TableOwner + ".ValidationEmails", "Code", "Email", reg.UserEmail,"Result","0");
            string entrycode = C1.Text + C2.Text + C3.Text + C4.Text + C5.Text + C6.Text;

            if (code.Equals(entrycode))
            {
                if(B8.UpdateExpress("ValidationEmails", "Email", reg.UserEmail, "Result", "1")==true)
                {
                    int insert = 0;
                    string query = "";
                    string photo = "https://firebasestorage.googleapis.com/v0/b/trueketea-bd250.appspot.com/o/System%2Fno_photo.jpeg?alt=media&token=6a5466f5-7d02-4375-b4bf-c7472557daeb";
;
                    query = $"Insert into { _dbContext.TableOwner}.Login_Users (User_Id,Rol_Id,User_Name," +
                        $"User_Email,Password,Last_Conection,Device,Active,Photo)" +
                        $"Values ('{reg.UserId}','0','{reg.UserName}','{reg.UserEmail}','{reg.Password}',convert(datetime,'{reg.LastConnection}',103),'{reg.Device}','1','{photo}')";

                    if (0 != _dbContext.DbExecute(query, ref insert))
                    {
                        B8.Log_Error(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, _dbContext.DbLastError, query);
                        //await waning.Warning("Error en la validacion del email");
                        await waning.ToastWarning("Error en el sistema de validación.Intentalo mas tarde.", myView);
                    }
                    else
                    {


                        IsBusy = true;
                        AppConstant.Constants.UserLoginEmail = reg.UserEmail;
                        AppConstant.Constants.UserLoginId = reg.UserId;
                        AppConstant.Constants.UserDirection = "";
                        //AppConstant.Constants.UserName = B8.DBLookupEx(_dbContext.TableOwner + ".view_users", "Name", "Email", reg.UserEmail);
                        AppConstant.Constants.UserProfilePhoto = photo;
                        

                        ChageCommonsVariables();


                        query = $"Insert into { _dbContext.TableOwner}.ChatRelationship (User1,User2,Conversation_id," +
                       $"MsgType,MsgStatus,LastMsg)" +
                       $"Values ('{reg.UserId}','0','6279ee72d15a035968357a24','Text','New','Bienvenido')";

                        if (0 != _dbContext.DbExecute(query, ref insert))
                        {
                            B8.Log_Error(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, _dbContext.DbLastError, query);
                            //await waning.Warning("Error en la validacion del email");
                            //await waning.ToastWarning("Error en el sistema de validación.Intentalo mas tarde.", myView);
                        }


                        await NavigationService.NavigateToAsync<HomeViewModel>();

                        B8.UpdateExpress("Login_Users", "User_Email", reg.UserEmail, "Connected", "1");
                    }


                }
            }
            else
            {
                await waning.ToastWarning("Error en la validación, vuelve a introducir el codigo.", myView);
            }

            IsBusy = false;
        }
   }
}
