using Franquicia.Bussiness;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace Franquicia.WebForms.Controllers
{
    public class PayCardHookController : TwilioController
    {
        [HttpPost]
        public TwiMLResult Receive(SmsRequest incomingMessage)
        {
            ValidacionesServices validacionesServices = new ValidacionesServices();
            TelefonosUsuariosServices telefonosUsuariosServices = new TelefonosUsuariosServices();
            WhatsAppPendientesServices whatsAppPendientesServices = new WhatsAppPendientesServices();
            ClienteCuentaServices clienteCuentaServices = new ClienteCuentaServices();
            TicketsServices ticketsServices = new TicketsServices();
            TarifasServices tarifasServices = new TarifasServices();

            int count = incomingMessage.From.Length;

            string Number = string.Empty;

            for (int i = count - 10; i < incomingMessage.From.Length; i++)
            {
                Number += incomingMessage.From[i];
            }

            string Id = telefonosUsuariosServices.ObtenerIdTelefono(Number);

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            DateTime horParse = DateTime.Parse(hoy.ToString("dd/MM/yyyy"));

            var messagingResponse = new MessagingResponse();

            if (incomingMessage.Body.ToUpper() == "SI")
            {
                if (telefonosUsuariosServices.ActualizarEstatusWhats(Guid.Parse(Id), Guid.Parse("602D7AD6-79D0-4651-BCB5-EE1D6B9BE27A")))
                {
                    whatsAppPendientesServices.ObtenerWhatsPendiente(Number);

                    if (whatsAppPendientesServices.lsWhatsAppPendientes.Count >= 1)
                    {
                        int intMsnj = 0;

                        foreach (var item in whatsAppPendientesServices.lsWhatsAppPendientes)
                        {
                            DateTime DtVencimiento = DateTime.Parse(item.DtVencimiento.ToString("dd/MM/yyyy"));

                            if (DtVencimiento >= horParse)
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
                                    messagingResponse.Message(item.VchUrl.Replace("[n]", "\n"));

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

                                    intMsnj++;
                                }
                                else
                                {

                                }
                            }
                        }

                        if (intMsnj == 0)
                        {
                            messagingResponse.Message("Gracias por confirmar. Esto es un mensaje automatico, por favor no responda.");
                        }
                    }
                    else
                    {
                        messagingResponse.Message("Gracias por confirmar. Esto es un mensaje automatico, por favor no responda.");
                    }
                }
            }
            //else if (incomingMessage.Body.ToUpper() == "NO")
            //{
            //    telefonosUsuariosServices.ActualizarEstatusWhats(Guid.Parse(Id), Guid.Parse("A3DED1DC-765E-4CA5-A7E5-6283EF9B52C2"));
            //    messagingResponse.Message("Gracias por confirmar: NO!");
            //}

            return TwiML(messagingResponse);
        }
    }
}