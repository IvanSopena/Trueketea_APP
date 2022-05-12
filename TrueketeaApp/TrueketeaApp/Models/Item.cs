using MongoDB.Bson;
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;

namespace TrueketeaApp.Models
{
    public class Item : ViewModelBase
    {
        public ObjectId Id { get; set; }
        public string description { get; set; }
        public string complement { get; set; }
        public string category { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public bool favorite { get; set; }
        public ImageSource image { get; set; }
        public double rating { get; set; }

        private Color _backgroundColor;
        public Color backgroundColor
        {
            get { return _backgroundColor; }
            set { SetProperty(ref _backgroundColor, value); }
        }
    }
}
