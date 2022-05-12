using System;
using System.Collections.ObjectModel;
using TrueketeaApp.Models;
using TrueketeaApp.PageModels.Base;
using TrueketeaApp.Service;
using TrueketeaApp.Services.Messages;
using Xamarin.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TrueketeaApp.Services;

namespace TrueketeaApp.ViewModels
{
    public class ConversationViewModel: ViewModelBase
    {
        Warnings wg = new Warnings();
        private B8Clases B8 = new B8Clases();

        private ObservableCollection<ChatMessage> chatMessageInfo = new ObservableCollection<ChatMessage>();
        public string user_photo { get; set; }
        public string user_name { get; set; }
        public bool status { get; set; }
        private string receptor { get; set; }
        private Command menuCommand;
        private ContentPage MyView;
        private Command sendCommand; 
        private string newMessage;
        private HubConnection hubConnection;


        public ConversationViewModel(ContentPage view, string foto, string user, bool _status,string receptor_id)
        {
           // hubConnection = new HubConnectionBuilder()
           //.WithUrl($"http://localhost:5000/SendHub") 
           //.Build();
            MyView = view;
            user_photo = foto;
            user_name = user;
            status = _status;
            receptor = receptor_id;
            GetMessages();
            AppConstant.Constants.ActualView = "chat";

            Device.StartTimer(new TimeSpan(0, 0, 10), () =>
            {

                Device.BeginInvokeOnMainThread(() =>
                {
                    GetMessages();
                });
                return true; 
            });


        }

        public string NewMessage
        {
            get
            {
                return this.newMessage;
            }

            set
            {
                this.SetProperty(ref this.newMessage, value);
            }
        }

        public ObservableCollection<ChatMessage> ChatMessageInfo
        {
            get
            {
                return this.chatMessageInfo;
            }

            set
            {
                this.SetProperty(ref this.chatMessageInfo, value);
            }
        }

        private void GetMessages()
        {
            ChatMessageInfo = new ObservableCollection<ChatMessage>(DataService.GetMsg(receptor));
        }

        public Command MenuCommand
        {
            get { return this.menuCommand ?? (this.menuCommand = new Command(this.MenuClicked)); }
        }

        public Command SendCommand
        {
            get { return this.sendCommand ?? (this.sendCommand = new Command(this.SendClicked)); }
        }



        private async void MenuClicked(object obj)
        {
            await wg.ToastInfo("En desarrollo", MyView);
            
        }

        private void SendClicked(object obj)
        {
            string id_conversation = B8.DBLookupEx("ChatRelationship", "Conversation_id", "User1", AppConstant.Constants.UserLoginId, "User2", receptor);

            if (!string.IsNullOrWhiteSpace(this.NewMessage))
            {
                this.ChatMessageInfo.Add(new ChatMessage
                {
                    Message = this.NewMessage,
                    Time = DateTime.Now,
                    User_Send = AppConstant.Constants.UserLoginId
                });
            }

            if (!string.IsNullOrWhiteSpace(id_conversation))
            {
                DataService.UpdateMessages(ChatMessageInfo, id_conversation,this.NewMessage);
            }
            else
            {
                DataService.NewConversation(ChatMessageInfo,receptor, this.NewMessage);
            }

            this.NewMessage = null;
            
        }
    }
}
