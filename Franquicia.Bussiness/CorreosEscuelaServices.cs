using Franquicia.DataAccess.Repository;
using Franquicia.Domain;
using Franquicia.Domain.Models;
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
    public class CorreosEscuelaServices
    {
        //private ClientesRepository _clientesRepository = new ClientesRepository();
        //public ClientesRepository clientesRepository
        //{
        //    get { return _clientesRepository; }
        //    set { _clientesRepository = value; }
        //}


        //string Host = "mail.compuandsoft.com";
        //string EmailFrom = "website@compuandsoft.com";
        //string Password = "+K7;v0{?SgVX";
        //bool IsBodyHtml = true;
        //bool EnableSsl = false;
        //bool UseDefaultCredentials = true;
        //int Port = 587;

        string Host = "smtpout.secureserver.net";
        string EmailFrom = "ventas@pagalaescuela.mx";
        string Password = "P4gal4e5cu3l@.MX";
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
        public string CorreoEnvioPagoColegiatura(List<PagosColegiaturas> lsPagosColegiaturas, List<DetallesPagosColegiaturas> lsDetallesPagosColegiaturas, string Asunto, string Referencia, DateTime FHPago, string TarjetaPago, string Folio, string Correo, Guid UidCliente)
        {
            string mnsj = string.Empty;
            string VchDesglose = string.Empty;

            try
            {
                foreach (var item in lsDetallesPagosColegiaturas.OrderBy(x => x.IntNum))
                {
                    string negativo = "#222";
                    if (item.DcmImporte < 0)
                    {
                        negativo = "#f55145";
                    }

                    VchDesglose +=
                        "\t\t\t\t\t\t\t\t<tr>\r\n" +
                        "\t\t\t\t\t\t\t\t\t<td style=\"border-bottom:1px solid #ddd;\" bgcolor=\"#ffffff\" align =\"center\">" + item.IntNum + " </td>\r\n" +
                        "\t\t\t\t\t\t\t\t\t<td style=\"border-bottom:1px solid #ddd;\" bgcolor=\"#ffffff\">" + item.VchDescripcion + "</td>\r\n" +
                        "\t\t\t\t\t\t\t\t\t<td style=\"border-bottom:1px solid #ddd;color:" + negativo + ";\" bgcolor=\"#ffffff\" align=\"right\"> $" + item.DcmImporte + " </td>\r\n" +
                        "\t\t\t\t\t\t\t\t</tr>\r\n";
                }

                //Se crea el contenido del correo eletronico
                var html = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/PlantillasHtml/CorreoEnvioPagoColegiatura.html"));

                //byte[] imagen = clientesRepository.CargarLogo(UidCliente);

                //if (imagen != null)
                //{
                //    string j = "<img height=\"100\" width=\"150\" src=\"data:image/png;base64,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\" alt=\"LogoEscuela\"/>";

                //    string h = "<img height=\"100\" width=\"150\" src=\"data:image/png;base64,"+ Convert.ToBase64String(imagen) + '"' + " alt=\"LogoEscuela\"/>\r\n";

                //    html = html.Replace("{LogoEscuela}", j);
                //    html = html.Replace("{ByteImagen}", Convert.ToBase64String(imagen));
                //}
                //else
                //{
                //    html = html.Replace("{LogoEscuela}", "https://pagalaescuela.mx/images/SinLogo2.png");
                //}

                html = html.Replace("{trSubtotal}", "display:none;");
                html = html.Replace("{trComicion}", "display:none;");
                html = html.Replace("{trPromocion}", "display:none;");
                html = html.Replace("{trDetallePromociones}", "display:none;");

                foreach (var item in lsPagosColegiaturas)
                {
                    //Asigancion de parametros
                    html = html.Replace("{Alumno}", item.VchAlumno);
                    html = html.Replace("{Matricula}", item.VchMatricula);
                    html = html.Replace("{FHPago}", item.DtFHPago.ToString("dd/MM/yyyy"));

                    if (item.BitSubtotal)
                    {
                        html = html.Replace("{Subtotal}", item.DcmSubtotal.ToString("N2"));
                        html = html.Replace("{trSubtotal}", "");
                    }

                    if (item.BitComisionBancaria)
                    {
                        html = html.Replace("{ComisionBancaria}", item.VchComisionBancaria);
                        html = html.Replace("{ImpComisionBancaria}", item.DcmComisionBancaria.ToString("N2"));
                        html = html.Replace("{trComicion}", "");
                    }

                    if (item.BitPromocionDePago)
                    {
                        html = html.Replace("{Promocion}", item.VchPromocionDePago);
                        html = html.Replace("{ImpPromocion}", item.DcmPromocionDePago.ToString("N2"));
                        html = html.Replace("{trPromocion}", "");

                        string dPromo = item.VchPromocionDePago.Replace("COMISIÓN ", "").Replace(" MESES:", "");

                        html = html.Replace("{DetallePromocion}", dPromo.Trim() + " pagos mensuales de:");
                        html = html.Replace("{impDetallePromocion}", (item.DcmTotal / decimal.Parse(dPromo.Trim())).ToString("N2"));
                        html = html.Replace("{trDetallePromociones}", "");

                        
                    }

                    html = html.Replace("{Total}", item.DcmTotal.ToString("N2"));

                }
                html = html.Replace("{Desglose}", VchDesglose);
                
                html = html.Replace("{OpeReferencia}", Referencia);
                html = html.Replace("{OpeFecha}", FHPago.ToLongDateString());
                html = html.Replace("{OpeHora}", FHPago.ToString("HH:mm:ss"));
                html = html.Replace("{OpeTarjeta}", TarjetaPago);
                html = html.Replace("{OpeFolio}", Folio);
                
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
                var from = new EmailAddress("ventas@pagalaescuela.mx", "PagaLaEscuela");
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

    }
}
