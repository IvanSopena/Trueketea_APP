using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueketeaAdmin.Services.Security;
using TrueketeaApp.Services.Messages;

namespace TrueketeaApp.Services.DataBase
{
    public class DBContext
    {
        private string mDbAlias;
        private string mTableOwner;
        private string mDbLastError;
        private string mClsLastError;
        private bool mIsOpen = false;
        private string mDbLastSQL;
        private int mOpenTrans = 0;
        private string mDbDateFormat;
        private string mDbTimeFormat;

        public SqlConnection DbConn;
        private SqlDataReader mDr;

        private readonly Encrypting Encrypt = new Encrypting();
        private string pass;


        public string DbAlias { get { return mDbAlias; } set { mDbAlias = value; } }
        public string TableOwner { get { return mTableOwner; } set { mTableOwner = value; } }
        public string DbLastError { get { return mDbLastError; } set { mDbLastError = value; } }
        public string ClsLastError { get { return mClsLastError; } set { mClsLastError = value; } }
        public string DbLastSQL { get { return mDbLastSQL; } set { mDbLastSQL = value; } }
        public bool IsOpen { get { return mIsOpen; } set { mIsOpen = value; } }
        public bool OpenTrans { get { return OpenTrans; } set { OpenTrans = value; } }
        public string DbDateFormat { get { return mDbDateFormat; } set { DbDateFormat = value; } }
        public string DbTimeFormat { get { return mDbTimeFormat; } set { mDbTimeFormat = value; } }


        public int Conexion(string User, string Pwd, string Server, string db)
        {
            // Establecemos la conexion con la base de datos

            try
            {

                pass = Encrypt.Decrypt(Pwd, User);
                DbConn = new SqlConnection()
                {
                    ConnectionString = "Persist Security Info=False;User ID=" + User + ";Password=" + pass + ";Initial Catalog=" + db + ";Server=" + Server

                };
                DbConn.Open();
                mTableOwner = "[" + db + "].[dbo]";
                mDbAlias = Server;
                mIsOpen = true;
                return 0;
            }
            catch (SqlException ex)
            {

                this.IsOpen = false;
                mDbLastError = ex.Message.ToString();
                
                return -1;
            } 
            finally
            {
            }
        }

        public long DbClose()
        {
            try
            {
                mDbLastError = "";
                DbConn.Dispose();

                mIsOpen = false;

                return 0;
            }
            catch (SqlException ex)
            {
                mDbLastError = ex.Message.ToString();
                return -1;
            }
            finally
            {
            }
        }

        public long DbSelect(string Sql, ref DataTable oDt)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, DbConn);

                oDt.Clear();   // Para asegurar que el "datatable" esté vacío antes de cargarlo.
                oDt.Rows.Clear();
                oDt.Columns.Clear();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql;
                cmd.Connection = DbConn;
                cmd.CommandTimeout = 60;
                mDr = cmd.ExecuteReader();
                cmd.Dispose();

                oDt.Load(mDr);

                return 0;
            }
            catch (Exception ex)
            {
                mDbLastError = ex.Message.ToString();
                return -1;
            }
            finally
            {
                // put cleanup code here.
                mDbLastSQL = Sql;
            }
        }

        public long DbExecute(string Sql, ref int RecNum, SqlTransaction transaccion = null)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(Sql, DbConn)
                {
                    Transaction = transaccion,
                    CommandType = CommandType.Text,
                    CommandTimeout = 60
                };
                RecNum = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return 0;
            }
            catch (Exception ex)
            {
                mDbLastError = ex.Message.ToString();
                return -1;
            }
            finally
            {
                // put cleanup code here.
                mDbLastSQL = Sql;
            }
        }

        public SqlTransaction DbBegintrans()
        {
            if (mOpenTrans == 0)
            {
                mOpenTrans = 1;
                return DbConn.BeginTransaction();
            }
            else
            {
                mDbLastError = " Hay una transaccion abierta ";
                return null;
            }
        }

        public void DbRollBack(ref SqlTransaction transaccion)
        {
            transaccion.Rollback();
            mOpenTrans = 0;
        }

        public void DbCommit(ref SqlTransaction transaccion)
        {
            transaccion.Commit();
            mOpenTrans = 0;
        }

        public void SQLS()
        {
            mDbDateFormat = "dd/mm/yyyy";
            mDbTimeFormat = "hh24:mi:ss";
        }



        //public List<T> ConvertToList<T>(DataTable dt)
        //{
        //    var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
        //    var properties = typeof(T).GetProperties();
        //    return dt.AsEnumerable().Select(row =>
        //    {
        //        var objT = Activator.CreateInstance<T>();
        //        foreach (var pro in properties)
        //        {
        //            if (columnNames.Contains(pro.Name.ToLower()))
        //            {
        //                try
        //                {
        //                    pro.SetValue(objT, row[pro.Name]);
        //                }
        //                catch (Exception ex) { }
        //            }
        //        }
        //        return objT;
        //    }).ToList();
        //}


    }
}

