using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class ReferenciasClubPagoRepository : SqlDataRepository
    {
        ReferenciasClubPago _referenciasClubPago = new ReferenciasClubPago();
        public ReferenciasClubPago referenciasClubPago
        {
            get { return _referenciasClubPago; }
            set { _referenciasClubPago = value; }
        }

        #region Metodos Escuela

        #region Pagos padres
        public bool GenerarReferenciaPagosColegiatura(Guid UidReferencia, string VchFolio, string VchUrl, string VchConcepto, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchAsunto, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ReferenciaRegistrarPagosPadres";

                comando.Parameters.Add("@UidReferencia", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidReferencia"].Value = UidReferencia;

                comando.Parameters.Add("@VchFolio", SqlDbType.VarChar);
                comando.Parameters["@VchFolio"].Value = VchFolio;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = VchUrl;

                comando.Parameters.Add("@VchConcepto", SqlDbType.VarChar);
                comando.Parameters["@VchConcepto"].Value = VchConcepto;

                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = UidUsuario;

                comando.Parameters.Add("@VchIdentificador", SqlDbType.VarChar);
                comando.Parameters["@VchIdentificador"].Value = VchIdentificador;

                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = DtRegistro;

                comando.Parameters.Add("@DtVencimiento", SqlDbType.DateTime);
                comando.Parameters["@DtVencimiento"].Value = DtVencimiento;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = DcmImporte;

                comando.Parameters.Add("@VchAsunto", SqlDbType.VarChar, 50);
                comando.Parameters["@VchAsunto"].Value = VchAsunto;

                comando.Parameters.Add("@UidPagoColegiatura", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPagoColegiatura"].Value = UidPagoColegiatura;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public string ReimprimirReferenciaPagoColegiatura(Guid UidPagoColegiatura)
        {
            string result = "";

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchUrl from ReferenciasClubPago where UidPagoColegiatura = '" + UidPagoColegiatura + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["VchUrl"].ToString();
            };

            return result;
        }
        #endregion

        #endregion
    }
}
