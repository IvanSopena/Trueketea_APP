using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;

namespace TrueketeaApp.Models
{
    public class MyContactsModel : ViewModelBase
    {

        public bool Connected { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public string LastConnection { get; set; }
        public string Estado { get; set; }


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
