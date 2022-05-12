using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TrueketeaApp.Models;
using TrueketeaApp.Service;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using TrueketeaApp.Views;
using System.ComponentModel;
using TrueketeaApp.PageModels.Base;
using System.Threading.Tasks;

namespace TrueketeaApp.ViewModels
{
    public class MessagesViewModel : ViewModelBase

    {
        public Command RefreshCommand { private set; get; }
        public Command SelectGroupCommand { get; }
        public ObservableCollection<MyContactsModel> Contacts { get; private set; }
       
        public MessagesViewModel(ContentPage view)
        {
            SelectGroupCommand = new Command<MyContactsModel>((model) => ExecuteSelectGroupCommand(model));
            RefreshCommand = new Command(async () => await RefreshItems());
            GetContacts();
        }

        void GetContacts()
        {
            Contacts = new ObservableCollection<MyContactsModel>(DataService.GetAllMyContacts(AppConstant.Constants.UserLoginId));
            FilteredItems = Contacts;
        }

        public string FilterText
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }

        public static readonly BindableProperty FilterTextProperty = BindableProperty.Create(
           nameof(FilterText),
           typeof(string),
           typeof(MessagesViewModel),
           default(string),
           propertyChanged: OnFilterTextChanged);


        private static void OnFilterTextChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var page = (MessagesViewModel)bindable;
            string filterText = (string)newvalue;

            if (string.IsNullOrEmpty(filterText))
            {
                page.FilteredItems = page.Contacts;
            }
            else
            {
                page.FilteredItems = page.Contacts.Where(item =>
                {
                    return item.UserName.Split().Any(substring => substring.StartsWith(filterText, StringComparison.CurrentCultureIgnoreCase));
                });
            }
        }

        public static readonly BindableProperty FilteredItemsProperty = BindableProperty.Create(
            nameof(FilteredItems),
            typeof(IEnumerable<MyContactsModel>),
            typeof(MainViewModel),
            default(IEnumerable<MyContactsModel>));

        public IEnumerable<MyContactsModel> FilteredItems
        {
            get => (IEnumerable<MyContactsModel>)GetValue(FilteredItemsProperty);
            set => SetValue(FilteredItemsProperty, value);
        }


        private async Task RefreshItems()  //async Task 
        {
            //IsRefreshing = true;

            FilteredItems = Contacts;
            IsRefreshing = false;
        }


        private async void ExecuteSelectGroupCommand(MyContactsModel model)
        {
            var index = Contacts
                .ToList()
                .FindIndex(p => p.UserName == model.UserName);

            if (index > -1)
            {
                UnselectGroupItems();

                Contacts[index].selected = true;
                RefreshItems();
                await NavigationService.NavigateToAsync<ChatViewModel>();

            }

             
        }

        void UnselectGroupItems()
        {
            FilteredItems.ForEach((item) =>
            {
                item.selected = false;
                //item.backgroundColor = Color.FromHex("#F2F3F4");
            });

        }

    }
}

