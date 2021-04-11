using Franquicia.Bussiness;
using Franquicia.Bussiness.IntegracionesClubPago;
using Franquicia.Bussiness.IntegracionesPraga;
using Franquicia.Domain.Models.ClubPago;
using Franquicia.Domain.ViewModels.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers.IntegracionesClubPago
{
    public class GenerarReferenciaController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GenerarRefe([FromBody]GenerarRefereciaPagoIntegraciones generarRefereciaPagoIntegraciones)
        {
            PagosIntegracionServices pagosIntegracionServices = new PagosIntegracionServices();
            RefClubPagoServices refClubPagoServices = new RefClubPagoServices();
            GenerarRefereciaClubPago generarRefereciaClubPago = new GenerarRefereciaClubPago();
            EndPointClubPagoServices endPointClubPagoServices = new EndPointClubPagoServices();

            ObtenerRefereciaPago obtenerRefereciaPago = generarRefereciaClubPago.ApiGenerarReferencia(generarRefereciaPagoIntegraciones);

            if (obtenerRefereciaPago.Message == "Success" || obtenerRefereciaPago.Message == "Exitosa")
            {
                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                Guid UidPagoIntegracion = Guid.NewGuid();

                if (pagosIntegracionServices.RegistrarPagosIntegracion(UidPagoIntegracion, int.Parse(generarRefereciaPagoIntegraciones.SchoolID), decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, 0, decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, Guid.Parse("6BE13FFE-E567-4D4D-9CBC-37DA30EC23A5"), Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"), Guid.Parse(generarRefereciaPagoIntegraciones.UidTipoPagoIntegracion)))
                {
                    if (refClubPagoServices.RegistrarReferencia(obtenerRefereciaPago.Folio, int.Parse(generarRefereciaPagoIntegraciones.IntegrationID), int.Parse(generarRefereciaPagoIntegraciones.SchoolID), obtenerRefereciaPago.PayFormat, obtenerRefereciaPago.BarCode, generarRefereciaPagoIntegraciones.Description, obtenerRefereciaPago.Reference, generarRefereciaPagoIntegraciones.Account, hoy, DateTime.Parse(generarRefereciaPagoIntegraciones.ExpirationDate), decimal.Parse(generarRefereciaPagoIntegraciones.Amount) / 100, generarRefereciaPagoIntegraciones.CustomerEmail, generarRefereciaPagoIntegraciones.CustomerName, UidPagoIntegracion))
                    {
                        //Obtener la url de entrega
                        var resSand = endPointClubPagoServices.ObtenerEndPointClubPagoSandbox(int.Parse(generarRefereciaPagoIntegraciones.IntegrationID), generarRefereciaPagoIntegraciones.User, generarRefereciaPagoIntegraciones.Password);

                        string UrlEntrega = string.Empty;

                        if (resSand.Item2)
                        {
                            UrlEntrega = resSand.Item1;
                        }
                        else
                        {
                            var resProd = endPointClubPagoServices.ObtenerEndPointClubPagoProduccion(int.Parse(generarRefereciaPagoIntegraciones.IntegrationID), generarRefereciaPagoIntegraciones.User, generarRefereciaPagoIntegraciones.Password);

                            if (resProd.Item2)
                            {
                                UrlEntrega = resProd.Item1;
                            }
                        }

                        generarRefereciaClubPago.EnviarRespuesta(obtenerRefereciaPago, UrlEntrega);
                    }
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, obtenerRefereciaPago);
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, obtenerRefereciaPago);
            }
        }
    }
}