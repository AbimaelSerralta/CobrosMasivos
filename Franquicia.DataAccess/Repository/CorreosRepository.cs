using Franquicia.Domain;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Franquicia.DataAccess.Repository
{
    public class CorreosRepository
    {
        private ParametrosSendGridRepository _parametrosSendGridRepository = new ParametrosSendGridRepository();
        public ParametrosSendGridRepository parametrosSendGridRepository
        {
            get { return _parametrosSendGridRepository; }
            set { _parametrosSendGridRepository = value; }
        }


        string Host = "smtpout.secureserver.net";
        string EmailFrom = "ligas@cobrosmasivos.com";
        string Password = "C0br05m4s1v05.@";
        bool IsBodyHtml = true;
        bool EnableSsl = false;
        bool UseDefaultCredentials = true;
        int Port = 587; /*465; Con Ssl*/

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

        private void Creden(string Asunto, string Correo, string html)
        {
            string msnj = string.Empty;
            try
            {
                parametrosSendGridRepository.ObtenerParametrosSendGrid();

                var apiKey = parametrosSendGridRepository.parametrosSendGrid.VchApiKey; //insert your Sendgrid API Key
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
        }
    }
}
