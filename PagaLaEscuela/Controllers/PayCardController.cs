using Franquicia.Bussiness;
using PagaLaEscuela.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using WebApplication1.Vista;

namespace PagaLaEscuela.Controllers
{
    public class PayCardController : ApiController
    {
        [HttpPost]
        public ResponseHelpers PostPagosTarjeta([FromBody] RespuestaPago strResponse)
        {
            CorreosServices correosServices = new CorreosServices();
            CorreosEscuelaServices correosEscuelaServices = new CorreosEscuelaServices();
            ValidacionesServices validacionesServices = new ValidacionesServices();
            ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
            LigasUrlsServices ligasUrlsServices = new LigasUrlsServices();
            WhatsAppPendientesServices whatsAppPendientesServices = new WhatsAppPendientesServices();
            TicketsServices ticketsServices = new TicketsServices();
            TarifasServices tarifasServices = new TarifasServices();
            ParametrosTwiServices parametrosTwiServices = new ParametrosTwiServices();
            ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
            PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();

            strResponse.StrResponse = HttpUtility.HtmlEncode(strResponse.StrResponse);
            var respuesta = new ResponseHelpers();
            string finalString = strResponse.StrResponse.Replace("%25", "%").Replace("%20", " ").Replace("%2B", "+").Replace("%3D", "=").Replace("%2F", "/").Replace("%0D%0A", "\r\n");
            // key con produccion
            string cadena = finalString;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            try
            {
                correosServices.CorreoCadena(thisDay + " finalString " + cadena, "serralta2008@gmail.com");
            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

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
                DateTime DtFechaOperacion = thisDay;

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
                        fechaRegistro = thisDay;
                        DtFechaOperacion = thisDay;
                        cc_type = "denied";
                        auth = "denied";
                        tp_operation = "denied";
                        amount = "0";
                        break;
                    case "approved":
                        fechaRegistro = thisDay;
                        string fecha1 = DateTime.Parse(fecha + " " + Hora).ToString("dd/MM/yyyy HH:mm:ss");
                        DtFechaOperacion = DateTime.Parse(fecha1);
                        break;
                    case "error":
                        fechaRegistro = thisDay;
                        DtFechaOperacion = thisDay;
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


                        // ==> ENVIO DE CORREO DE PAGO COLEGIATURA
                        var para = pagosServices.ConsultarPagoColegiatura(reference);

                        if (!string.IsNullOrEmpty(para.Item1))
                        {
                            var list = pagosServices.ObtenerPagoColegiatura(Guid.Parse(para.Item1));

                            pagosServices.ActualizarPagoColegiaturaPLE(Guid.Parse(para.Item1), Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9"));

                            //correosEscuelaServices.CorreoEnvioPagoColegiatura(list.Item1, list.Item2, "Comprobante de pago de colegiatura", reference, fechaRegistro, "************" + cc_number, foliocpagos, para.Item2, "APROBADO", Guid.Parse(para.Item3));

                            var data = pagosServices.ConsultarDatosValidarPago(Guid.Parse(para.Item1));

                            Guid UidClienteLocal = Guid.Parse(data.Item1);
                            Guid UidUsuario = Guid.Parse(data.Item2);
                            Guid UidFechaColegiatura = Guid.Parse(data.Item3);
                            Guid UidAlumno = Guid.Parse(data.Item4);

                            //Necesito saber el importe de la colegiatura
                            decimal ImporteCole = colegiaturasServices.ObtenerDatosFechaColegiatura(UidClienteLocal, UidUsuario, UidFechaColegiatura, UidAlumno);

                            //Necesito saber el importe de todos los pagos
                            decimal ImportePagado = pagosColegiaturasServices.ObtenerPagosPadresRLE(UidFechaColegiatura, UidAlumno);
                            decimal ImportePendiente = pagosColegiaturasServices.ObtenerPendientesPadresRLE(UidFechaColegiatura, UidAlumno);

                            // ==>Validar con importe<==
                            if (ImporteCole == ImportePagado) //el importeColegiatura es igual al importe de todos los pagos con estatus aprobado
                            {
                                //Se cambia el estatus de la colegiatura a pagado.
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("605A7881-54E0-47DF-8398-EDE080F4E0AA"), Guid.Parse("80EAC55B-8363-4FA3-86A5-430971F0E0E6"), true);
                            }
                            else if (ImporteCole == (ImportePagado + ImportePendiente)) //el importe de los pagos aprobado y pendiente es igual al importe la colegiatura
                            {
                                // La colegiatura mantiene el estatus en proceso
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse("5554CE57-1288-46D5-B36A-8AC69CB94B9A"), Guid.Parse("80EAC55B-8363-4FA3-86A5-430971F0E0E6"), true);
                            }
                            else
                            {
                                Guid UidEstatusColeAlumnos = Guid.Parse("CEB03962-B62D-42F4-A299-582EB59E0D75");

                                if ((ImportePagado + ImportePendiente) != 0)
                                {
                                    UidEstatusColeAlumnos = Guid.Parse("2545BF35-F4D4-4D18-8234-8AB9D8A4ECB8");
                                }

                                // La colegiatura regresa al ultimo estatus
                                DateTime HoraServidor = DateTime.Now;
                                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                                string UidEstatus = colegiaturasServices.ObtenerEstatusColegiaturasRLE(hoy, UidFechaColegiatura, UidAlumno);
                                colegiaturasServices.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, Guid.Parse(UidEstatus.ToString()), UidEstatusColeAlumnos, false);
                            }

                        }

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

                                DateTime HoraDelServidor2 = DateTime.Now;
                                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor2, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
                                DateTime horParse = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

                                //if (whatsAppPendientesServices.lsWhatsAppPendientes.Count >= 1)
                                //{
                                //    ////******Configuracion de Twilio******
                                //    parametrosTwiServices.ObtenerParametrosTwi();
                                //    string accountSid = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AccountSid;
                                //    string authToken = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.AuthToken;
                                //    string NumberFrom = parametrosTwiServices.parametrosTwiRepository.parametrosTwi.NumberFrom;

                                //    //string accountSid = "ACc7561cb09df3180ee1368e40055eedf5";
                                //    ////string authToken = "0f47ce2d28c9211ac6a9ae42f630d1d6";
                                //    //string authToken = "3f914e588826df9a93ed849cee73eae2";
                                //    ////string NumberFrom = "+14158739087";
                                //    //string NumberFrom = "+14155238886";

                                //    foreach (var item in whatsAppPendientesServices.lsWhatsAppPendientes)
                                //    {
                                //        DateTime DtVencimiento = DateTime.Parse(item.DtVencimiento.ToString("dd/MM/yyyy"));

                                //        if (DtVencimiento >= horParse)
                                //        {
                                //            string prefijo = item.VchTelefono.Split('(', ')')[1];
                                //            string NumberTo = item.VchTelefono.Split('(', ')')[2];

                                //            if (prefijo == "+52")
                                //            {
                                //                prefijo = prefijo + "1";
                                //            }

                                //            try
                                //            {
                                //                tarifasServices.CargarTarifas();
                                //                clienteCuentaServices.ObtenerDineroCuentaCliente(item.UidPropietario);
                                //                decimal DcmCuenta = clienteCuentaServices.clienteCuentaRepository.clienteCuenta.DcmDineroCuenta;

                                //                decimal DcmWhatsapp = 0;
                                //                foreach (var tariWhats in tarifasServices.lsTarifasGridViewModel)
                                //                {
                                //                    DcmWhatsapp = tariWhats.DcmWhatsapp;
                                //                }

                                //                if (DcmCuenta >= DcmWhatsapp)
                                //                {
                                //                    TwilioClient.Init(accountSid, authToken);

                                //                    var message = MessageResource.Create(
                                //                    body: item.VchUrl.Replace("[n]", "\n"),
                                //                    from: new Twilio.Types.PhoneNumber("whatsapp:" + NumberFrom),
                                //                    to: new Twilio.Types.PhoneNumber("whatsapp:" + prefijo + NumberTo));

                                //                    decimal NuevoSaldo = DcmCuenta - DcmWhatsapp;

                                //                    string[] DatosUsuario = Regex.Split(validacionesServices.ObtenerDatosUsuario(item.UidUsuario, item.UidPropietario), ",");

                                //                    string IdCliente = string.Empty;
                                //                    string IdUsuario = string.Empty;

                                //                    if (DatosUsuario.Length >= 2)
                                //                    {
                                //                        IdCliente = DatosUsuario[0];
                                //                        IdUsuario = DatosUsuario[1];
                                //                    }

                                //                    string Folio = IdCliente + IdUsuario + thisDay.ToString("ddMMyyyyHHmmssfff");

                                //                    if (ticketsServices.RegistrarTicketPago(Folio, DcmWhatsapp, 0, DcmWhatsapp, item.VchDescripcion, item.UidPropietario, hoy, 1, 0, 0, DcmCuenta, DcmWhatsapp, NuevoSaldo))
                                //                    {
                                //                        clienteCuentaServices.ActualizarDineroCuentaCliente(NuevoSaldo, item.UidPropietario, "");
                                //                        whatsAppPendientesServices.ActualizarWhatsPendiente(item.UidWhatsAppPendiente, Guid.Parse("FB046B99-A9DF-4826-9EDB-E47BCE0251EA"));
                                //                    }
                                //                }
                                //            }
                                //            catch (Exception ex)
                                //            {

                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                    else
                    {
                        var para = pagosServices.ConsultarPagoColegiatura(reference);

                        if (!string.IsNullOrEmpty(para.Item1))
                        {
                            pagosServices.ActualizarPagoColegiaturaPLE(Guid.Parse(para.Item1), Guid.Parse("77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581"));
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