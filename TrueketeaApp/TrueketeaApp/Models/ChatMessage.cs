using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace TrueketeaApp.Models
{
   
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ChatMessage : INotifyPropertyChanged
    {
       

        private string message;
        private DateTime time;
        private string imagePath;
        
        

      
        public event PropertyChangedEventHandler PropertyChanged;

     

       
        [DataMember(Name = "message")]
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }

        
        [DataMember(Name = "time")]
        public DateTime Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.OnPropertyChanged("Time");
            }
        }


        [DataMember(Name = "imagePath")]
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                this.OnPropertyChanged("ImagePath");
            }
        }


        [DataMember(Name = "isReceived")]
        public bool IsReceived { get; set; }

        [DataMember(Name = "User_Send")]
        public string User_Send { get; set; }

        //[DataMember(Name = "_t")]
        //public string _t { get; set; }


        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

       
    }
}
