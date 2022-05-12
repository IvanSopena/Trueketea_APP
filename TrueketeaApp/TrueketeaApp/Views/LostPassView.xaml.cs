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
    public partial class LostPassView : ContentPage
    {
        public LostPassView()
        {
            InitializeComponent();
            this.BindingContext = new LostPassViewModel(this);
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromRgb(11, 191, 47);
        }
    }
}