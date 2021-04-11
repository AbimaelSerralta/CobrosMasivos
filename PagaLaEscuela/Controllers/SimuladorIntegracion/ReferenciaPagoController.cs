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
    public class ReferenciaPagoController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RealizarPago([FromBody] DataRequest dataRequest)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime thisDay = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            string para1 = dataRequest.referencia;
            string para2 = dataRequest.fecha;
            decimal para3 = decimal.Parse(dataRequest.monto) / 100;
            string para4 = dataRequest.transaccion;

            return Request.CreateResponse(System.Net.HttpStatusCode.OK, AutorizacionPagoClubPago(para1, thisDay, para2, para3, para4));
        }

        public class DataRequest
        {
            public string referencia { get; set; }
            public string fecha { get; set; }
            public string monto { get; set; }
            public string transaccion { get; set; }
        }

        public class AutPago
        {
            public int codigo { get; set; }
            public string autorizacion { get; set; }
            public string mensaje { get; set; }
            public string transaccion { get; set; }
            public string fecha { get; set; }
            public string notificacion_sms { get; set; }
            public string mensaje_sms { get; set; }
            public string mensaje_ticket { get; set; }
        }

        public AutPago AutorizacionPagoClubPago(string IdReferencia, DateTime FechaRegistro, string Fecha, decimal Monto, string Transaccion)
        {
            SqlDataRepository sqlDataRepository = new SqlDataRepository();

            AutPago autorizacionPago = new AutPago();

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select rcp.*, pain.DcmImporteNuevo from RefClubPago rcp, PagosIntegracion pain where rcp.UidPagoIntegracion = pain.UidPagoIntegracion and IdReferencia = '" + IdReferencia + "'";

                DataTable dt = sqlDataRepository.Busquedas(query);

                int codigo = 0;
                string mnsj = "Transaccion Exitosa";
                decimal monto = 0;
                string referencia = "";
                string autorizacion = "";

                if (dt.Rows.Count == 0)
                {
                    codigo = 40;
                    mnsj = "Adquiriente inválido";
                    monto = 0;
                }
                else
                {
                    string Vencimiento = "";
                    Guid UidPagoIntegracion = Guid.Empty;

                    foreach (DataRow item in dt.Rows)
                    {
                        monto = decimal.Parse(item["DcmImporteNuevo"].ToString());
                        referencia = item["IdReferencia"].ToString();
                        Vencimiento = item["DtVencimiento"].ToString();
                        UidPagoIntegracion = Guid.Parse(item["UidPagoIntegracion"].ToString());
                    }

                    if (DateTime.Parse(DateTime.Parse(Fecha).ToString("dd-MM-yyyy")) > DateTime.Parse(Vencimiento))
                    {
                        codigo = 14;
                        mnsj = "Referencia fuera de vigencia";
                        monto = 0;
                        autorizacion = "";
                    }
                    else
                    {
                        if (Monto <= monto)
                        {
                            // Aqui se registraria el pago
                            var correct = ConsultarPagoClubPago();

                            if (correct.Item2)
                            {
                                autorizacion = correct.Item1.ToString();
                                Guid UidPago = Guid.NewGuid();
                            }
                            else
                            {
                                codigo = 50;
                                mnsj = "Error de sistema.";
                                monto = 0;
                                autorizacion = "";
                            }
                        }
                        else
                        {
                            codigo = 30;
                            mnsj = "Monto inválido";
                            monto = 0;
                            autorizacion = "";
                        }
                    }
                }

                autorizacionPago.codigo = codigo;
                autorizacionPago.autorizacion = autorizacion;
                autorizacionPago.mensaje = mnsj;
                //autorizacionPago.monto = Math.Truncate(monto * 100).ToString();
                autorizacionPago.transaccion = Transaccion;
                autorizacionPago.fecha = Fecha;
                autorizacionPago.notificacion_sms = "";
                autorizacionPago.mensaje_sms = "";
                autorizacionPago.mensaje_ticket = "";

            }
            catch (Exception ex)
            {
                string mns = ex.Message;

                autorizacionPago.codigo = 50;
                autorizacionPago.autorizacion = "";
                autorizacionPago.mensaje = "Error de sistema.";
                autorizacionPago.transaccion = Transaccion;
                autorizacionPago.fecha = Fecha;
                autorizacionPago.notificacion_sms = "";
                autorizacionPago.mensaje_sms = "";
                autorizacionPago.mensaje_ticket = "";
            }

            return autorizacionPago;
        }
        public Tuple<int, bool> ConsultarPagoClubPago()
        {
            SqlDataRepository sqlDataRepository = new SqlDataRepository();

            int result = 1;
            bool resbool = false;

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select CASE WHEN MAX(VchAutorizacion) IS NOT NULL THEN MAX(VchAutorizacion) ELSE 0 END VchAutorizacion from RefPagosClubPago";

                DataTable dt = sqlDataRepository.Busquedas(query);

                foreach (DataRow item in dt.Rows)
                {
                    result = int.Parse(item["VchAutorizacion"].ToString()) + 1;
                }

                resbool = true;
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            return Tuple.Create(result, resbool);
        }
    }
}