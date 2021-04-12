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
    public class CancelaPagoController : ApiController
    {
        [HttpDelete]
        public HttpResponseMessage CancelarRef(CancelacionPago cancelacion)
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

            //Validar el tipo de pago
            //Obtener TipoEndpoint
            string TipoEndpoint = endPointClubPagoServices.ObtenerEndPointAUtilizar(cancelacion.referencia);

            string UrlEntrega = string.Empty;
            bool Integracion = false;

            //Obtener la url de entrega
            switch (TipoEndpoint)
            {
                case "SANDBOX":
                    var resSand = endPointClubPagoServices.ObtenerEndPointClubPagoSandbox(cancelacion.referencia, Guid.Parse("D8903F55-B478-452D-ACC9-A7F0524C687B"));

                    if (resSand.Item2)
                    {
                        UrlEntrega = resSand.Item1;
                        Integracion = true;
                    }
                    break;

                case "PRODUCCION":
                    var resProd = endPointClubPagoServices.ObtenerEndPointClubPagoProduccion(cancelacion.referencia, Guid.Parse("D8903F55-B478-452D-ACC9-A7F0524C687B"));

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
                CancelacionPagoResp cancelacionPagoResp = enviarIntegracionesServices.EnviarPeticionCancelarPagoReferenciaClubPago(cancelacion, UrlEntrega);
                
                if (cancelacionPagoResp.codigo == 0 && !string.IsNullOrEmpty(cancelacionPagoResp.mensaje))
                {
                    Guid UidPagoIntegracion = Guid.Empty;
                    decimal Importe = 0;
                    decimal ImportePagado = 0;
                    decimal ImporteNuevo = 0;

                    foreach (var item in pagosIntegracionServices.ObtenerPagoClubPagoIntegracion(cancelacion.referencia))
                    {
                        UidPagoIntegracion = item.UidPagoIntegracion;
                        Importe = item.DcmImporte;
                        ImportePagado = item.DcmImportePagado;
                        ImporteNuevo = item.DcmImporteNuevo;
                    }

                    ImportePagado = ImportePagado - cancelacion.monto / 100;
                    ImporteNuevo = ImporteNuevo + cancelacion.monto / 100;

                    DateTime HoraDelServidor = DateTime.Now;
                    DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                    if (refPagosClubPagoServices.EliminarPagoClubPago(cancelacion.autorizacion, cancelacion.monto / 100, cancelacion.transaccion, cancelacion.referencia))
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

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, cancelacionPagoResp);
            }
            else
            {

                CancelacionPagoServices cancelacionPagoServices = new CancelacionPagoServices();

                decimal monto = cancelacion.monto / 100;
                string fecha = cancelacion.fecha.ToString("dd/MM/yyyy");

                CancelacionPagoResp cancelacionPagoResp = cancelacionPagoServices.CancelacionPagoClubPago(cancelacion.transaccion, fecha, monto, cancelacion.referencia, cancelacion.autorizacion);

                //string json = JsonConvert.SerializeObject(cancelacionPago);

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, cancelacionPagoResp);
            }
        }
    }
}