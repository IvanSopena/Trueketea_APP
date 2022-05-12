using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;

namespace TrueketeaApp.Services.Messages
{
    public class Warnings
    {


        public async Task Information(string texto)
        {
            await Application.Current.MainPage.DisplayAlert("Información", texto, "Ok");
        }

        public async Task Warning(string texto)
        {
            await Application.Current.MainPage.DisplayAlert("Error", texto, "Aceptar");
        }

        public async Task <bool> AlertYesNoClicked(string texto)
        {
            return await Application.Current.MainPage.DisplayAlert("Trueketea", texto, "Si", "No");
            
        }

        public async Task ToastInfo(string texto, ContentPage view)
        {

            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.Black,
                    Message = texto,
                    Font = Font.SystemFontOfSize(16),
                    Padding = new Thickness(10, 20, 20, 40)
                },
                BackgroundColor = Color.FromHex("#567cd7"),
                CornerRadius = new Thickness(20),
                Duration = TimeSpan.FromSeconds(5)
            };

            await view.DisplaySnackBarAsync(options);
        }

        public async Task ToastError(string texto, ContentPage view)
        {

            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = texto,
                    Font = Font.SystemFontOfSize(16),
                    Padding = new Thickness(10, 20, 30, 40)
                },
                BackgroundColor = Color.FromHex("#dc4e41"),
                CornerRadius = new Thickness(20),
                Duration = TimeSpan.FromSeconds(5)
            };

            await view.DisplaySnackBarAsync(options);
        }

        public async Task ToastWarning(string texto, ContentPage view)
        {

            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = texto,
                    Font = Font.SystemFontOfSize(16),
                    Padding = new Thickness(10, 20, 30, 40)

                },
                BackgroundColor = Color.FromHex("#fcbc0f"),
                CornerRadius = new Thickness(20),

                Duration = TimeSpan.FromSeconds(5)
            };

            await view.DisplaySnackBarAsync(options);
        }

        public async Task ToastSuccess(string texto, ContentPage view)
        {

            var options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = texto,
                    Font = Font.SystemFontOfSize(16),
                    Padding = new Thickness(10, 20, 30, 40)
                },
                BackgroundColor = Color.FromHex("#35c659"),
                CornerRadius = new Thickness(20),
                Duration = TimeSpan.FromSeconds(5)
            };

            await view.DisplaySnackBarAsync(options);
        }

    }
}
