using Franquicia.Bussiness;
using Franquicia.Domain.Models.ClubPago;
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
        public HttpResponseMessage GenerarRefe([FromBody]GenerarRefereciaPago generarRefereciaPago)
        {
            GenerarRefereciaClubPago generarRefereciaClubPago = new GenerarRefereciaClubPago();

            ObtenerRefereciaPago obtenerRefereciaPago = generarRefereciaClubPago.ApiGenerarReferencia(generarRefereciaPago);

            if (obtenerRefereciaPago.Message == "Success" || obtenerRefereciaPago.Message == "Exitosa")
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, obtenerRefereciaPago);
            }
            else
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, obtenerRefereciaPago);
            }
        }
    }
}