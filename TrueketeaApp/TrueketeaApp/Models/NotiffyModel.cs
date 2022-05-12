using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TrueketeaApp.Models
{
	public class NotiffyModel
	{

		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }

		[BsonElement("Receptor")]
		public string Receptor { get; set; }

		[BsonElement("Tipo")]
		public int Tipo { get; set; }

		[BsonElement("Texto")]
		public string Texto { get; set; }

		[BsonElement("Estado")]
		public int Estado { get; set; }

		[BsonElement("IdProducto")]
		public string IdProducto { get; set; }

		[BsonElement("Chat")]
		public bool Chat { get; set; }
	}
}

