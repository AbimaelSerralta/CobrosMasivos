using Franquicia.DataAccess.Common;
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
    }
}
