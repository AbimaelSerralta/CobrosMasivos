using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.ClubPago
{
    public class ConsultarReferenciasRepository : SqlDataRepository
    {
        public ConsultarReferencia ConsultarReferenciaClubPago(string IdReferencia, DateTime day)
        {
            ConsultarReferencia consultarReferencia = new ConsultarReferencia();

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                //No tiene estatus del pago query.CommandText = "select * from ReferenciasClubPago where IdReferencia = '" + IdReferencia + "'";
                query.CommandText = "select rcp.*, fp.UidEstatusFechaPago from ReferenciasClubPago rcp, FechasPagos fp where rcp.UidPagoColegiatura = fp.UidPagoColegiatura and rcp.IdReferencia = '" + IdReferencia + "'";

                DataTable dt = this.Busquedas(query);

                int codigo = 0;
                string mnsj = "Transaccion Exitosa";
                decimal monto = 0;
                string referencia = "";
                int transaccion = 0;
                bool? parcial = false;

                if (dt.Rows.Count == 0)
                {
                    codigo = 40;
                    mnsj = "Adquiriente inválido";
                    monto = 0;
                    parcial = null;
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {

                        if (Guid.Parse(item["UidEstatusFechaPago"].ToString()) != Guid.Parse("408431CA-DB94-4BAA-AB9B-8FF468A77582"))
                        {
                            var pagado = ConsultarPagoReferenciaClubPago(IdReferencia);

                            if (pagado.Item3)
                            {
                                codigo = 50;
                                mnsj = "Error de sistema";
                                monto = 0;
                                referencia = item["IdReferencia"].ToString();
                                parcial = null;
                            }
                            else
                            {
                                if (pagado.Item1 == pagado.Item2)
                                {
                                    codigo = 13;
                                    mnsj = "Referencia sin adeudo";
                                    monto = 0;
                                    referencia = item["IdReferencia"].ToString();
                                    parcial = null;
                                }
                                else
                                {
                                    if (DateTime.Parse(day.ToString("dd-MM-yyyy")) > DateTime.Parse(item["DtVencimiento"].ToString()))
                                    {
                                        codigo = 14;
                                        mnsj = "Referencia fuera de vigencia";
                                        monto = decimal.Parse(item["DcmImporte"].ToString());
                                        referencia = item["IdReferencia"].ToString();
                                        parcial = null;
                                    }
                                    else
                                    {
                                        monto = decimal.Parse(item["DcmImporte"].ToString());
                                        referencia = item["IdReferencia"].ToString();
                                        transaccion = int.Parse(item["IntFolio"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            codigo = 13;
                            mnsj = "Referencia sin adeudo";
                            monto = 0;
                            referencia = item["IdReferencia"].ToString();
                            parcial = null;
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
                consultarReferencia.parcial = null;

                return consultarReferencia;
            }
        }
        public Tuple<decimal, decimal, bool> ConsultarPagoReferenciaClubPago(string IdReferencia)
        {
            decimal Importe = 0;
            decimal Pagado = 0;
            bool Error = true;

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                //Consulta mas larga query.CommandText = "select rc.DcmImporte, CASE WHEN (select SUM(DcmMonto) from PagosClubPago pc where pc.VchEstatus = 'Aprobado' and pc.IdReferencia = rc.IdReferencia) IS NOT NULL THEN (select SUM(DcmMonto) from PagosClubPago pc where pc.VchEstatus = 'Aprobado' and pc.IdReferencia = rc.IdReferencia) ELSE 0 END MontoPagado from ReferenciasClubPago rc where rc.IdReferencia = '" + IdReferencia + "'";
                query.CommandText = "select rc.DcmImporte, (select CASE WHEN SUM(DcmMonto) IS NULL THEN 0 else SUM(DcmMonto) end MontoPagado from PagosClubPago pc where pc.UidPagoEstatus = '9F512165-96A6-407F-925A-A27C2149F3B9' and pc.IdReferencia = rc.IdReferencia) as MontoPagado from ReferenciasClubPago rc where rc.IdReferencia = '" + IdReferencia + "'";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow item in dt.Rows)
                {
                    Importe = decimal.Parse(item["DcmImporte"].ToString());
                    Pagado = decimal.Parse(item["MontoPagado"].ToString());
                    Error = false;
                }
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            return Tuple.Create(Importe, Pagado, Error);

        }
    }
}
