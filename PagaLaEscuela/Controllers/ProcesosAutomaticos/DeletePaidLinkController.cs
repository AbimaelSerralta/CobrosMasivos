using Franquicia.Bussiness;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers.ProcesosAutomaticos
{
    public class DeletePaidLinkController : ApiController
    {
        [HttpDelete]
        public HttpResponseMessage DeleteLink()
        {
            ColegiaturasServices colegiaturasServices = new ColegiaturasServices();
            PagosColegiaturasServices pagosColegiaturasServices = new PagosColegiaturasServices();

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");
            hoy = hoy.AddDays(-2);

            //Eliminar ligas de pago
            List<EliminarPagos> lsEliminarLiga = colegiaturasServices.ObtenerLigasEliminarATM(hoy);

            foreach (var item in lsEliminarLiga)
            {
                colegiaturasServices.EliminarLigaColegiatura(item.IdReferencia);
                pagosColegiaturasServices.EliminarPagoColegiatura(item.UidPagoColegiatura);
            }

            //Eliminar ligas de pago Praga
            List<EliminarPagos> lsEliminarPraga = colegiaturasServices.ObtenerLigasEliminarPragaATM(hoy);

            foreach (var item in lsEliminarPraga)
            {
                //colegiaturasServices.EliminarLigaPragaColegiatura(item.IdReferencia);
                //pagosColegiaturasServices.EliminarPagoColegiatura(item.UidPagoColegiatura);
            }


            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }
    }
}