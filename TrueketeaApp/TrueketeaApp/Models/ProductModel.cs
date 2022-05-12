
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using Xamarin.Forms;

namespace TrueketeaApp.Models
{
   
    public class ProductModel 
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("Categoria")]
        public string Categoria { get; set; }
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }
        [BsonElement("Direccion")]
        public string Direccion { get; set; }
        [BsonElement("Estado")]
        public string Estado { get; set; }
        [BsonElement("Foto1")]
        public string Foto1 { get; set; }
      
        [BsonElement("Foto2")]
        public string Foto2 { get; set; }
        [BsonElement("Foto3")]
        public string Foto3 { get; set; }
        [BsonElement("Foto4")]
        public string Foto4 { get; set; }
        [BsonElement("IdProducto")]
        public string IdProducto { get; set; }
        [BsonElement("Likes")]
        public int Likes { get; set; }
        [BsonElement("NombreProducto")]
        public string NombreProducto { get; set; }
        [BsonElement("Precio")]
        public decimal Precio { get; set; }
        [BsonElement("Id_Usuario")]
        public int User_id { get; set; }
        [BsonElement("Usuario")]
        public string User_Name { get; set; }

        [BsonElement("DescBreve")]
        public string ShortDesc { get; set; }

        [BsonElement("Long")]
        public double Longitud { get; set; }

        [BsonElement("Lat")]
        public double Latitud { get; set; }

        [BsonElement("UserPic")]
        public string USerPic { get; set; }

        [BsonElement("EstadoActual")]
        public string EstadoActual { get; set; }

        [BsonElement("Fav_User_Id")]
        public int Fav_User_Id { get; set; }

    }
}
