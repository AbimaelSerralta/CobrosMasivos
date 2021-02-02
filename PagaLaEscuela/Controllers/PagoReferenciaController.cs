using Franquicia.Bussiness.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers
{
    public class PagoReferenciaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage PagoRef([FromBody]SolicitudPago solicitud)
        {
            DecodificarService decodificarService = new DecodificarService();
            HeaderClubPagoServices headerClubPagoServices = new HeaderClubPagoServices();
            HeaderClubPago headerClubPago = headerClubPagoServices.ObtenerHeaderClubPago();

            if (Request.Headers.Contains("X-Origin"))
            {
                string XOrigin = decodificarService.DecodeBase64ToString(Request.Headers.GetValues("X-Origin").FirstOrDefault().ToString());

                if (XOrigin != headerClubPago.XOrigin)
                {
                    ErrorHeader error = new ErrorHeader() { codigo = 1, mensaje = "Token Inválido" };
                    return Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, error);
                }
            }
            else
            {
                ErrorHeader error = new ErrorHeader() { codigo = 1, mensaje = "Token Inválido" };
                return Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized, error);
            }

            if (Request.Headers.Contains("User-Agent"))
            {
                string UserAgent = Request.Headers.GetValues("User-Agent").FirstOrDefault();

                if (UserAgent != headerClubPago.UserAgent)
                {
                    ErrorHeader error = new ErrorHeader() { codigo = 2, mensaje = "Origen Desconocido" };
                    return Request.CreateResponse(System.Net.HttpStatusCode.Forbidden, error);
                }
            }
            else
            {
                ErrorHeader error = new ErrorHeader() { codigo = 2, mensaje = "Origen Desconocido" };
                return Request.CreateResponse(System.Net.HttpStatusCode.Forbidden, error);
            }


            string para1 = solicitud.referencia;
            string para2 = solicitud.fecha;
            decimal para3 = decimal.Parse(solicitud.monto) / 100;
            string para4 = solicitud.transaccion;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            AutorizacionPagoServices autorizacionPagoServices = new AutorizacionPagoServices();

            AutorizacionPago autorizacionPago = autorizacionPagoServices.AutorizacionPagoClubPago(para1, thisDay, para2, para3, para4);


            //string json = JsonConvert.SerializeObject(autorizacionPaga);

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, autorizacionPago);
        }
    }
}