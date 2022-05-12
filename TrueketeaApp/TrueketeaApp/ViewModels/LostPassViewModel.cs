using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Services;
using TrueketeaApp.Services.Messages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class LostPassViewModel : ViewModelBase
    {
        private Warnings wg = new Warnings();
        private B8Clases B8 = new B8Clases();
        private readonly EntryValidation validation = new EntryValidation();
        ContentPage myView;
        Entry email;

        private string _err;
        public string Error
        {
            get => _err;
            set => SetProperty(ref _err, value);
        }

        public LostPassViewModel(ContentPage view)
        {
            IsVisbleMail = false;
            
            myView = view;
            email = myView.FindByName<Entry>("EMAIL");
            email.Unfocused += EmailEntry_Unfocused;
           
            this.SendCommand = new Command(this.RecoveryPass);
        }


        private void EmailEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (validation.Email_validacion(email.Text) == false)
            {
                IsVisbleMail = true;
                Error = "El Email no es correcto.";
            }
        }

        public bool AreFieldsValid()
        {
            bool isEmailValid = validation.Verify_Email(email.Text);
            return isEmailValid;
        }

        public Command SendCommand { get; set; }


        private async void RecoveryPass(object obj)
        {
            IsBusy = true;
            if (this.AreFieldsValid())
            {
                EmailMessages msg = new EmailMessages(email.Text);

                if (msg.Send_Email(0) == true)
                {
                    B8.UpdateExpress("Users", "Email", email.Text, "Password", msg.Password);
                    IsBusy = false;
                    await wg.ToastSuccess("La nueva contraseña ha sido enviada por email.", myView);
                    await NavigationService.NavigateToAsync<LoginViewModel>();
                }
                else
                {
                    
                    await wg.ToastError("No se ha podido restablecer la contraseña por un error" + msg.EmailError, myView);
                    IsBusy = false;
                }
                
            }
            else
            {
                await wg.ToastError("Error al recuperar la contraseña", myView);
                IsBusy = false;
            }
        }

        
        
    }
}
