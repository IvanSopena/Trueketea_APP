using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrueketeaApp.Models
{
	public class ChatMessages
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }

		
		[BsonElement("chatMessageInfo")]
		public object[] chatMessageInfo { get; set; }
	}
}

