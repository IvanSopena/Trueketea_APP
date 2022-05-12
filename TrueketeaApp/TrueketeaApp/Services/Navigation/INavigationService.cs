using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TrueketeaApp.Services.Navigation
{

   

    public interface INavigationService
    {

        //Metodo que nos va a permitir realizar la navegacion entre Views Models
        Task NavigateToAsync<TViewModel>();

    }
}
