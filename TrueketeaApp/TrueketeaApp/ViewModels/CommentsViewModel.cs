using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.Messages;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
   public class CommentsViewModel : ViewModelBase
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

        public Command SendCommand { get; }

        private string clave;

        private CommentsModel msg = new CommentsModel();

        public CommentsViewModel(ContentPage View, string userID)
        {
            MyView = View;
            clave =  userID;
            Note = "";
            Comment = "";
            MyVotes = B8.DBLookupEx("view_users", "Vote", "User_id", clave);
            ShowVotes();
            UserName = B8.DBLookupEx("view_users", "Name", "User_id", clave);
            UserPhoto = B8.DBLookupEx("view_users", "PhotoPath", "User_id", clave);
            LastConnection = "Última conexión: " + B8.DBLookupEx("view_users", "Last_Conection", "User_id", clave);
            USerEmail = B8.DBLookupEx("view_users", "Email", "User_id", userID);
            SendCommand = new Command(this.SendComment);
        }

        async private void SendComment(object obj)
        {

            if (Note == "" && Comment == "")
            {
                await wg.ToastWarning("Es necesario escribir un comentario o una calificación", MyView);
            }
            else
            {  
                if (Convert.ToInt32(Note) > 5)
                {
                    await wg.ToastWarning("La calificación debe ser maximo un 5.", MyView);
                }
                else
                {
                    msg.Foto = "";
                    msg.Emisor = AppConstant.Constants.UserLoginId;
                    msg.Receptor = clave;
                    msg.Votos = Note;
                    msg.msg = Comment;


                    if (ViewModelLocator.mongo.InsertCommet(msg) == true)
                    {
                        await wg.ToastSuccess("Comentario enviado con exito", MyView);
                    }
                    else
                    {
                        await wg.ToastWarning("El comentario no ha podido ser enviado.", MyView);
                    }

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
