using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrueketeaApp.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        //Implemetacion del interfaz
        public Task NavigateToAsync<TViewModel>()
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        //Navegamos internamente desde un viewmodel a otro viewmodel
        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreatePage(viewModelType, parameter);
            var navigationPage = Application.Current.MainPage as NavigationPage; 

            if (navigationPage != null)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(page);
            }
        }


        //Metodo para crear la instacia de la pagina a la que vamos a navegar.
        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"No se ha podido localizar la pagina {viewModelType}");
            }
            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }


        //Obtener el nombre de la pagina a partir del ViewModel que pasamos
        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName =
                viewModelType.FullName.Replace("Model", string.Empty);

            
            var viewModelAssemblyName =
                viewModelType.GetTypeInfo().Assembly.FullName;

            var viewAssemblyName =
                string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);

            var viewType = Type.GetType(viewAssemblyName);
            return viewType;

        }
    }
}
