using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    class ProfileFavViewModel : ViewModelBase
    {

        public Command Nav { get; }
        public ProfileFavViewModel()
        {
            Nav = new Command(async () => await ShowFavorites());
        }

        private async Task ShowFavorites()
        {
            await NavigationService.NavigateToAsync<FavViewModel>();
        }



    }
}
