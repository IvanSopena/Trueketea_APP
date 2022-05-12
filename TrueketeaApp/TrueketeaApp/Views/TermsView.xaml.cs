
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrueketeaApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsView : ContentPage
    {
        public TermsView()
        {
            InitializeComponent();
            BindingContext = new ViewModelBase();
        }
    }
}