using Franquicia.Bussiness;
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
    public class ConsultaReferenciaController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ConsultarRef(string r)
        {
            ColegiaturasServices colegiaturasServices = new ColegiaturasServices();

            DecodificarService decodificarService = new DecodificarService();

            HeaderClubPagoServices headerClubPagoServices = new HeaderClubPagoServices();
            HeaderClubPago headerClubPago = headerClubPagoServices.ObtenerHeaderClubPago();

            #region ValidacionHeader

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
            #endregion



            if (colegiaturasServices.ObtenerEstatusColegiatura(r))
            {
                DateTime HoraDelServidor = DateTime.Now;
                DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                ConsultarReferenciasServices consultarReferenciasServices = new ConsultarReferenciasServices();
                ConsultarReferencia consultarReferecia = consultarReferenciasServices.ConsultarReferenciaClubPago(r, thisDay);

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, consultarReferecia);
            }
            else
            {
                ConsultarReferencia consultarReferencia = new ConsultarReferencia();

                consultarReferencia.codigo = 50;
                consultarReferencia.mensaje = "Error de sistema";
                consultarReferencia.monto = "0";
                consultarReferencia.referencia = r;
                consultarReferencia.transaccion = "";
                consultarReferencia.parcial = false;

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, consultarReferencia);
            }
        }
    }
}