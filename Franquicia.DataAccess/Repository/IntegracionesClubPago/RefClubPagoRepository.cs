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
    public class RefClubPagoRepository : SqlDataRepository
    {
        RefClubPago _refClubPago = new RefClubPago();
        public RefClubPago refClubPago
        {
            get { return _refClubPago; }
            set { _refClubPago = value; }
        }

        public bool RegistrarReferencia(Guid UidReferencia, string VchFolio, int IdIntegracion, int IdEscuela, string VchUrl, string VchCodigoBarra, string VchConcepto, string IdReferencia, string VchCuenta, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchEmail, string VchUsuario, Guid UidPagoIntegracion)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ReferenciaRegistrarIntegraciones";

                comando.Parameters.Add("@UidReferencia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidReferencia"].Value = UidReferencia;

                comando.Parameters.Add("@VchFolio", SqlDbType.VarChar);
                comando.Parameters["@VchFolio"].Value = VchFolio;

                comando.Parameters.Add("@IdIntegracion", SqlDbType.Int);
                comando.Parameters["@IdIntegracion"].Value = IdIntegracion;

                comando.Parameters.Add("@IdEscuela", SqlDbType.Int);
                comando.Parameters["@IdEscuela"].Value = IdEscuela;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                comando.Parameters.Add("@VchCodigoBarra", SqlDbType.VarChar);
                comando.Parameters["@VchCodigoBarra"].Value = VchCodigoBarra;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar);
                comando.Parameters["@VchConcepto"].Value = VchConcepto;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@VchCuenta", SqlDbType.VarChar);
                comando.Parameters["@VchCuenta"].Value = VchCuenta;

                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = DtRegistro;

                comando.Parameters.Add("@DtVencimiento", SqlDbType.DateTime);
                comando.Parameters["@DtVencimiento"].Value = DtVencimiento;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;

                comando.Parameters.Add("@VchEmail", SqlDbType.VarChar);
                comando.Parameters["@VchEmail"].Value = VchEmail;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar);
                comando.Parameters["@VchUsuario"].Value = VchUsuario;
                
                comando.Parameters.Add("@UidPagoIntegracion", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoIntegracion"].Value = UidPagoIntegracion;                

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
    }
}
