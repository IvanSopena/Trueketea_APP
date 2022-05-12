using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TrueketeaApp.Models;
using MongoDB.Driver.Linq;
using System.Security.Authentication;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace TrueketeaApp.Services.MongoService
{

    public class MongoServiceDB
    {

        readonly static string dbName = "Trueketea";
        B8Clases B8 = new B8Clases();
        static MongoClient client;

        public MongoServiceDB()
        {
            try
            {
                var conx = "mongodb://192.168.1.51:27017";
                MongoClientSettings settings = MongoClientSettings.FromUrl(
                        new MongoUrl(conx)
                    );
                settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
                client = new MongoClient(settings);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("Constructor", "MongoServiceDB", msg);
            }


        }

        public List<ProductModel> DetailProduct (ObjectId ID,string collection)
        {
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>(collection);

                var allItems = ProductsCollection.AsQueryable<ProductModel>()
                    .Where(tdi => tdi.Id.Equals(ID))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetProductsCath", "MongoServiceDB", msg);
                return null;
            }
        }

        public List<CommentsModel> GetComments()
        {
            try
            {
                IMongoCollection<CommentsModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<CommentsModel>("UserComments");

                var allItems = ProductsCollection.AsQueryable<CommentsModel>().ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetProducts", "MongoServiceDB", msg);
                return null;
            }
        }

        public List<PriceModel> GetPrices()
        {
            try
            {
                IMongoCollection<PriceModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<PriceModel>("Products_Price");

                var allItems = ProductsCollection.AsQueryable<PriceModel>().ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetPrices", "MongoServiceDB", msg);
                return null;
            }
        }
        public List<ProductModel> GetProducts()
        {
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>("Products");

                var allItems = ProductsCollection.AsQueryable<ProductModel>().ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetProducts", "MongoServiceDB", msg);
                return null;
            }
        }

        public bool InsertConversation(ChatMessages newmsg)
        {
            try
            {
                IMongoCollection<BsonDocument> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<BsonDocument>("ChatMessages");

                var data = JsonConvert.SerializeObject(newmsg);
                var returnDocument = new BsonDocument(BsonDocument.Parse(data));

                returnDocument.Remove("Id");

                ProductsCollection.InsertOne(returnDocument);

                return true;

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("InsertConversation", "MongoServiceDB", msg);
                return false;
            }


        }

        public bool UpdateChat(ChatMessages newmsg)
        {
            try
            {
                string id = newmsg.Id.ToString();
                
                IMongoCollection<BsonDocument> ProductsCollection;
                var db = client.GetDatabase(dbName);

               
                ProductsCollection = db.GetCollection<BsonDocument>("ChatMessages");

                var filter = Builders<BsonDocument>.Filter
               .Eq("_id", new ObjectId(id));


                var data = JsonConvert.SerializeObject(newmsg);
               

                var returnDocument = new BsonDocument(BsonDocument.Parse(data));
               
                returnDocument.Remove("Id");

               
                ProductsCollection.ReplaceOne(filter, returnDocument);

                return true;

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("UpdateChat", "MongoServiceDB", msg);
                return false;
            }
        }

        public List<ChatMessages> GetMessage(String id)
        {
            try
            {
                IMongoCollection<ChatMessages> ProductsCollection;
                var db = client.GetDatabase(dbName);

               // BsonSerializer.Deserialize<ChatMessages>("ChatMessages");

                ProductsCollection = db.GetCollection<ChatMessages>("ChatMessages");

                // ProductsCollection.DocumentSerializer;
                //BsonSerializer.Deserialize<ChatMessages>("chatMessageInfo");

                var allItems = ProductsCollection.AsQueryable<ChatMessages>()
                   
                    .Where(tdi => tdi.Id.Equals(ObjectId.Parse(id)))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetProductsCath", "MongoServiceDB", msg);
                return null;
            }
        }

        public List<ProductModel> GetProductsCath(String cat_name)
        {
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>("Products");

                var allItems = ProductsCollection.AsQueryable<ProductModel>()
                    .Where(tdi => tdi.Categoria.Contains(cat_name))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetProductsCath", "MongoServiceDB", msg);
                return null;
            }
        }


       

        public bool InsertProduct(ProductModel prod,string collection)
        {
            //prod.Id =  prod. //ObjectId.GenerateNewId();
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>(collection);

                ProductsCollection.InsertOne(prod);

                

                return true;

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("InsertProduct", "MongoServiceDB", msg);
                return false;
            }


        }

        public bool UpdateNotification(NotiffyModel notification)
        {

            try
            {
                IMongoCollection<NotiffyModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<NotiffyModel>("Notifications");

                if (ProductsCollection.ReplaceOne(tdi => tdi.Id == notification.Id, notification).ModifiedCount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("UpdateNotification", "MongoServiceDB", msg);
                return false;
            }

        }

        public bool UpdateProduct(ProductModel prod)
        {

            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>("Products");

                if(ProductsCollection.ReplaceOne(tdi => tdi.Id==prod.Id, prod).ModifiedCount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("InsertProduct", "MongoServiceDB", msg);
                return false;
            }

        }

        public bool DeleteTrueque(TruequeModel item)
        {
            try
            {
                IMongoCollection<TruequeModel> ProductsCollection;
                var db = client.GetDatabase(dbName);
                ProductsCollection = db.GetCollection<TruequeModel>("Trueques");

                if (ProductsCollection.DeleteOne(tdi => tdi.Id == item.Id).DeletedCount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("DeleteTrueque", "MongoServiceDB", msg);
                return false;
            }
        }

        public bool DeleteCollect(ProductModel prod, string collections)
        {
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);
                ProductsCollection = db.GetCollection<ProductModel>(collections);

                if (ProductsCollection.DeleteOne(tdi => tdi.Id == prod.Id).DeletedCount == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("DeleteCollect", "MongoServiceDB", msg);
                return false;
            }
        }

        public List<ConnectedUserModel> GetContacts(int user)
        {
            try
            {
                IMongoCollection<ConnectedUserModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ConnectedUserModel>("ChatUsers");

                var allItems = ProductsCollection.AsQueryable<ConnectedUserModel>()
                    .Where(tdi => tdi.User_id.Equals(user))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetContacts", "MongoServiceDB", msg);
                return null;
            }
        }


        public List<ProductModel> GetUserProducts(String Userid,string collection)
        {
            try
            {
                IMongoCollection<ProductModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<ProductModel>(collection);

                

                if (collection.Equals("Products"))
                {
                    var allItems = ProductsCollection.AsQueryable<ProductModel>()
                    .Where(tdi => tdi.User_id.Equals(Userid))
                    .ToList();

                    return allItems;
                }
                else
                {
                    var allItems = ProductsCollection.AsQueryable<ProductModel>()
                    .Where(tdi => tdi.Fav_User_Id.Equals(Userid))
                    .ToList();

                    return allItems;
                }

            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetUserProducts", "MongoServiceDB", msg);
                return null;
            }
        }


        public bool InsertCommet(CommentsModel mensaje)
        {
            mensaje.Id = ObjectId.GenerateNewId();
            try
            {
                IMongoCollection<CommentsModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<CommentsModel>("UserComments");

                ProductsCollection.InsertOne(mensaje);



                return true;

            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("InsertComment", "MongoServiceDB", msg);
                return false;
            }


        }

        public List<NotiffyModel> GetNotifications ()
        {
            try
            {
                IMongoCollection<NotiffyModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<NotiffyModel>("Notifications");

                var allItems = ProductsCollection.AsQueryable<NotiffyModel>()
                    .Where(tdi => tdi.Estado.Equals(0))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetContacts", "MongoServiceDB", msg);
                return null;
            }
        }

        public List<TruequeModel> GetTrueques(int user, string collection)
        {
            try
            {
                IMongoCollection<TruequeModel> ProductsCollection;
                var db = client.GetDatabase(dbName);

                ProductsCollection = db.GetCollection<TruequeModel>(collection);

                var allItems = ProductsCollection.AsQueryable<TruequeModel>()
                    .Where(tdi => tdi.User.Equals(user))
                    .ToList();

                return allItems;
            }

            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                B8.Log_Error("GetContacts", "MongoServiceDB", msg);
                return null;
            }
        }

    }
}
