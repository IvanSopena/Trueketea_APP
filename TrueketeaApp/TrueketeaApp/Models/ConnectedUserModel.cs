using Xamarin.Forms;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TrueketeaApp.Models
{
    public class ConnectedUserModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId MongoId { get; set; }

        [BsonElement("User_id")]
        public int User_id { get; set; }

        [BsonElement("Contactos")]
        public int[] Contactos { get; set; }

    }
}
