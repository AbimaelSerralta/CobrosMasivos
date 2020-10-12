using Franquicia.Domain;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Franquicia.Bussiness
{
    public class CorreosServices
    {

        //string Host = "mail.compuandsoft.com";
        //string EmailFrom = "website@compuandsoft.com";
        //string Password = "+K7;v0{?SgVX";
        //bool IsBodyHtml = true;
        //bool EnableSsl = false;
        //bool UseDefaultCredentials = true;
        //int Port = 587;

        string Host = "smtpout.secureserver.net";
        string EmailFrom = "ligas@cobrosmasivos.com";
        string Password = "C0br05m4s1v05.@";
        bool IsBodyHtml = true;
        bool EnableSsl = false;
        bool UseDefaultCredentials = true;
        int Port = 587; /*465; Con Ssl*/

        public void CorreoLiga(string Nombre, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string LigaUrl, string Correo, string strPromociones, bool boolPromociones, string NombreComercial)
        {
            try
            {
                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoLiga.html"));

                string MostrarPromociones = "display:none;";

                if (boolPromociones)
                {
                    MostrarPromociones = "display:block;";
                }

                //Asigancion de parametros
                html = html.Replace("{Nombre}", Nombre);
                html = html.Replace("{Concepto}", Concepto);
                html = html.Replace("{Importe}", "$" + Importe.ToString("N2"));
                html = html.Replace("{Vencimiento}", Vencimiento.ToString("dd/MM/yyyy"));
                html = html.Replace("{LigaUrl}", LigaUrl);
                html = html.Replace("{NombreComercial}", NombreComercial);
                html = html.Replace("{MostrarPromociones}", MostrarPromociones);
                html = html.Replace("{strPromociones}", strPromociones);

                //SendGrid
                Creden(Asunto, Correo, html);

                ApiEmail(new EmailConfiguration
                {
                    Host = Host,
                    EmailFrom = EmailFrom,
                    Password = Password,
                    IsBodyHtml = IsBodyHtml,
                    EnableSsl = EnableSsl,
                    UseDefaultCredentials = UseDefaultCredentials,
                    Port = Port,
                    BodyHtml = html,
                    EmailTo = Correo,
                    Subject = Asunto
                });


            }
            catch (Exception ex)
            {
                string Msns = ex.Message;
            }
        }
        public void CorreoLigaMultiple(string Nombre, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string LigaUrl, string Correo)
        {
            try
            {
                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoLigaMultiple.html"));

                //Asigancion de parametros
                html = html.Replace("{Nombre}", Nombre);
                html = html.Replace("{Concepto}", Concepto);
                html = html.Replace("{Importe}", "$" + Importe.ToString("N2"));
                html = html.Replace("{Vencimiento}", Vencimiento.ToString("dd/MM/yyyy"));
                html = html.Replace("{LigaUrl}", LigaUrl);

                Creden(Asunto, Correo, html);
                ApiEmail(new EmailConfiguration
                {
                    Host = Host,
                    EmailFrom = EmailFrom,
                    Password = Password,
                    IsBodyHtml = IsBodyHtml,
                    EnableSsl = EnableSsl,
                    UseDefaultCredentials = UseDefaultCredentials,
                    Port = Port,
                    BodyHtml = html,
                    EmailTo = Correo,
                    Subject = Asunto
                });
            }
            catch (Exception ex)
            {
                string Msns = ex.Message;
            }
        }
        public void CorreoCadena(string Nombre, string Correo)
        {
            try
            {
                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoCadena.html"));

                //Asigancion de parametros
                html = html.Replace("{Nombre}", Nombre);

                Creden("Cadena", Correo, html);
                ApiEmail(new EmailConfiguration
                {
                    Host = Host,
                    EmailFrom = EmailFrom,
                    Password = Password,
                    IsBodyHtml = IsBodyHtml,
                    EnableSsl = EnableSsl,
                    UseDefaultCredentials = UseDefaultCredentials,
                    Port = Port,
                    BodyHtml = html,
                    EmailTo = Correo,
                    Subject = "Cadena"
                });
            }
            catch (Exception ex)
            {
                string Msns = ex.Message;
            }
        }
        public string CorreoEnvioCredenciales(string Nombre, string Asunto, string Usuario, string Contrasenia, string LigaUrl, string Correo, string NombreComercial)
        {
            string mnsj = string.Empty;

            try
            {
                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoEnvioCredenciales.html"));

                //Asigancion de parametros
                html = html.Replace("{Nombre}", Nombre);
                html = html.Replace("{Usuario}", Usuario);
                html = html.Replace("{Contrasenia}", Contrasenia);
                html = html.Replace("{LigaUrl}", LigaUrl);
                html = html.Replace("{NombreComercial}", NombreComercial);

                Creden(Asunto, Correo, html);

                ApiEmail(new EmailConfiguration
                {
                    Host = Host,
                    EmailFrom = EmailFrom,
                    Password = Password,
                    IsBodyHtml = IsBodyHtml,
                    EnableSsl = EnableSsl,
                    UseDefaultCredentials = UseDefaultCredentials,
                    Port = Port,
                    BodyHtml = html,
                    EmailTo = Correo,
                    Subject = Asunto
                });
            }
            catch (Exception ex)
            {
                mnsj = ex.Message;
            }

            return mnsj;
        }
        public string CorreoRecoveryPassword(string Nombre, string Asunto, string Usuario, string Contrasenia, string LigaUrl, string Correo)
        {
            string mnsj = string.Empty;

            try
            {
                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoRecoveryPassword.html"));

                //Asigancion de parametros
                html = html.Replace("{Nombre}", Nombre);
                html = html.Replace("{Usuario}", Usuario);
                html = html.Replace("{Contrasenia}", Contrasenia);
                html = html.Replace("{LigaUrl}", LigaUrl);
                //html = html.Replace("{NombreComercial}", NombreComercial);

                Creden(Asunto, Correo, html);

                ApiEmail(new EmailConfiguration
                {
                    Host = Host,
                    EmailFrom = EmailFrom,
                    Password = Password,
                    IsBodyHtml = IsBodyHtml,
                    EnableSsl = EnableSsl,
                    UseDefaultCredentials = UseDefaultCredentials,
                    Port = Port,
                    BodyHtml = html,
                    EmailTo = Correo,
                    Subject = Asunto
                });
            }
            catch (Exception ex)
            {
                mnsj = ex.Message;
            }

            return mnsj;
        }

        private void Creden(string Asunto, string Correo, string html)
        {
            string msnj = string.Empty;
            try
            {
                var apiKey = "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"; //insert your Sendgrid API Key
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("ligas@cobrosmasivos.com", "CobrosMasivos");
                var subject = Asunto;
                var to = new EmailAddress(Correo);
                var plainTextContent = string.Empty;
                var htmlContent = html;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                msnj = ex.Message;
            }
        }
        private void ApiEmail(EmailConfiguration email)
        {
            string Mnsj = string.Empty;

            var url = $"http://sendgrid.gearhostpreview.com/api/Send/Email/SendEmail";
            var request = (HttpWebRequest)WebRequest.Create(url);
            //string json = $"{{\"Host\":\"" + email.Host + "\", \"EmailFrom\":\"" + email.EmailFrom + "\", \"Password\":\"" + email.Password + "\", \"IsBodyHtml\":\"" + email.IsBodyHtml + "\", \"EnableSsl\":\"" + email.EnableSsl + "\", \"UseDefaultCredentials\":\"" + email.UseDefaultCredentials + "\", \"Port\":\"" + email.Port + "\", \"BodyHtml\":\"" + email.BodyHtml + "\", \"EmailTo\":\"" + email.EmailTo + "\", \"Subject\":\"" + email.Subject + "\"}}";
            string json = JsonConvert.SerializeObject(email);
            request.Method = "post";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return;
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            Mnsj = responseBody;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Mnsj = ex.Message;
            }



            //try
            //{
            //    Uri uri = new Uri("http://cobrostesttwilio.gearhostpreview.com/api/Send/Email/SendEmail", email);
            //    WebRequest request = WebRequest.Create(uri);
            //    request.Method = "post";
            //    request.ContentLength = 0;
            //    request.Credentials = CredentialCache.DefaultCredentials;

            //    request.ContentType = "application/json";

            //    WebResponse response = request.GetResponse();
            //    StreamReader reader = new StreamReader(response.GetResponseStream());
            //    string tmp = reader.ReadToEnd();

            //    Mnsj = tmp;
            //}
            //catch (Exception ex)
            //{
            //    Mnsj = ex.Message;
            //}

        }

        private void Respa(string Nombre, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string LigaUrl, string Correo)
        {
            MailMessage mail = new MailMessage("liga@cobrosmasivos.com", Correo);
            mail.Subject = Asunto;
            //Se crea el contenido del correo eletronico

            string ho = "<!DOCTYPE html>\r\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n" +
                "<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />\r\n" +
                "<title></title>\r\n" +
                "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"/>\r\n" +
                "</head>\r\n" +
                "<body style=\"margin: 0; padding: 0;\">\r\n" +
                "\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\t\r\n" +
                "\t\t<tr>\r\n" +
                "\t\t\t<td style=\"padding: 10px 0 30px 0;\">\r\n" +
                "\t\t\t\t<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border: 1px solid #cccccc; border-collapse: collapse;\">\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td align=\"center\" bgcolor=\"#00bcd4\" style=\"padding: 40px 0 30px 0; color: #153643; font-size: 28px; font-weight: bold; font-family: Arial, sans-serif;\">\r\n" +
                "\t\t\t\t\t\t\t<a href=\"http://www.cobroscontarjeta.com/\"><img src=\"http://www.cobroscontarjeta.com/images/logo.png\" alt=\"Cobroscontarjeta\" width=\"500\" height=\"120\" style=\"display: block;\" /></a>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"text-align: center;color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<b> Hola, " + Nombre + "</b>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td colspan=\"2\" style=\"padding-top: 20px; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: center;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<h3>Le hemos enviado su liga de pago con los siguientes datos:</h3>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tConcepto:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Concepto + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tImporte:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;$" + Importe.ToString("N2") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: right;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\tVencimiento:\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td width=\"50%\" style=\"color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;text-align: left;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t &nbsp;" + Vencimiento.ToString("dd/MM/yyyy") + "\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +

                "\r\n\r\n\t\t\t\t\t\t<br/><br/><a style =\"display:block;color:#fff;font-weight:400;text-align:center;width:230px;font-size:20px;text-decoration:none;background:#28a745;margin:0 auto;padding:15px 0\" href=" + LigaUrl + "> Pagar $" + Importe.ToString("N2") + "</a>" +

                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t<tr>\r\n" +
                "\t\t\t\t\t\t<td bgcolor=\"#df5f16\" style=\"padding: 30px 30px 30px 30px;\">\r\n" +
                "\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n" +
                "\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t<td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" width=\"75%\">\r\n\t\t\t\t\t\t\t\t\t\tCobroscontarjeta &reg; Todos los derechos reservados, 2020<br/>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t<td align=\"right\" width=\"25%\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n\t\t\t\t\t\t\t\t\t\t\t<tr>\r\n\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://twitter.com/ \" style=\"color: #ffffff;\">\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://help.twitter.com/content/dam/help-twitter/brand/logo.png\" alt=\"Twitter\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-size: 0; line-height: 0;\" width=\"20\">&nbsp;</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t<td style=\"font-family: Arial, sans-serif; font-size: 12px; font-weight: bold;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t<a href=\"https://www.facebook.com/ \" style=\"color: #ffffff;\">\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t<img src=\"https://images.vexels.com/media/users/3/137253/isolated/preview/90dd9f12fdd1eefb8c8976903944c026-icono-de-facebook-logo-by-vexels.png\" alt=\"Facebook\" width=\"38\" height=\"38\" style=\"display: block;\" border=\"0\" />\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t\t</a>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t\t\t\t</table>\r\n" +
                "\t\t\t\t\t\t</td>\r\n" +
                "\t\t\t\t\t</tr>\r\n" +
                "\t\t\t\t</table>\r\n" +
                "\t\t\t</td>\r\n" +
                "\t\t</tr>\r\n" +
                "\t</table>\r\n" +
                "</body>\r\n" +
                "</html>";

            mail.Body = ho;
            mail.IsBodyHtml = true;
            //Se activa una variable del protocolo SMTP para poder enviar el correo electronico
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.cobrosmasivos.com";
            smtp.EnableSsl = false;

            //Activacion de la cuenta de la que se enviaran los correos electronicos
            NetworkCredential credenciales = new NetworkCredential("liga@cobrosmasivos.com", "L1g4m4s1va.c0m");
            // asignacion de las credenciales al protocolo smtp
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = credenciales;
            //Puerto de salida de correo electronico
            smtp.Port = 2525;
            //envio del correo electronico
            smtp.Send(mail);
        }
    }
}
