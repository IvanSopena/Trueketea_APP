using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrueketeaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ValidarView : ContentPage
    {
        public ValidarView()
        {
            InitializeComponent();
            this.BindingContext = new ValidarViewModel(this);
        }
    }
}