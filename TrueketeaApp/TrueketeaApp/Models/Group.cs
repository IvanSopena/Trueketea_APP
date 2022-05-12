using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;


namespace TrueketeaApp.Models
{
    public class Group : ViewModelBase
    {
        public string description { get; set; }
        public string image { get; set; }
        public string id { get; set; }

        private bool _selected;
        public bool selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        private Color _backgroundColor;
        public Color backgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}
