using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using TrueketeaAdmin.Services.Security;

namespace TrueketeaApp.Services.Messages
{
    public class EmailMessages: Warnings
    {
        private string ToAddresses;
        public string Password { get; set; }
        public string EmailError;

        //Creacion del objeto mensaje
        private MimeMessage message = new MimeMessage();
        private readonly Encrypting Encrypt = new Encrypting();
        
        public EmailMessages(string _ToAddress)
        {
            ToAddresses = _ToAddress;
        }

        private void Error_Messaje(string text)
        {

            //Direccion de Envio
            message.From.Add(new MailboxAddress("NotResponding Trueketea", "trueketeaadm@gmail.com"));

            //Direccion de destino
            message.To.Add(MailboxAddress.Parse(ToAddresses));

            //Subject del Mensaje
            message.Subject = "Error App Trueketea";


            //Texto del Mensaje
            message.Body = new TextPart("plain")
            {
                Text = text
            };
        }

        private void Report_Messaje(string text)
        {

            //Direccion de Envio
            message.From.Add(new MailboxAddress("NotResponding Trueketea", "trueketeaadm@gmail.com"));

            //Direccion de destino
            message.To.Add(MailboxAddress.Parse(ToAddresses));

            //Subject del Mensaje
            message.Subject = "Reporte de incidencia";


            //Texto del Mensaje
            message.Body = new TextPart("plain")
            {
                Text = text
            };
        }

        private void Recovery_Messaje()
        {

            //Direccion de Envio
            message.From.Add(new MailboxAddress("NotResponding Trueketea", "trueketeaadm@gmail.com"));

            //Direccion de destino
            message.To.Add(MailboxAddress.Parse(ToAddresses));

            //Subject del Mensaje
            message.Subject = "Recuperacion de contraseña";


            //Texto del Mensaje
            message.Body = new TextPart("plain")
            {
                Text = @"Este es el sistema de recuperacion de contraseñas de Trueketea.
                        Su neuva contraseña es: Temporal1." +
                        "\nSe recomienda cambiar esta contraseña una vez se haya iniciado la sesión."
            };
        }

        private void validation_Messaje(string text)
        {

            //Direccion de Envio
            message.From.Add(new MailboxAddress("NotResponding Trueketea", "trueketeaadm@gmail.com"));

            //Direccion de destino
            message.To.Add(MailboxAddress.Parse(ToAddresses));

            //Subject del Mensaje
            message.Subject = "Verificación de cuenta";


            //Texto del Mensaje
            message.Body = new TextPart("plain")
            {
                Text = @"Este es el sistema de recuperacion de verificacion de cuentas de Trueketea.
                        este es el codigo de verificación:" + text +
                        "\nIntroduce el codigo en las casillas de verificación para validar la cuenta."
            };
        }

        public bool Send_Email(int typeMessage,string text ="")
        {
            SmtpClient Send = new SmtpClient();
            try
            {
                switch(typeMessage)
                {
                    case 0:
                        Password = Encrypt.Encrypt("Temporal1", ToAddresses);
                        this.Recovery_Messaje();
                        break;
                    case 1:
                        this.Error_Messaje(text);
                        break;
                    case 2:
                        this.validation_Messaje(text);
                        break;
                    case 3:
                        this.Report_Messaje(text);
                        break;

                }

                Send.Connect("smtp.gmail.com", 465, true);
                string pass = Encrypt.Decrypt("WQrx2v79QPc+H72JNlEd8A==", "trueketeaadm@gmail.com");
                Send.Authenticate("trueketeaadm@gmail.com", pass);
                Send.Send(message);

                return true;
            }

            catch (Exception ex)
            {
                EmailError = ex.Message;
                return false;
            }
            finally
            {
                Send.Disconnect(true);
                Send.Dispose();
            }


        }


    }
}
