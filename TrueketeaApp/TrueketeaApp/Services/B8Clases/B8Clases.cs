using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrueketeaApp.Models;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using TrueketeaApp.ViewModels;

namespace TrueketeaApp.Services
{
    public class B8Clases 
    {

        public bool InsertChatRelationship(string user1, string user2,string id_conversation,string lastmessage)
        {
            string sql = "";
            int rows = 0;

         
            sql = $"Insert into ChatRelationship (User1,User2,Conversation_id,MsgType,MsgStatus,LastMsg) values ('{user1}','{user2}','{id_conversation}','Text','Viewed','{lastmessage}')";

            if (0 != ViewModelLocator.sql.DbExecute(sql, ref rows))
            {
                return false;
            }
            if (rows == 0)
            {
                return false;
            }

            rows = 0;

            sql = $"Insert into ChatRelationship (User1,User2,Conversation_id) values ('{user2}','{user1}','{id_conversation}')";

            if (0 != ViewModelLocator.sql.DbExecute(sql, ref rows))
            {
                return false;
            }
            if (rows == 0)
            {
                return false;
            }

            return true;

        }


        public bool AddHistory(string IdProduct,string name,string User,string status,string category)
        {
            string sql = "";
            string thisDay = DateTime.Today.ToString("dd/MM/yyyy");
            string owner = ViewModelLocator.sql.TableOwner + ".";
            string province = this.DBLookupEx("Users", "Location", "Email", AppConstant.Constants.UserLoginEmail);
            int rows = 0;

            sql = $"Insert into {owner}HistoricalActivities (IdProduct,NombreProducto,UserId,StatusId,CategoryId,ProvinceId,TransDate) " +
                $"Values('{IdProduct}','{name}','{User}','{status}','{category}','{province}',convert(date, '{thisDay}'))";

            if (0 != ViewModelLocator.sql.DbExecute(sql, ref rows))
            {
                return false;
            }

            if (rows == 0)
            {
                return false;
            }

            return true;
        }

        public bool UpdateExpress(string table, string key, string keyvalue, string PK1, string VALUE1, 
            string PK2 = "", string VALUE2 = "", string PK3 = "", string VALUE3 = "", string PK4 = "", 
            string VALUE4 = "", string PK5 = "", string VALUE5 = "", string PK6 = "", string VALUE6 = "")
        {
            int affectedRow = 0;
            string SQL;
            string owner = ViewModelLocator.sql.TableOwner +".";

            SQL = $"Update {owner + table} set {PK1} = '{VALUE1}'";
            if (string.IsNullOrEmpty(PK2) == false)
            {
                SQL = SQL + $" , {PK2} = '{VALUE2}'";

                if (string.IsNullOrEmpty(PK3) == false)
                {
                    SQL = SQL + $" , {PK3} = '{VALUE3}'";

                    if (string.IsNullOrEmpty(PK4) == false)
                    {
                        SQL = SQL + $" , {PK4} = '{VALUE4}'";

                        if (string.IsNullOrEmpty(PK5) == false)
                        {
                            SQL = SQL + $" , {PK5} = '{VALUE5}'";

                            if (string.IsNullOrEmpty(PK6) == false)
                            {
                                SQL = SQL + $" , {PK6} = '{VALUE6}'";
                            }

                        }

                    }

                }

            }

            SQL = SQL + $" Where {key} = '{keyvalue}'";

            if (0 != ViewModelLocator.sql.DbExecute(SQL, ref affectedRow))
            {
                return false;
            }

            if(affectedRow == 0)
            {
                return false;
            }

            return true;
        }

        public string DBLookupEx(string table, string FIELDTOLOOKUP, string PK1 = "", string VALUE1 = "", 
            string PK2 = "", string VALUE2 = "", string PK3 = "", string VALUE3 = "", string PK4 = "", 
            string VALUE4 = "", string PK5 = "", string VALUE5 = "", string PK6 = "", string VALUE6 = "")
        {
            //  devuelve una consulta de un campo con hasta 6 condiciones
            //  Nota: si se especifican m�s de un campo en la consulta (separados por comas) devuelve los resultados
            //  separados por vbtab. ByVal
            // Atencion : mandar las fechas con la funcion de fechas de SQL_SERVER o ACCESS
            //  NOTA:
            //  Esta versi�n exige el objeto database como par�metro.
            // 

            try
            {
                int i;
                string SQL;
                string resultado = "";
                DataTable Dr = new DataTable();


                SQL = ("select " + (FIELDTOLOOKUP + (" from " + table)));

                if ((PK1 != ""))
                {
                    SQL = SQL + " WHERE " + PK1 + "='" + VALUE1 + "'";
                    if ((PK2 != ""))
                    {
                        SQL = SQL + " and " + PK2 + "='" + VALUE2 + "'";
                        if ((PK3 != ""))
                        {
                            SQL = SQL + " and " + PK3 + "='" + VALUE3 + "'";
                            if ((PK4 != ""))
                            {
                                SQL = SQL + " and " + PK4 + "='" + VALUE4 + "'";
                                if ((PK5 != ""))
                                {
                                    SQL = SQL + " and " + PK5 + "='" + VALUE5 + "'";
                                    if ((PK6 != ""))
                                    {
                                        SQL = SQL + " and " + PK6 + "='" + VALUE6 + "'";
                                    }

                                }

                            }

                        }

                    }

                }


                if ((ViewModelLocator.sql.DbSelect(SQL, ref Dr) != 0))
                {
                    return "";
                }

                if ((Dr.Rows.Count == 0))
                {
                    resultado = "";
                }
                else
                {

                    for (i = 0; (i <= Dr.Rows.Count - 1); i++)
                    {
                        string row = Convert.ToString(Dr.Rows[0][i]);
                        resultado = row;
                    }


                }

                if ((resultado == ""))
                {
                    return "";
                }
                else
                {
                    return resultado;
                }


            }

            catch (Exception)
            {
                return "";
            }
        }


        object ConvertirNulos(string UnValor, string defecto, bool sT)
        {
            if (UnValor == null)
            {
                return defecto;
            }
            else
            {

                return UnValor;

            }

        }

        public string StrComillasBD(string Cadena)
        {
            string aux;
            string C;
            int i;

            aux = "";
            for (i = 1; (i <= Cadena.Length); i++)
            {
                C = Cadena.Substring((i - 1), 1);
                if ((C == "\'"))
                {
                    aux = (aux + "\'\'");
                }
                else
                {
                    aux = (aux + C);
                }

            }

            return aux;
        }

        public string GetItemList(string Lista, int NumItem, string separador)
        {
            string Cadena;
            int Posic;
            int i;
            int lenSeparador;
            int LastPosic;


            Cadena = Lista;
            Posic = 1;
            lenSeparador = separador.Length;

            for (i = 1; (i <= (NumItem - 1)); i++)
            {

                Posic = Cadena.IndexOf(separador);
                Cadena = Mid(Cadena, Posic + lenSeparador);
            }


            if ((Posic == 0))
            {
                return "";


            }


            LastPosic = (Cadena.IndexOf(separador) + 1);
            if ((LastPosic == 0))
            {
                // Es el �ltimo Item y ya tenemos el resultado
                return Cadena;
            }
            else
            {
                return Cadena.Substring(0, (LastPosic - 1));
            }

        }

        public static string Right(string param, int length)
        {

            int value = param.Length - length;
            string result = param.Substring(value, length);
            return result;
        }

        public static string Left(string param, int length)
        {

            string result = param.Substring(0, length);
            return result;
        }

        public static string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex, length);
            return result;
        }

        public string Mid(string param, int startIndex)
        {
            string result = param.Substring(startIndex);
            return result;
        }


        public int Getnumlist(string Lista, ref string separador)
        {
            int i;
            int Inicio;
            Inicio = 0;
            // Si el separador no es Espacio, se realiza un TRIM de la Lista.
            if ((separador != " "))
            {
                Lista = Lista.Trim();
            }

            if ((Lista != ""))
            {
                i = 1;
            }
            else
            {
                i = 0;
            }

            // Se realiza un bucle averiguando los elementos y sus posiciones.
            if ((separador != ""))
            {
                for (
                ; (i < Lista.Length);
                )
                {
                    Inicio = (Lista.IndexOf(separador, Inicio) + 1);
                    if ((Inicio == 0))
                    {
                        break; //Warning!!! Review that break works as 'Exit Do' as it could be in a nested instruction like switch
                    }

                    i = (i + 1);
                }

            }
            else
            {
                i = 1;
            }

            return i;
        }

        public void Log_Error(string method,string classorigen,string err,string query = "")
        {

            string sql = "";
            int affectedRow = 0;

            err = err.Replace("'", "");
            query = query.Replace("'", "");

            EmailMessages msg = new EmailMessages("trueketeaadm@gmail.com");
            string idError = this.DBLookupEx(ViewModelLocator.sql.TableOwner + ".LogErrors", "IsNull(max(Id_Error) +1,1)", "1", "1");

            sql = $"insert into {ViewModelLocator.sql.TableOwner}.LogErrors (Id_Error,App_Name,Method,Error_Desc,Active,Class,SQL_Sentence)" +
                $" Values('{idError}','Movile_Version','{method}','{err}','1','{classorigen}','{query}')";

            if (0 != ViewModelLocator.sql.DbExecute(sql, ref affectedRow))
            {

                string incidencia = $"Error en AppMovil: {err}. La clase:{classorigen} y el Metodo:{method}.";
                msg.Send_Email(1, incidencia);
            }
            else
            {
                if (affectedRow == 0)
                {
                    string incidencia = $"Error en AppMovil: {err}. La clase:{classorigen} y el Metodo:{method}.";
                    msg.Send_Email(1, incidencia);
                }
            }




        }

        public string ValidationCode()
        {
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[6];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var resultString = new String(Charsarr);

            return resultString;
        }


    }


    
}
