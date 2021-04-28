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
    public class ParametrosClubPagoRepository : SqlDataRepository
    {
        private ParametrosClubPago _parametrosClubPago = new ParametrosClubPago();
        public ParametrosClubPago parametrosClubPago
        {
            get { return _parametrosClubPago; }
            set { _parametrosClubPago = value; }
        }


        #region MetodosFranquicias
        public void ObtenerParametrosClubPago()
        {
            parametrosClubPago = new ParametrosClubPago();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosClubPago";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosClubPago.UidParametro = Guid.Parse(item["UidParametro"].ToString());
                parametrosClubPago.VchUrlAuth = item["VchUrlAuth"].ToString();
                parametrosClubPago.VchUrlGenerarRef = item["VchUrlGenerarRef"].ToString();
                parametrosClubPago.VchUser = item["VchUser"].ToString();
                parametrosClubPago.VchPass = item["VchPass"].ToString();
            }
        }

        //Estos metod no se han creado
        public bool RegistrarParametrosClubPago(ParametrosSendGrid parametrosSendGrid)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosSendGridRegistrar"; //NoCreado

                comando.Parameters.Add("@VchApiKey", SqlDbType.VarChar);
                comando.Parameters["@VchApiKey"].Value = parametrosSendGrid.VchApiKey;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarParametrosClubPago(ParametrosSendGrid parametrosSendGrid)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosSendGridActualizar"; //NoCreado

                comando.Parameters.Add("@VchApiKey", SqlDbType.VarChar);
                comando.Parameters["@VchApiKey"].Value = parametrosSendGrid.VchApiKey;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region Metodos Integraciones
        public void ObtenerParametrosPragaSandbox(int IdIntegracion)
        {
            parametrosClubPago = new ParametrosClubPago();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pcpi.* from ParametrosClubPagoIntegracion pcpi, UrlGeneradoresClubPagoIntegracion ugcpi, Integraciones inte where pcpi.UidGenerador = ugcpi.UidGenerador and ugcpi.UidGenerador = inte.UidGenerador and pcpi.UidTipoPagoIntegracion = '3F792D20-B3B6-41D3-AF88-1BCB20D99BBE' and inte.IdIntegracion = '" + IdIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosClubPago.UidParametro = Guid.Parse(item["UidParametro"].ToString());
                parametrosClubPago.VchUrlAuth = item["VchUrlAuth"].ToString();
                parametrosClubPago.VchUrlGenerarRef = item["VchUrlGenerarRef"].ToString();
                parametrosClubPago.VchUser = item["VchUser"].ToString();
                parametrosClubPago.VchPass = item["VchPass"].ToString();
            }
        }
        public void ObtenerParametrosPragaProduccion(int IdIntegracion)
        {
            parametrosClubPago = new ParametrosClubPago();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pcpi.* from ParametrosClubPagoIntegracion pcpi, UrlGeneradoresClubPagoIntegracion ugcpi, Integraciones inte where pcpi.UidGenerador = ugcpi.UidGenerador and ugcpi.UidGenerador = inte.UidGenerador and pcpi.UidTipoPagoIntegracion = 'D87454C9-12EF-4459-9CED-36E8401E4033' and inte.IdIntegracion = '" + IdIntegracion + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosClubPago.UidParametro = Guid.Parse(item["UidParametro"].ToString());
                parametrosClubPago.VchUrlAuth = item["VchUrlAuth"].ToString();
                parametrosClubPago.VchUrlGenerarRef = item["VchUrlGenerarRef"].ToString();
                parametrosClubPago.VchUser = item["VchUser"].ToString();
                parametrosClubPago.VchPass = item["VchPass"].ToString();
            }
        }
        #endregion
    }
}
