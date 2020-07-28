using Franquicia.WebForms.Response;
using Franquicia.Bussiness;
using System;
using System.Web.Http;
using System.Xml;
using WebApplication1.Vista;
using System.Web;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Franquicia.WebForms.Controller
{
    public class PayCardController : ApiController
    {
        [HttpPost]
        public ResponseHelpers PostPagosTarjeta([FromBody] RespuestaPago strResponse)
        {
            CorreosServices correosServices = new CorreosServices();
            ValidacionesServices validacionesServices = new ValidacionesServices();
            ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
            LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
            WhatsAppPendientesServices whatsAppPendientesServices = new WhatsAppPendientesServices();
            TicketsServices ticketsServices = new TicketsServices();
            TarifasServices tarifasServices = new TarifasServices();

            strResponse.StrResponse = HttpUtility.HtmlEncode(strResponse.StrResponse);
            var respuesta = new ResponseHelpers();
            string finalString = strResponse.StrResponse.Replace("%25", "%").Replace("%20", " ").Replace("%2B", "+").Replace("%3D", "=").Replace("%2F", "/").Replace("%0D%0A", "\r\n");
            // key con produccion
            string cadena = finalString;

            correosServices.CorreoCadena(DateTime.Now + " finalString " + cadena, "serralta2008@gmail.com");

            //string key = "5DCC67393750523CD165F17E1EFADD21"; //Credenciales sanbox 
            string key = "7AACFE849FABD796F6DCB947FD4D5268";
            AESCrypto o = new AESCrypto();
            string decryptedString = o.decrypt(key, cadena);
            if (!string.IsNullOrEmpty(decryptedString))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(decryptedString);

                XmlNodeList RespuestaWebPayPlus = doc.DocumentElement.SelectNodes("//CENTEROFPAYMENTS");
                string reference = string.Empty;
                string response = string.Empty;
                string foliocpagos = string.Empty;
                string auth = string.Empty;
                string cc_type = string.Empty;
                string tp_operation = string.Empty;
                string cc_number = string.Empty;
                string amount = string.Empty;
                string fecha = string.Empty;
                string Hora = string.Empty;
                string nb_company = string.Empty;
                string bn_merchant = string.Empty;
                string id_url = string.Empty;
                string cd_error = string.Empty;
                string nb_error = string.Empty;
                string cc_mask = string.Empty;
                DateTime DtFechaOperacion = DateTime.Now;

                for (int i = 0; i < RespuestaWebPayPlus[0].ChildNodes.Count; i++)
                {
                    switch (RespuestaWebPayPlus[0].ChildNodes[i].Name)
                    {
                        case "nb_company":
                            nb_company = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "bn_merchant":
                            bn_merchant = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "id_url":
                            id_url = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "nb_error":
                            nb_error = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cd_error":
                            cd_error = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "reference":
                            reference = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "response":
                            response = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "foliocpagos":
                            foliocpagos = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "auth":
                            auth = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "date":
                            fecha = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "time":
                            Hora = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cc_type":
                            cc_type = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "tp_operation":
                            tp_operation = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;

                        case "cc_number":
                            cc_number = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "cc_mask":
                            cc_mask = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        case "amount":
                            amount = RespuestaWebPayPlus[0].ChildNodes[i].InnerText;
                            break;
                        default:
                            break;
                    }
                }
                PagosServices pagosServices = new PagosServices();

                DateTime fechaRegistro = DateTime.MinValue;
                switch (response)
                {
                    case "denied":
                        fechaRegistro = DateTime.Now;
                        DtFechaOperacion = DateTime.Now;
                        cc_type = "denied";
                        auth = "denied";
                        tp_operation = "denied";
                        amount = "0";
                        break;
                    case "approved":
                        fechaRegistro = DateTime.Now;
                        string fecha1 = DateTime.Parse(fecha + " " + Hora).ToString("dd/MM/yyyy HH:mm:ss");
                        DtFechaOperacion = DateTime.Parse(fecha1);
                        break;
                    case "error":
                        fechaRegistro = DateTime.Now;
                        DtFechaOperacion = DateTime.Now;
                        cc_type = "error";
                        auth = "error";
                        tp_operation = "error";
                        amount = "0";
                        break;
                }

                if (pagosServices.AgregarInformacionTarjeta(auth, reference, fechaRegistro, response, cc_type, tp_operation, nb_company, bn_merchant, id_url, cd_error, nb_error, cc_number, cc_mask, foliocpagos, decimal.Parse(amount), DtFechaOperacion))
                {
                    respuesta.Data = true;

                    if (response == "approved")
                    {
                        //pagosServices.ConsultarPagoEventoLiga(reference);

                        //if (pagosServices.lsLigasEventoPayCardModel.Count >= 1)
                        //{
                        //    pagosServices.ObtenerCorreoAuxiliar(reference);
                        //}

                        pagosServices.ConsultarPromocionLiga(reference);

                        if (pagosServices.lsLigasUrlsPayCardModel.Count >= 1)
                        {
                            foreach (var item in pagosServices.lsLigasUrlsPayCardModel)
                            {
                                pagosServices.AgregarInformacionTarjeta("canceled", item.IdReferencia, fechaRegistro.AddMinutes(-30), "canceled", "canceled", "canceled", "", "", "", "", "", "", "", "", decimal.Parse("0"), DtFechaOperacion.AddMinutes(-20));
                            }
                        }
                        else
                        {
                            if (validacionesServices.ValidarPagoClientePayCard(reference))
                            {
                                ligasUrlsServices.ObtenerDatosUrl(reference);

                                if (validacionesServices.ExisteCuentaDineroCliente(ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.UidPropietario))
                                {
                                    clienteCuentaServices.ObtenerDineroCuentaCliente(ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.UidPropietario);

                                    decimal NuevoSaldo = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta + ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.DcmImporte;

                                    clienteCuentaServices.ActualizarDineroCuentaCliente(NuevoSaldo, ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.UidPropietario, reference);
                                }
                                else
                                {
                                    clienteCuentaServices.RegistrarDineroCuentaCliente(ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.DcmImporte, ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.UidPropietario, reference);
                                }

                                whatsAppPendientesServices.ObtenerWhatsPntHistPago(ligasUrlsServices.ligasUrlsRepository.ligasUrlsGridViewModel.UidPropietario);

                                DateTime HoraDelServidor = DateTime.Now;
                                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                                DateTime horParse = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

                                if (whatsAppPendientesServices.lsWhatsAppPendientes.Count >= 1)
                                {
                                    ////******Configuracion de Twilio******
                                    const string accountSid = "ACcf4d1380ccb0be6d47e78a73036a29ab";
                                    ////string authToken = "30401e7bf2b7b3a2ab24c0a22203acc1";
                                    const string authToken = "915ce4d30dc09473b5ed753490436281";
                                    //var accountSid = Environment.GetEnvironmentVariable("ACcf4d1380ccb0be6d47e78a73036a29ab");
                                    //var authToken = Environment.GetEnvironmentVariable("f6b99afa9cdf4da4e685ff4837b2f911");
                                    string NumberFrom = "+14582243212";

                                    //string accountSid = "ACc7561cb09df3180ee1368e40055eedf5";
                                    ////string authToken = "0f47ce2d28c9211ac6a9ae42f630d1d6";
                                    //string authToken = "3f914e588826df9a93ed849cee73eae2";
                                    ////string NumberFrom = "+14158739087";
                                    //string NumberFrom = "+14155238886";

                                    foreach (var item in whatsAppPendientesServices.lsWhatsAppPendientes)
                                    {
                                        DateTime DtVencimiento = DateTime.Parse(item.DtVencimiento.ToString("dd/MM/yyyy"));

                                        if (DtVencimiento >= horParse)
                                        {
                                            string prefijo = item.VchTelefono.Split('(', ')')[1];
                                            string NumberTo = item.VchTelefono.Split('(', ')')[2];

                                            if (prefijo == "+52")
                                            {
                                                prefijo = prefijo + "1";
                                            }

                                            try
                                            {
                                                tarifasServices.CargarTarifas();
                                                clienteCuentaServices.ObtenerDineroCuentaCliente(item.UidPropietario);
                                                decimal DcmCuenta = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta;

                                                decimal DcmWhatsapp = 0;
                                                foreach (var tariWhats in tarifasServices.lsTarifasGridViewModel)
                                                {
                                                    DcmWhatsapp = tariWhats.DcmWhatsapp;
                                                }

                                                if (DcmCuenta >= DcmWhatsapp)
                                                {
                                                    TwilioClient.Init(accountSid, authToken);

                                                    var message = MessageResource.Create(
                                                    body: item.VchUrl.Replace("[n]", "\n"),
                                                    from: new Twilio.Types.PhoneNumber("whatsapp:" + NumberFrom),
                                                    to: new Twilio.Types.PhoneNumber("whatsapp:" + prefijo + NumberTo));

                                                    decimal NuevoSaldo = DcmCuenta - DcmWhatsapp;

                                                    string[] DatosUsuario = Regex.Split(validacionesServices.ObtenerDatosUsuario(item.UidUsuario, item.UidPropietario), ",");

                                                    string IdCliente = string.Empty;
                                                    string IdUsuario = string.Empty;

                                                    if (DatosUsuario.Length >= 2)
                                                    {
                                                        IdCliente = DatosUsuario[0];
                                                        IdUsuario = DatosUsuario[1];
                                                    }

                                                    string Folio = IdCliente + IdUsuario + DateTime.Now.ToString("ddMMyyyyHHmmssfff");

                                                    if (ticketsServices.RegistrarTicketPago(Folio, DcmWhatsapp, 0, DcmWhatsapp, item.VchDescripcion, item.UidPropietario, hoy, 1, 0, 0, DcmCuenta, DcmWhatsapp, NuevoSaldo))
                                                    {
                                                        clienteCuentaServices.ActualizarDineroCuentaCliente(NuevoSaldo, item.UidPropietario, "");
                                                        whatsAppPendientesServices.ActualizarWhatsPendiente(item.UidWhatsAppPendiente, Guid.Parse("FB046B99-A9DF-4826-9EDB-E47BCE0251EA"));
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                respuesta.Data = "Lo sentimos, no hemos podido desifrar la cadena. " + cadena;
            }
            return respuesta;
        }
    }

    public class RespuestaPago
    {
        private string _strResponse;
        public string StrResponse
        {
            get { return _strResponse; }
            set { _strResponse = value; }
        }
    }
}