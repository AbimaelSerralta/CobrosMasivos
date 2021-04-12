using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesClubPago
{
    public class RefPagosClubPagoRepository : SqlDataRepository
    {
        public bool RegistrarPagoClubPago(Guid UidPago, string IdReferencia, DateTime FechaRegistro, DateTime FechaOperacion, decimal Monto, string Transaccion, string Autorizacion, Guid UidPagoEstatus)
        {
            bool result = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_RefPagosClubPagoRegistrar";

                comando.Parameters.Add("@UidPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPago"].Value = UidPago;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@DtFechaRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtFechaRegistro"].Value = FechaRegistro;

                comando.Parameters.Add("@DtFechaOperacion", SqlDbType.DateTime);
                comando.Parameters["@DtFechaOperacion"].Value = FechaOperacion;

                comando.Parameters.Add("@DcmMonto", SqlDbType.Decimal);
                comando.Parameters["@DcmMonto"].Value = Monto;

                comando.Parameters.Add("@VchTransaccion", SqlDbType.VarChar);
                comando.Parameters["@VchTransaccion"].Value = Transaccion;

                comando.Parameters.Add("@VchAutorizacion", SqlDbType.VarChar);
                comando.Parameters["@VchAutorizacion"].Value = Autorizacion;

                comando.Parameters.Add("@UidPagoEstatus", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoEstatus"].Value = UidPagoEstatus;

                result = this.ManipulacionDeDatos(comando);

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            return result;
        }
        public bool EliminarPagoClubPago(int Autorizacion, decimal Monto, string Transaccion, string IdReferencia)
        {
            bool result = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_RefPagosClubPagoEliminar";

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@Monto", SqlDbType.Decimal);
                comando.Parameters["@Monto"].Value = Monto;
                
                comando.Parameters.Add("@Transaccion", SqlDbType.VarChar);
                comando.Parameters["@Transaccion"].Value = Transaccion;

                comando.Parameters.Add("@Autorizacion", SqlDbType.VarChar);
                comando.Parameters["@Autorizacion"].Value = Autorizacion;

                result = this.ManipulacionDeDatos(comando);

            }
            catch (Exception ex)
            {
                string mnsj = ex.Message;
            }

            return result;
        }


        #region Metodos Web
        #region CheckReference
        public List<RefPagosClubPago> ObtenerPagoReferencia(Guid UidIntegracion, string IdReferencia)
        {
            List<RefPagosClubPago> lsRefPagosClubPago = new List<RefPagosClubPago>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select top 1 rpcp.* from RefClubPago rcp, Integraciones inte, RefPagosClubPago rpcp where rcp.IdIntegracion = inte.IdIntegracion and rcp.IdReferencia = rpcp.IdReferencia and inte.UidIntegracion = '" + UidIntegracion + "' and rcp.IdReferencia = '" + IdReferencia + "' order by rpcp.DtFechaRegistro desc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsRefPagosClubPago.Add(new RefPagosClubPago()
                {
                    UidPago = Guid.Parse(item["UidPago"].ToString()),
                    IdReferencia = item["IdReferencia"].ToString(),
                    DtFechaRegistro = DateTime.Parse(item["DtFechaRegistro"].ToString()),
                    DcmMonto = decimal.Parse(item["DcmMonto"].ToString()),
                    VchTransaccion = item["VchTransaccion"].ToString(),
                    VchAutorizacion = item["VchAutorizacion"].ToString()
                });
            }

            return lsRefPagosClubPago;
        }
        #endregion
        #endregion
    }
}
