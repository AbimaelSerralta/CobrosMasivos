using Franquicia.Bussiness;
using Franquicia.Bussiness.IntegracionesPraga;
using Franquicia.Domain.Models.Praga;
using Franquicia.Domain.ViewModels.Praga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers.IntegracionesPraga
{
    public class GenerarLigasController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GenerarLiga([FromBody]GenerarLigaPagoIntegraciones generarLigaPagoIntegraciones)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            PagosIntegracionServices pagosIntegracionServices = new PagosIntegracionServices();
            LigasUrlsPragaIntegracionServices ligasUrlsPragaIntegracionServices = new LigasUrlsPragaIntegracionServices();
            EndPointPragaServices endPointPragaServices = new EndPointPragaServices();

            GenerarLigaPraga generarLigaPraga = new GenerarLigaPraga();
            UrlV3PaymentResponse urlV3PaymentResponse = generarLigaPraga.ApiGenerarLiga(generarLigaPagoIntegraciones);
            
            if (urlV3PaymentResponse.code == "success")
            {
                DateTime HoraDelServidor = DateTime.Now;
                DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

                Guid UidPagoIntegracion = Guid.NewGuid();

                if (pagosIntegracionServices.RegistrarPagosIntegracion(UidPagoIntegracion, int.Parse(generarLigaPagoIntegraciones.schoolID), generarLigaPagoIntegraciones.ammount, 0, generarLigaPagoIntegraciones.ammount, Guid.Parse("310F3557-682A-4144-9433-E47E48805D28"), Guid.Parse("F25E4AAB-6044-46E9-A575-98DCBCCF7604"), Guid.Parse(generarLigaPagoIntegraciones.UidTipoPagoIntegracion)))
                {
                    if (ligasUrlsPragaIntegracionServices.RegistrarLiga(int.Parse(generarLigaPagoIntegraciones.integrationID), int.Parse(generarLigaPagoIntegraciones.schoolID), int.Parse(generarLigaPagoIntegraciones.businessId), urlV3PaymentResponse.url, generarLigaPagoIntegraciones.description, generarLigaPagoIntegraciones.reference, hoy, DateTime.Parse(generarLigaPagoIntegraciones.effectiveDate), generarLigaPagoIntegraciones.ammount, generarLigaPagoIntegraciones.paymentTypes, UidPagoIntegracion))
                    {
                        //Obtener la url de entrega
                        var resSand = endPointPragaServices.ObtenerEndPointPragaSandbox(int.Parse(generarLigaPagoIntegraciones.integrationID), generarLigaPagoIntegraciones.user, generarLigaPagoIntegraciones.password);

                        string UrlEntrega = string.Empty;

                        if (resSand.Item2)
                        {
                            UrlEntrega = resSand.Item1;
                        }
                        else
                        {
                            var resProd = endPointPragaServices.ObtenerEndPointPragaProduccion(int.Parse(generarLigaPagoIntegraciones.integrationID), generarLigaPagoIntegraciones.user, generarLigaPagoIntegraciones.password);

                            if (resProd.Item2)
                            {
                                UrlEntrega = resProd.Item1;
                            }
                        }

                        generarLigaPraga.EnviarRespuesta(urlV3PaymentResponse, UrlEntrega);
                    }
                }

                return Request.CreateResponse(System.Net.HttpStatusCode.OK, urlV3PaymentResponse);
            }
            else
            {
                //Obtener la url de entrega
                var resSand = endPointPragaServices.ObtenerEndPointPragaSandbox(int.Parse(generarLigaPagoIntegraciones.integrationID), generarLigaPagoIntegraciones.user, generarLigaPagoIntegraciones.password);

                string UrlEntrega = string.Empty;

                if (resSand.Item2)
                {
                    UrlEntrega = resSand.Item1;
                }
                else
                {
                    var resProd = endPointPragaServices.ObtenerEndPointPragaProduccion(int.Parse(generarLigaPagoIntegraciones.integrationID), generarLigaPagoIntegraciones.user, generarLigaPagoIntegraciones.password);

                    if (resProd.Item2)
                    {
                        UrlEntrega = resProd.Item1;
                    }
                }
                generarLigaPraga.EnviarRespuesta(urlV3PaymentResponse, UrlEntrega);
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, urlV3PaymentResponse);
            }

        }
    }
}