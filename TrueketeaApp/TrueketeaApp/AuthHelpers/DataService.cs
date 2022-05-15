using System.Collections.ObjectModel;
using Xamarin.Forms;
using TrueketeaApp.Models;
using System.Data;
using TrueketeaApp.ViewModels;
using TrueketeaApp.Services.MongoService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xamarin.Forms.Internals;
using System.Linq;
using TrueketeaApp.Services;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Reflection;
using MongoDB.Bson;

namespace TrueketeaApp.Service
{
    public class DataService
    {
        public static ObservableCollection<ChatMessage> msg = new ObservableCollection<ChatMessage>();
        public static ObservableCollection<Item> product = new ObservableCollection<Item>();
        public static ObservableCollection<MyContactsModel> Usr = new ObservableCollection<MyContactsModel>();
        public static ObservableCollection<ChatDetail> UsrChat = new ObservableCollection<ChatDetail>();
        public static ObservableCollection<CommentsModel> Comments = new ObservableCollection<CommentsModel>();
        public static ObservableCollection<TruequeModel> TruequeData = new ObservableCollection<TruequeModel>();
        public static ObservableCollection<NotiffyModel> Notifications = new ObservableCollection<NotiffyModel>();
        public static List<ProductModel> MyFavorites;
        private static Color favorite_color;
        private static B8Clases B8 = new B8Clases();
        

        public static ObservableCollection<Group> GetGroups()
        {

            string sql;
            DataTable cat = new DataTable();
            int i = 0;

            ObservableCollection<Group> categories = new ObservableCollection<Group>();

            sql = "select Categorie_Desc,Icon,Id_Categorie from Categories where active = '1'";

            if ((ViewModelLocator.sql.DbSelect(sql, ref cat) == 0))
            {

                foreach (DataRow item in cat.Rows)
                {

                    if (i == 0)
                    {
                        categories.Add(new Group
                        {
                            description = item[0].ToString(),
                            image = item[1].ToString(),
                            id = item[2].ToString(),
                            selected = true,
                            backgroundColor = Color.FromHex("#0bbf2f")
                        });

                    }
                    else
                    {
                        categories.Add(new Group
                        {
                            description = item[0].ToString(),
                            image = item[1].ToString(),
                            selected = false,
                            backgroundColor = Color.FromHex("#F2F3F4")
                        });
                    }
                    i = i + 1;
                }


            }

            return categories;
        }

        private static bool SearchFav(string id_product)
        {
           foreach(var item in MyFavorites)
            {
                if (item.IdProducto.ToString().Equals(id_product))
                {
                    return true;
                }
            }

            return false;
        }

      

        public  static ObservableCollection<Item> GetItems() 
        {
            product.Clear();
            var allItems = ViewModelLocator.mongo.GetProducts();
            MyFavorites = ViewModelLocator.mongo.GetUserProducts(AppConstant.Constants.UserLoginId, "Favorites");
            
            if (allItems != null)
            {
                foreach (var item in allItems)
                {
                    if(item.User_id.ToString() != AppConstant.Constants.UserLoginId)
                    {
                        if (item.Estado != "Bloqueado")
                        {
                            if (SearchFav(item.Id.ToString()))
                            {
                                favorite_color = Color.Red;
                            }
                            else
                            {
                                favorite_color = Color.Wheat;
                            }
                            product.Add(new Item
                            {
                                Id = item.Id,
                                description = item.NombreProducto,
                                complement = item.ShortDesc,
                                image = ImageSource.FromUri(new Uri(item.Foto1)),
                                price = item.Precio,
                                rating = item.Likes,
                                backgroundColor = favorite_color,
                                favorite = true
                            });
                        }
                    }
                    
                }

            }
            else
            {
                product.Add(new Item
                {
                    description = "No hay Resultados para mostrar.",
                    complement = "",
                    image = "",
                    price = 0,
                    rating = 0,
                    favorite = false
                });
            }
         
           
            return product;
            
        }

        public static ObservableCollection<Item> GetUserItems(string User,string collection)
        {
            
            product.Clear();
            var allItems = ViewModelLocator.mongo.GetUserProducts(User, collection);

            if (allItems != null & allItems.Count > 0)
            {
                foreach (var item in allItems)
                {
                    string productName = item.NombreProducto;//

                        product.Add(new Item
                        {
                            Id = item.Id,
                            description = $"{productName.Substring(0, 10)} ...",
                            complement = item.ShortDesc,
                            image = item.Foto1,
                            status = item.Estado,
                            category = item.Categoria,
                            price = item.Precio,
                            rating = item.Likes,
                            favorite = true
                        });
                    
                   
                }
            }
            else
            {
                //product.Add(new Item
                //{
                //    description = "",
                //    complement = "",
                //    image = "not_found.png",
                //    category = "categoria",
                //    price = 0,
                //    rating = 0,
                //    favorite = false
                //});
            }
            return product;
        }

        public static ObservableCollection<Item> GetItemsCategory(string CatName)
        {
            product.Clear();
            var allItems = ViewModelLocator.mongo.GetProductsCath(CatName);

            if (allItems != null & allItems.Count > 0)
            {

                foreach (var item in allItems)
                {

                    if (item.User_id.ToString() != AppConstant.Constants.UserLoginId)
                    {
                        if (item.Estado != "Bloqueado")
                        {
                            if (SearchFav(item.Id.ToString()))
                            {
                                favorite_color = Color.Red;
                            }
                            else
                            {
                                favorite_color = Color.Wheat;
                            }
                            product.Add(new Item
                            {
                                Id = item.Id,
                                description = item.NombreProducto,
                                complement = item.ShortDesc,
                                image = ImageSource.FromUri(new Uri(item.Foto1)),
                                price = item.Precio,
                                rating = item.Likes,
                                backgroundColor = favorite_color,
                                favorite = true
                            });

                            //product.Add(new Item
                            //{
                            //    description = item.NombreProducto,
                            //    complement = item.ShortDesc,
                            //    image = item.Foto1,
                            //    price = item.Precio,
                            //    rating = item.Likes,
                            //    favorite = true
                            //});
                        }
                    }

                }

            }
            else
            {
                product.Add(new Item
                {
                    description = "No hay Resultados para mostrar.",
                    complement = "",
                    image = "",
                    price = 0,
                    rating = 0,
                    favorite = false
                });
            }


            return product;

        }

        public static ObservableCollection<NotiffyModel> GetNotiffies()
        {
            Notifications.Clear();
            var allItems = ViewModelLocator.mongo.GetNotifications();
            if (allItems != null & allItems.Count > 0)
            {
                foreach (var item in allItems)
                {
                    Notifications.Add(new NotiffyModel
                    {
                        Id = item.Id,
                        Receptor = item.Receptor,
                        Tipo = item.Tipo,
                        Texto = item.Texto,
                        Estado = item.Estado,
                        IdProducto = item.IdProducto,
                        Chat = item.Chat
                    });
                }
            }

            return Notifications;

        }

        public static ObservableCollection<ProductStatus> GetProductStatus()
        {
            string sql;
            DataTable cat = new DataTable();
            //int i = 0;

            ObservableCollection<ProductStatus> status = new ObservableCollection<ProductStatus>();

            sql = "select Id_SubCat,Subcat_Desc,Subcat_Name from Subcategories where active = '1'";

            if ((ViewModelLocator.sql.DbSelect(sql, ref cat) == 0))
            {
                foreach (DataRow item in cat.Rows)
                {
                        status.Add(new ProductStatus
                        {
                            StatusId = item[0].ToString(),
                            StatusLongDesc = item[1].ToString(),
                            StatusName = item[1].ToString()
                        });
                }
            }

            return status;
        }

        public static bool NewConversation(ObservableCollection<ChatMessage> conversation,string user2, string lastsms)
        {
            ChatMessages newmessage = new ChatMessages();
            newmessage.Id = ObjectId.GenerateNewId();
            newmessage.chatMessageInfo = new object[conversation.Count];
            for (int i = 0; i <= conversation.Count - 1; i++)
            {
                newmessage.chatMessageInfo[i] = conversation[i];
            }
            if(lastsms != "")
            {
                lastsms = lastsms.Substring(0, 5);
                lastsms = lastsms + "...";
            }
           
            if (ViewModelLocator.mongo.InsertConversation(newmessage)==true)
            {
                B8.InsertChatRelationship(AppConstant.Constants.UserLoginId, user2, newmessage.Id.ToString(), lastsms);
            }
            return true;
        }

       public static bool UpdateMessages (ObservableCollection<ChatMessage> conversation,string id_conversation,string lastsms)
        {
            int i = 0;
            ChatMessages newmessage = new ChatMessages();
            
            newmessage.Id = ObjectId.Parse(id_conversation);

            newmessage.chatMessageInfo = new object[conversation.Count];

            for(i = 0; i<= conversation.Count -1; i++)
            {
                newmessage.chatMessageInfo[i] = conversation[i];
            }


            if (lastsms != "")
            {
                lastsms = lastsms.Substring(0, 5);
                lastsms = lastsms + "...";
            }

            
            ViewModelLocator.mongo.UpdateChat(newmessage);
            B8.UpdateExpress("ChatRelationship", "Conversation_id", id_conversation, "MsgType", "Text", "MsgStatus", "Viewed", "LastMsg", lastsms);

            return true;
        } 

        public static ObservableCollection<ChatMessage> GetMsg(string Id)
        {
            msg.Clear();

            string id_conversation = B8.DBLookupEx("ChatRelationship", "Conversation_id", "User1", AppConstant.Constants.UserLoginId, "User2", Id);

            if (!string.IsNullOrWhiteSpace(id_conversation))
            {

                var allItems = ViewModelLocator.mongo.GetMessage(id_conversation);
                if (allItems != null & allItems.Count > 0)
                {
                    foreach (var item in allItems)
                    {

                        var data = JsonConvert.SerializeObject(item.chatMessageInfo);

                        List<ChatMessage> elemento;

                        elemento = JsonConvert.DeserializeObject<List<ChatMessage>>(data);

                        foreach (var temp in elemento)
                        {
                            msg.Add(new ChatMessage
                            {
                                Message = temp.Message,
                                Time = temp.Time,
                                IsReceived = temp.IsReceived,
                                User_Send = temp.User_Send

                            });
                        }

                    }
                }
            }

            return msg;
        }

       

        public static ObservableCollection<ChatDetail> ChatContacts(string user)
        {
            UsrChat.Clear();

            string sql = "";
            DataTable chat = new DataTable();
            string owner = ViewModelLocator.sql.TableOwner + ".";

            sql = $"Select  Name, IIF(Connected>0,CAST(1 AS BIT),CAST(0 AS BIT)), PhotoPath, convert(varchar(30),Last_Conection),User_id,MsgType,MsgStatus,LastMsg " +
                  $"From {owner}view_users,ChatRelationship where User2 = User_id and  User1 = '{user}'";

            if ((ViewModelLocator.sql.DbSelect(sql, ref chat) == 0))
            {

                if (chat.Rows.Count > 0)
                {
                    foreach (DataRow contac in chat.Rows)
                    {
                        string estado;
                        if (Convert.ToBoolean(contac[1]) == true)
                        {
                            estado = "Available";
                        }
                        else
                        {
                            estado = "";
                        }
                        UsrChat.Add(new ChatDetail
                        {
                            User_id = contac[4].ToString(),
                            ImagePath = contac[2].ToString(),
                            SenderName = contac[0].ToString(),
                            Time = contac[3].ToString(),
                            MessageType = contac[5].ToString(),
                            Message = contac[7].ToString(),
                            AvailableStatus = estado,
                            NotificationType = contac[6].ToString()

                        });
                    }
                }
              
                
            }

            return UsrChat;
        }

        public static ObservableCollection<MyContactsModel> GetAllMyContacts(string user)
        {
            Usr.Clear();
            string sql = "";
            DataTable chat = new DataTable();
            string owner = ViewModelLocator.sql.TableOwner + ".";

            sql = $"Select Name, IIF(Connected>0,CAST(1 AS BIT),CAST(0 AS BIT)), PhotoPath, convert(varchar(30),Last_Conection) " +
                  $"From {owner}view_users where User_id in ( select User2 from ChatRelationship where User1 = '{user}')";

            if ((ViewModelLocator.sql.DbSelect(sql, ref chat) == 0))
            {

                foreach (DataRow contac in chat.Rows)
                {
                    string estado;
                    if (Convert.ToBoolean(contac[1]) == true)
                    {
                        estado = "Conectado";
                    }
                    else
                    {
                        estado = "Desconectado";
                    }
                    Usr.Add(new MyContactsModel
                    {
                        Connected = Convert.ToBoolean(contac[1]),
                        Estado = estado,
                        UserName = contac[0].ToString(),
                        Photo = contac[2].ToString(),
                        LastConnection = contac[3].ToString(),
                        selected = false,
                        backgroundColor = Color.FromHex("#FFFFFF")
                    });

                }


            }
            else
            {
                Usr.Add(new MyContactsModel
                {
                    Connected = false,
                    UserName = "No tienes ninguna conversación",
                    Photo = "",
                    LastConnection = "",
                    Estado = "",
                    selected = false,
                    backgroundColor = Color.FromHex("#FFFFFF")
                });
            }

            return Usr;
        }




        public static ObservableCollection<ProvincesModel> GetAllProvinces()
        {

            string sql;
            DataTable prov = new DataTable();
            int i = 0;

            ObservableCollection<ProvincesModel> provinces = new ObservableCollection<ProvincesModel>();

            sql = "select Id_Province,Province_Name,Internal_Code from SpainLocation ";

            if ((ViewModelLocator.sql.DbSelect(sql, ref prov) == 0))
            {

                foreach (DataRow item in prov.Rows)
                {

                        provinces.Add(new ProvincesModel
                        {
                            Id = item[0].ToString(),
                            Desc = item[1].ToString(),
                            InternalCode = item[2].ToString()
                        });

                }


            }

            return provinces;
        }


        public static ObservableCollection<RegisterData> GetProfilesFav()
        {
            string sql;
            DataTable prov = new DataTable();
            int i = 0;

            ObservableCollection<RegisterData> usr = new ObservableCollection<RegisterData>();

            sql = $"select Name,PhotoPath,Vote,Email " +
               $" from view_users where User_id in ( select fav_id from FavProfiles where User_id = '{AppConstant.Constants.UserLoginId}')";

            if ((ViewModelLocator.sql.DbSelect(sql, ref prov) == 0))
            {

                foreach (DataRow item in prov.Rows)
                {

                    usr.Add(new RegisterData
                    {
                        UserId = item[3].ToString(),
                        UserName = item[0].ToString(),
                        Photo = item[1].ToString(),
                        Gender = item[2].ToString()
                    });

                }

            }

            return usr;
        }

        public static ObservableCollection<RegisterData> GetDataUser(string email)
        {

            string sql;
            DataTable prov = new DataTable();
            int i = 0;

            ObservableCollection<RegisterData> usr = new ObservableCollection<RegisterData>();

            sql = $"select Name,Email,Password,AppUser,IIF(Gender = '1','Hombre','Mujer') AS Gen," +
               $"Province_Name,CONVERT(varchar(30),Last_Conection, 103) as LastConnect " +
               $" from view_users where Email = '{email}'";

            if ((ViewModelLocator.sql.DbSelect(sql, ref prov) == 0))
            {

                foreach (DataRow item in prov.Rows)
                {

                    usr.Add(new RegisterData
                    {
                        UserName = item[0].ToString(),
                        UserEmail = item[1].ToString(),
                        Password = item[2].ToString(),
                        AppUser = item[3].ToString(),
                        Gender = item[4].ToString(),
                        Province = item[5].ToString(),
                        LastConnection = item[6].ToString()
                    });

                }

            }

            return usr;
        }


        public static ObservableCollection<TruequeModel> GetTrueques(string UsrID, string collection)
        {

            TruequeData.Clear();
            var allItems = ViewModelLocator.mongo.GetTrueques(Convert.ToInt32(UsrID), collection);
            if (allItems != null & allItems.Count > 0)
            {
                foreach (var item in allItems)
                {
                    if (item.User == UsrID)
                    {
                        string letter = item.Letter;
                        TruequeData.Add(new TruequeModel
                        {
                            Categoria = item.Categoria,
                            Descripcion = item.Descripcion,
                            Estado = item.Estado,
                            Foto1 = item.Foto1,
                            IdProducto = item.IdProducto,
                            NombreProducto = item.NombreProducto,
                            User = item.User,
                            Owner = item.Owner,
                            Id = item.Id,
                            Color = item.Color,
                            Letter = letter.ToLower()

                        });

                    }
                }
            }
            else
            {
                TruequeData.Add(new TruequeModel
                {
                    Categoria = "",
                    Descripcion = "",
                    Estado = "",
                    Foto1 = "not_found",
                    IdProducto = "",
                    NombreProducto = "No hay datos para mostrar",
                    User = "",
                    Owner = "",
                    Color = "#dc4e41",
                    Letter = "&#xf06a;"

                });
            }

            return TruequeData;
        }



        public static ObservableCollection<CommentsModel> GetComments(string user)
        {
            B8Clases B8 = new B8Clases();
            Comments.Clear();
            var allItems = ViewModelLocator.mongo.GetComments();

            if (allItems != null & allItems.Count > 0)
            {
                foreach (var item in allItems)
                {
                    if (item.Receptor.ToString() == user)
                    {
                        string foto = B8.DBLookupEx("VIEW_USERS", "PHOTOPATH", "USER_ID", item.Emisor);
                        string name = B8.DBLookupEx("VIEW_USERS", "NAME", "USER_ID", item.Emisor);
                        string Votos = B8.DBLookupEx("VIEW_USERS", "VOTE", "USER_ID", item.Emisor);

                        Comments.Add(new CommentsModel
                        {
                            Foto = foto,
                            Emisor = name,
                            msg = item.msg,
                            Votos = Votos,//item.Votos,
                            Receptor = item.Receptor,
                            VotesImg = ShowVotes(Votos)
                        });
                    }
                    
                }

            }
            else
            {
                Comments.Add(new CommentsModel
                {
                    Foto="no_photo",
                    Receptor ="",
                    Emisor = "No hay Resultados para mostrar.",
                    Votos = "0",
                    msg = "",
                    VotesImg = "Img0.png"
                });
            }


           if(Comments.Count == 0  )
            {
                Comments.Add(new CommentsModel
                {
                    Foto = "no_photo",
                    Receptor = "",
                    Emisor = "",
                    Votos = "0",
                    msg = "No hay Resultados para mostrar.",
                    VotesImg = "Img0.png"
                });
            }

            return Comments;
        }

        public static bool AddFriend(string User_id,string fav_id)
        {
            string sql;
            int AffectedRow=0;

            sql = $"Insert into {ViewModelLocator.sql.TableOwner}.FavProfiles (User_id,Fav_id) values ('{User_id}','{fav_id}')";

            if ((ViewModelLocator.sql.DbExecute(sql, ref AffectedRow) == 0))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private static string ShowVotes(string valor)
        {

            if (Convert.ToInt32(valor) == 0)
            {
                return ("Img0.png");
            }

            if (Convert.ToInt32(valor) <= 10)
            {
                return ("Img1.png");
            }

            if (Convert.ToInt32(valor) > 10 && Convert.ToInt32(valor) <= 20)
            {
                return ("Img2.png");
            }

            if (Convert.ToInt32(valor) > 20 && Convert.ToInt32(valor) <= 30)
            {
                return ("Img3.png");
            }

            if (Convert.ToInt32(valor) > 30 && Convert.ToInt32(valor) <= 40)
            {
                return ("Img4.png"); 
            }

            if (Convert.ToInt32(valor) > 40)
            {
                return ("Img5.png");
            }

            return ("Img0.png");

        }

    }

}

