using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Views;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class ChatListViewModel : ViewModelBase
    {
        public Command<object> ItemSelectedCommand { get; set; }
        
        private ObservableCollection<ChatDetail> chatItems;
        public ObservableCollection<ChatDetail> Contacts
        {
            get
            {
                return this.chatItems;
            }

            set
            {
                if (this.chatItems == value)
                {
                    return;
                }

                this.SetProperty(ref this.chatItems, value);
            }
        }

        private string profileImage;
        public string ProfileImage
        {
            get
            {
                return this.profileImage;
            }

            set
            {
                this.SetProperty(ref this.profileImage, value);
            }
        }

        private string profileName;
        public string ProfileName
        {
            get
            {
                return this.profileName;
            }

            set
            {
                this.SetProperty(ref this.profileName, value);
            }
        }

        ChatDetail _selected;
        public ChatDetail Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }
        public ChatListViewModel(ContentPage view)
        {
            ItemSelectedCommand = new Command<object>(ItemSelected);
           
            ProfileName = AppConstant.Constants.UserName;
            ProfileImage = AppConstant.Constants.UserProfilePhoto;
            GetContacts();
        }

       

        void GetContacts()
        {
            Contacts = new ObservableCollection<ChatDetail>(DataService.ChatContacts(AppConstant.Constants.UserLoginId));
            
        }


        public async void ItemSelected(object model)
        {
            bool st;
            string photo;
            string usr;
            string usr_id;
            var ItemData = (model as Syncfusion.ListView.XForms.ItemTappedEventArgs).ItemData as ChatDetail;

            if (ItemData.AvailableStatus.Equals(""))
            {
                st = false;
            }
            else
            {
                st = true;
            }

            photo = ItemData.ImagePath;
            usr = ItemData.SenderName;
            usr_id = ItemData.User_id;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            await navigationPage.PushAsync(new ChatConversationView(photo,usr,st,usr_id));
            
        }


    }
}
