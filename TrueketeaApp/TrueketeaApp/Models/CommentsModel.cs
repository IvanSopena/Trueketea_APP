using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.PageModels.Base;

namespace TrueketeaApp.Models
{
   public class CommentsModel 
    {

        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("Foto")]
        public string Foto { get; set; }
        [BsonElement("Emisor")]
        public string Emisor { get; set; }
        [BsonElement("Receptor")]
        public string Receptor { get; set; }
        [BsonElement("Votos")]
        public string Votos { get; set; }
        [BsonElement("msg")]
        public string msg { get; set; }

        public string VotesImg { get; set; }




    }
}
