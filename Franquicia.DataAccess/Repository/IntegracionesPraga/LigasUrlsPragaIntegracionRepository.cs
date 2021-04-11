using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.IntegracionesPraga
{
    public class LigasUrlsPragaIntegracionRepository : SqlDataRepository
    {
        LigasUrlsPragaIntegracion _ligasUrlsPragaIntegracion = new LigasUrlsPragaIntegracion();
        public LigasUrlsPragaIntegracion ligasUrlsPragaIntegracion
        {
            get { return _ligasUrlsPragaIntegracion; }
            set { _ligasUrlsPragaIntegracion = value; }
        }

        public bool RegistrarLiga(Guid UidLigaUrl, int IdIntegracion, int IdEscuela, int IdComercio, string VchUrl, string VchConcepto, string IdReferencia, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchFormaPago, Guid UidPagoIntegracion)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_LigasUrlsPragaIntegracionRegistrar";

                comando.Parameters.Add("@UidLigaUrl", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidLigaUrl"].Value = UidLigaUrl;

                comando.Parameters.Add("@IdIntegracion", SqlDbType.Int);
                comando.Parameters["@IdIntegracion"].Value = IdIntegracion;

                comando.Parameters.Add("@IdEscuela", SqlDbType.Int);
                comando.Parameters["@IdEscuela"].Value = IdEscuela;
                
                comando.Parameters.Add("@IdComercio", SqlDbType.Int);
                comando.Parameters["@IdComercio"].Value = IdComercio;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar);
                comando.Parameters["@VchConcepto"].Value = VchConcepto;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = DtRegistro;

                comando.Parameters.Add("@DtVencimiento", SqlDbType.DateTime);
                comando.Parameters["@DtVencimiento"].Value = DtVencimiento;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;
                
                comando.Parameters.Add("@VchFormaPago", SqlDbType.VarChar);
                comando.Parameters["@VchFormaPago"].Value = VchFormaPago;
                
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
