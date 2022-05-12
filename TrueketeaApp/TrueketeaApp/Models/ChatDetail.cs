using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace TrueketeaApp.Models
{
   
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ChatDetail
    {

        [DataMember(Name = "User_id")]
        public string User_id { get; set; }
       
        [DataMember(Name = "imagePath")]
        public string ImagePath { get; set; }



        [DataMember(Name = "senderName")]
        public string SenderName { get; set; }

       
        [DataMember(Name = "time")]
        public string Time { get; set; }

       
        [DataMember(Name = "message")]
        public string Message { get; set; }

        
        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }

       
        [DataMember(Name = "notificationType")]
        public string NotificationType { get; set; }

       
        [DataMember(Name = "availableStatus")]
        public string AvailableStatus { get; set; }

       
    }
}
