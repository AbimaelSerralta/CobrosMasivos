using Franquicia.Bussiness;
using Franquicia.Bussiness.ClubPago;
using Franquicia.Bussiness.IntegracionesClubPago;
using Franquicia.Bussiness.IntegracionesPraga;
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

            EndPointClubPagoServices endPointClubPagoServices = new EndPointClubPagoServices();
            EnviarIntegracionesServices enviarIntegracionesServices = new EnviarIntegracionesServices();
            PagosIntegracionServices pagosIntegracionServices = new PagosIntegracionServices();
            RefPagosClubPagoServices refPagosClubPagoServices = new RefPagosClubPagoServices();

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

            string para1 = solicitud.referencia;
            string para2 = solicitud.fecha;
            decimal para3 = decimal.Parse(solicitud.monto) / 100;
            string para4 = solicitud.transaccion;

            //Validar el tipo de pago
            //Obtener TipoEndpoint
            string TipoEndpoint = endPointClubPagoServices.ObtenerEndPointAUtilizar(para1);

            string UrlEntrega = string.Empty;
            bool Integracion = false;

            //Obtener la url de entrega
            switch (TipoEndpoint)
            {
                case "SANDBOX":
                    var resSand = endPointClubPagoServices.ObtenerEndPointClubPagoSandbox(para1, Guid.Parse("65341240-E22B-49B4-88B5-792B80E13F97"));

                    if (resSand.Item2)
                    {
                        UrlEntrega = resSand.Item1;
                        Integracion = true;
                    }
                    break;

                case "PRODUCCION":
                    var resProd = endPointClubPagoServices.ObtenerEndPointClubPagoProduccion(para1, Guid.Parse("65341240-E22B-49B4-88B5-792B80E13F97"));

                    if (resProd.Item2)
                    {
                        UrlEntrega = resProd.Item1;

                        Integracion = true;
                    }
                    break;
            }

            //Validar el tipo de pago
            if (Integracion)
            {
                AutorizacionPago autorizacionPago = enviarIntegracionesServices.EnviarPeticionPagoReferenciaClubPago(solicitud, UrlEntrega);

                if (autorizacionPago.codigo == 0)
                {
                    Guid UidPagoIntegracion = Guid.Empty;
                    decimal Importe = 0;
                    decimal ImportePagado = 0;
                    decimal ImporteNuevo = 0;

                    foreach (var item in pagosIntegracionServices.ObtenerPagoClubPagoIntegracion(para1))
                    {
                        UidPagoIntegracion = item.UidPagoIntegracion;
                        Importe = item.DcmImporte;
                        ImportePagado = item.DcmImportePagado;
                        ImporteNuevo = item.DcmImporteNuevo;
                    }

                    ImportePagado = ImportePagado + para3;
                    ImporteNuevo = Importe - ImportePagado;

                    // Guid.NewGuid(), datoPago.Item1, hoy, hoy, datoPago.Item2, "", "", Guid.Parse("A90B996E-A78E-44B2-AB9A-37B961D4FB27"

                    DateTime HoraDelServidor = DateTime.Now;
                    DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                    if (refPagosClubPagoServices.RegistrarPagoClubPago(Guid.NewGuid(), para1, hoy, hoy, para3, autorizacionPago.transaccion, autorizacionPago.autorizacion, Guid.Parse("9F512165-96A6-407F-925A-A27C2149F3B9")))
                    {
                        if (ImporteNuevo == 0)
                        {
                            pagosIntegracionServices.ActualizarPagoClubPagoIntegracion(UidPagoIntegracion, ImportePagado, ImporteNuevo, Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9"));
                        }
                        else
                        {
                            pagosIntegracionServices.ActualizarPagoClubPagoIntegracion(UidPagoIntegracion, ImportePagado, ImporteNuevo, Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"));
                        }
                    }
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, autorizacionPago);
            }
            else
            {
                DateTime HoraDelServidor = DateTime.Now;
                DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                AutorizacionPagoServices autorizacionPagoServices = new AutorizacionPagoServices();

                AutorizacionPago autorizacionPago = autorizacionPagoServices.AutorizacionPagoClubPago(para1, thisDay, para2, para3, para4);


                //string json = JsonConvert.SerializeObject(autorizacionPaga);

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, autorizacionPago);
            }
        }
    }
}