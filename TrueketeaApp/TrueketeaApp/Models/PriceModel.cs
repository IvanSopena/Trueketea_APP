using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrueketeaApp.Models
{
	public class PriceModel
	{
		[BsonId, BsonRepresentation(BsonType.ObjectId)]
		public ObjectId Id { get; set; }

		[BsonElement("Categoria")]
		public string Categoria { get; set; }
		[BsonElement("Precio")]
		public string Precio { get; set; }
		[BsonElement("Producto")]
		public string Producto { get; set; }


	}
}

