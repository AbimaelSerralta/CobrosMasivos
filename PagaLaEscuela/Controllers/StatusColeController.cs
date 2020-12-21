using Franquicia.Bussiness;
using PagaLaEscuela.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers
{
    public class StatusColeController : ApiController
    {
        [HttpPut]
        public ResponseHelpers UpdateStatusCole()
        {
            ColegiaturasServices colegiaturasServices = new ColegiaturasServices();

            var respuesta = new ResponseHelpers();
            respuesta.Message = "Ha ocurrido un error inesperado";

            int Successful = 0;
            int Error = 0;

            DateTime HoraDelServidor = DateTime.Now;
            DateTime hoy = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            colegiaturasServices.ObtenerFechaColegiaturasATM(hoy);

            if (colegiaturasServices.lsFCAATMViewModel.Count > 0)
            {
                foreach (var item in colegiaturasServices.lsFCAATMViewModel)
                {
                    //respuesta.Data += "Ini" + item.FHInicio + " Lim" + item.FHLimite + " Ven" + item.FHVencimiento + " " + item.EstatusFechaColegiatura + "<br/>";

                    if (colegiaturasServices.ActualizarEstatusFechasPagosATM(item.UidFechaColegiaturaAlumno, item.UidEstatusFechaColegiatura))
                    {
                        Successful++;
                    }
                    else
                    {
                        Error++;
                    }
                }
            }
            else
            {
                respuesta.Message = "01pm1l";
            }

            if (Error > 0 && Successful > 0) { respuesta.Message = "Successful with errors"; }

            if (Successful > 0)
            {
                respuesta.Message = "Successful";
            }
            else if (Error > 0)
            {
                respuesta.Message = "Error";
            }

            return respuesta;
        }
    }
}