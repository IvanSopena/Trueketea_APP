using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrueketeaApp.Models
{
   public class TruequeModel
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Categoria")]
        public string Categoria { get; set; }

        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }
        
        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("Foto1")]
        public string Foto1 { get; set; }

        [BsonElement("IdProducto")]
        public string IdProducto { get; set; }

        [BsonElement("NombreProducto")]
        public string NombreProducto { get; set; }
        //[BsonElement("Precio")]
        //public decimal Precio { get; set; }
        [BsonElement("User")]
        public string User { get; set; }

        [BsonElement("Owner")]
        public string Owner { get; set; }

        [BsonElement("Color")]
        public string Color { get; set; }

        [BsonElement("Letter")]
        public string Letter { get; set; }


        //PROCESO OK: LETRAS(&#xf00c;) COLOR(#7ed321)
        //PROCESO PROCESO: LETRAS(&#xf017;) COLOR(#fcbc0f)
        //PROCESO CANCEL: LETRAS(&#xf05e;) COLOR(#dc4e41)

    }
}
