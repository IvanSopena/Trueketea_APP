using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrueketeaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : TabbedPage
    {
        public HomeView()
        {
            InitializeComponent();
           
            UnselectedTabColor = Color.Black;
            SelectedTabColor = Color.FromHex("#0bbf2f");
           
        }

       
    }
}