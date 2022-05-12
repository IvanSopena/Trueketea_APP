using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.PageModels.Base;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using TrueketeaApp.ViewModels;
using TrueketeaApp.AuthHelpers;

namespace TrueketeaApp.Views.customviews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToolBar : ContentView
    {
        
        public ToolBar()         
        {
            InitializeComponent();
            
        }

        public async void ShareOptions(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Subject = "Trueketea",
                Text = "Estado compartido de un producto",
                Title = "Producto de Trueketea",
                Uri = "https://trueketea.es/article_Share"
            });
        }
        public void OpenMenu(object sender, EventArgs e)
        {

           


        }

        public async void NavigateToBack(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        public bool ShareButtonIsVisible
        {
            get => (bool)GetValue(ShareButtonIsVisibleProperty);
            set => SetValue(ShareButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty ShareButtonIsVisibleProperty = BindableProperty.Create(

                nameof(ShareButtonIsVisible),
                typeof(bool),
                typeof(ToolBar),
                default(bool),
                defaultBindingMode: BindingMode.OneWay

            );

        public bool OptionsButtonIsVisible
        {
            get => (bool)GetValue(OptionsButtonIsVisibleProperty);
            set => SetValue(OptionsButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty OptionsButtonIsVisibleProperty = BindableProperty.Create(

                nameof(OptionsButtonIsVisible),
                typeof(bool),
                typeof(ToolBar),
                default(bool),
                defaultBindingMode: BindingMode.OneWay

            );

        public bool BackButtonIsVisible
        {
            get => (bool)GetValue(BackButtonIsVisibleProperty);
            set => SetValue(BackButtonIsVisibleProperty, value);
        }

        public static readonly BindableProperty BackButtonIsVisibleProperty = BindableProperty.Create(

                nameof(BackButtonIsVisible),
                typeof(bool),
                typeof(ToolBar),
                default(bool),
                defaultBindingMode: BindingMode.OneWay

            );

        public string ToolBarTitle
        {
            get => (string)GetValue(ToolBarTitleProperty);
            set => SetValue(ToolBarTitleProperty, value);
        }

        public static readonly BindableProperty ToolBarTitleProperty = BindableProperty.Create(

                nameof(ToolBarTitle),
                typeof(string),
                typeof(ToolBar),
                default(string),
                defaultBindingMode: BindingMode.OneWay

            );

        public string ButtonTextSize
        {
            get => (string)GetValue(ButtonTextSizeProperty);
            set => SetValue(ButtonTextSizeProperty, value);
        }

        public static readonly BindableProperty ButtonTextSizeProperty = BindableProperty.Create(

                nameof(ButtonTextSize),
                typeof(string),
                typeof(ToolBar),
                default(string),
                defaultBindingMode: BindingMode.OneWay

            );

        public string ToolBarTextSize
        {
            get => (string)GetValue(ToolBarTextSizeProperty);
            set => SetValue(ToolBarTextSizeProperty, value);
        }

        public static readonly BindableProperty ToolBarTextSizeProperty = BindableProperty.Create(

                nameof(ToolBarTextSize),
                typeof(string),
                typeof(ToolBar),
                default(string),
                defaultBindingMode: BindingMode.OneWay

            );

    }
}