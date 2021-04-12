using Franquicia.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PagaLaEscuela.Controllers.SimuladorIntegracion
{
    public class PagoCancelaController : ApiController
    {
        [HttpDelete]
        public HttpResponseMessage Cancelar([FromBody] DataRequest dataRequest)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            decimal monto = dataRequest.monto / 100;
            string fecha = dataRequest.fecha.ToString("dd/MM/yyyy");

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, ConsultarReferenciaClubPago(dataRequest.transaccion, fecha, monto, dataRequest.referencia, dataRequest.autorizacion));
        }

        public class DataRequest
        {
            public string referencia { get; set; }
            public DateTime fecha { get; set; }
            public decimal monto { get; set; }
            public string transaccion { get; set; }
            public int autorizacion { get; set; }
        }

        public class CancelacionResp
        {
            public int codigo { get; set; }
            public string mensaje { get; set; }
        }

        public CancelacionResp ConsultarReferenciaClubPago(string Transaccion, string Fecha, decimal Monto, string Referencia, int Autorizacion)
        {
            SqlDataRepository sqlDataRepository = new SqlDataRepository();

            CancelacionResp cancelacionPagoResp = new CancelacionResp();

            int codigo = 61;
            string mensaje = "Cancelación fallida";

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from RefPagosClubPago rpcp where rpcp.VchTransaccion = '" + Transaccion + "' and rpcp.DcmMonto = '" + Monto + "' and rpcp.IdReferencia = '" + Referencia + "' and rpcp.VchAutorizacion = '" + Autorizacion + "'";

                DataTable dt = sqlDataRepository.Busquedas(query);

                string DtFechaOperacion = "";

                foreach (DataRow item in dt.Rows)
                {
                    DtFechaOperacion = DateTime.Parse(item["DtFechaOperacion"].ToString()).ToString("dd/MM/yyyy");
                }

                if (DateTime.Parse(DtFechaOperacion) == DateTime.Parse(Fecha))
                {
                    codigo = 0;
                    mensaje = "Cancelación exitosa";
                }
                else
                {
                    codigo = 60;
                    mensaje = "Cancelación fuera de periodo";
                }
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            cancelacionPagoResp.codigo = codigo;
            cancelacionPagoResp.mensaje = mensaje;

            return cancelacionPagoResp;
        }
    }
}