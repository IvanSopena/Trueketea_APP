using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrueketeaApp.Views.customviews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotpView : ContentPage
    {
        public PhotpView()
        {
            InitializeComponent();
            TakePhoto.Clicked += TakePhoto_Clicked; 
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            var photo =
                await Plugin.Media.CrossMedia.Current
                .TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());

            if(photo != null)
            {
                Photo.Source = ImageSource.FromStream(() =>
                {
                    return photo.GetStream();
                });
            }
        }
    }
}