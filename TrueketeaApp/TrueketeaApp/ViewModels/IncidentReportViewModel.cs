using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.Services;
using TrueketeaApp.Services.DataBase;
using TrueketeaApp.Services.Messages;
using Xamarin.Forms;

namespace TrueketeaApp.ViewModels
{
    public class IncidentReportViewModel
    {
        Warnings wg = new Warnings();
        B8Clases B8 = new B8Clases();
        EmailMessages envio = new EmailMessages("trueketeaadm@gmail.com");
        private readonly DBContext _dbContext = ViewModelLocator.sql;
        ContentPage MyView;
        Editor reporte;
        public Command SendCommand { get; set; }
        public IncidentReportViewModel(ContentPage view)
        {
            MyView = view;
            reporte = MyView.FindByName<Editor>("ReportText");
            this.SendCommand = new Command(SendReport);
        }


        private async void SendReport()
        {
            string msg = reporte.Text;
            string sql = "";
            string thisDay = DateTime.Today.ToString("dd/MM/yyyy");
            int afectRow = 0;
            string id = B8.DBLookupEx("Internal_Emails", "Max(id_Notify) + 1", "1", "1");

            sql = $"Insert into {_dbContext.TableOwner}.Internal_Emails (id_Notify,id_Emisor,id_Receptor,Message,status_id,Read_Email,SendDate,Subject) ";
            sql = sql + $" values('{id}','{ViewModelLocator.MyId}','1','{msg}'," +
                $"'1','0',convert(datetime,{thisDay},103),'Reporte de Incidencias' )";

            if (0 != _dbContext.DbExecute(sql, ref afectRow))
            {
                B8.Log_Error("IncidentReportViewModel", "SendReport", _dbContext.DbLastError, sql);

                await wg.ToastWarning("Error al reportar el aviso.Intentalo mas tarde.", MyView);
            }
            else
            {
                envio.Send_Email(3, $"Reporte enviado por el usuario con id:{ViewModelLocator.MyId} \n {msg}");
                await wg.ToastInfo("Reporte enviado. Gracias por colaborar con nosotros.", MyView);
            }

        }



    }
}
