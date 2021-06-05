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
    public class ConsuReferenController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ConsultarRef(string r)
        {
            return Request.CreateResponse(System.Net.HttpStatusCode.OK, ObtenerEndPointClubPago(r));
        }

        public class genear
        {
            public int codigo { get; set; }
            public string mensaje { get; set; }
            public string monto { get; set; }
            public string referencia { get; set; }
            public string transaccion { get; set; }
            public bool parcial { get; set; }
        }

        //public decimal ObtenerEndPointClubPago(string IdReferencia)
        //{
        //    SqlDataRepository sqlDataRepository = new SqlDataRepository();

        //    decimal monto = 0;

        //    SqlCommand query = new SqlCommand();
        //    query.CommandType = CommandType.Text;

        //    query.CommandText = "select * from RefClubPago where IdReferencia = '" + IdReferencia + "'";

        //    DataTable dt = sqlDataRepository.Busquedas(query);

        //    foreach (DataRow item in dt.Rows)
        //    {
        //        monto = decimal.Parse(item["DcmImporte"].ToString());
        //    }

        //    return monto;
        //}
        public genear ObtenerEndPointClubPago(string IdReferencia)
        {
            DateTime HoraDelServidor = DateTime.Now;
            DateTime day = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(HoraDelServidor, TimeZoneInfo.Local.Id, "Eastern Standard Time (Mexico)");

            genear consultarReferencia = new genear();

            SqlDataRepository sqlDataRepository = new SqlDataRepository();

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select pain.*, rcp.IdReferencia, rcp.DtVencimiento, rcp.IntFolio from RefClubPago rcp, PagosIntegracion pain where rcp.UidPagoIntegracion = pain.UidPagoIntegracion and rcp.IdReferencia = '" + IdReferencia + "'";

                DataTable dt = sqlDataRepository.Busquedas(query);

                int codigo = 0;
                string mnsj = "Transaccion Exitosa";
                decimal monto = 0;
                string referencia = "";
                int transaccion = 0;
                bool parcial = true;

                if (dt.Rows.Count == 0)
                {
                    codigo = 40;
                    mnsj = "Adquiriente inválido";
                    monto = 0;
                    parcial = false;
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) != Guid.Parse("8720B2B9-5712-4E75-A981-932887AACDC9"))
                        {
                            
                            if (decimal.Parse(item["DcmImporte"].ToString()) == decimal.Parse(item["DcmImportePagado"].ToString()))
                            {
                                codigo = 13;
                                mnsj = "Referencia sin adeudo";
                                monto = 0;
                                referencia = item["IdReferencia"].ToString();
                                parcial = false;
                            }
                            else
                            {
                                if (DateTime.Parse(day.ToString("dd-MM-yyyy")) > DateTime.Parse(item["DtVencimiento"].ToString()))
                                {
                                    codigo = 14;
                                    mnsj = "Referencia fuera de vigencia";
                                    monto = decimal.Parse(item["DcmImporteNuevo"].ToString());
                                    referencia = item["IdReferencia"].ToString();
                                    parcial = false;
                                }
                                else
                                {
                                    monto = decimal.Parse(item["DcmImporteNuevo"].ToString());
                                    referencia = item["IdReferencia"].ToString();
                                    transaccion = int.Parse(item["IntFolio"].ToString());
                                }
                            }
                        }
                        else
                        {
                            codigo = 13;
                            mnsj = "Referencia sin adeudo";
                            monto = 0;
                            referencia = item["IdReferencia"].ToString();
                            parcial = false;
                        }
                    }
                }

                consultarReferencia.codigo = codigo;
                consultarReferencia.mensaje = mnsj;
                consultarReferencia.monto = Math.Truncate(monto * 100).ToString();
                consultarReferencia.referencia = referencia;
                consultarReferencia.transaccion = transaccion.ToString();
                consultarReferencia.parcial = parcial;

                return consultarReferencia;
            }
            catch (Exception ex)
            {
                string mns = ex.Message;

                consultarReferencia.codigo = 50;
                consultarReferencia.mensaje = "Error de sistema";
                consultarReferencia.monto = "0";
                consultarReferencia.referencia = IdReferencia;
                consultarReferencia.transaccion = "";
                consultarReferencia.parcial = false;

                return consultarReferencia;
            }
        }
    }
}