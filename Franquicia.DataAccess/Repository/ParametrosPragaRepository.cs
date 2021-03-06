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
    public class ParametrosPragaRepository : SqlDataRepository
    {
        private ParametrosPraga _parametrosPraga = new ParametrosPraga();
        public ParametrosPraga parametrosPraga
        {
            get { return _parametrosPraga; }
            set { _parametrosPraga = value; }
        }


        #region MetodosFranquicias
        public bool ObtenerParametrosPragaBl(Guid UidPropietario)
        {
            parametrosPraga = new ParametrosPraga();
            bool Accion = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosPraga where UidPropietario = '"+ UidPropietario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosPraga.UidParametro = Guid.Parse(item["UidParametro"].ToString());
                parametrosPraga.BusinessId = item["BusinessId"].ToString();
                parametrosPraga.VchUrl = item["VchUrl"].ToString();
                parametrosPraga.UserCode = item["UserCode"].ToString();
                parametrosPraga.WSEncryptionKey = item["WSEncryptionKey"].ToString();
                parametrosPraga.APIKey = item["APIKey"].ToString();
                parametrosPraga.Currency = item["Currency"].ToString();
                parametrosPraga.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());

                Accion = true;
            }

            return Accion;
        }
        public void ObtenerParametrosPraga(Guid UidPropietario)
        {
            parametrosPraga = new ParametrosPraga();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ParametrosPraga where UidPropietario = '"+ UidPropietario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosPraga.UidParametro = Guid.Parse(item["UidParametro"].ToString());
                parametrosPraga.BusinessId = item["BusinessId"].ToString();
                parametrosPraga.VchUrl = item["VchUrl"].ToString();
                parametrosPraga.UserCode = item["UserCode"].ToString();
                parametrosPraga.WSEncryptionKey = item["WSEncryptionKey"].ToString();
                parametrosPraga.APIKey = item["APIKey"].ToString();
                parametrosPraga.Currency = item["Currency"].ToString();
                parametrosPraga.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());
            }
        }

        //Estos metod no se han creado
        public bool RegistrarParametrosPraga(ParametrosPraga parametrosPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosPragaRegistrar";

                comando.Parameters.Add("@UidParametro", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidParametro"].Value = parametrosPraga.UidParametro;
                
                comando.Parameters.Add("@BusinessId", SqlDbType.VarChar);
                comando.Parameters["@BusinessId"].Value = parametrosPraga.BusinessId;
                
                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = parametrosPraga.VchUrl;
                
                comando.Parameters.Add("@UserCode", SqlDbType.VarChar);
                comando.Parameters["@UserCode"].Value = parametrosPraga.UserCode;

                comando.Parameters.Add("@WSEncryptionKey", SqlDbType.VarChar);
                comando.Parameters["@WSEncryptionKey"].Value = parametrosPraga.WSEncryptionKey;

                comando.Parameters.Add("@APIKey", SqlDbType.VarChar);
                comando.Parameters["@APIKey"].Value = parametrosPraga.APIKey;
                
                comando.Parameters.Add("@Currency", SqlDbType.VarChar);
                comando.Parameters["@Currency"].Value = parametrosPraga.Currency;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosPraga.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        public bool ActualizarParametrosPraga(ParametrosPraga parametrosPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosPragaActualizar";

                comando.Parameters.Add("@BusinessId", SqlDbType.VarChar);
                comando.Parameters["@BusinessId"].Value = parametrosPraga.BusinessId;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = parametrosPraga.VchUrl;

                comando.Parameters.Add("@UserCode", SqlDbType.VarChar);
                comando.Parameters["@UserCode"].Value = parametrosPraga.UserCode;

                comando.Parameters.Add("@WSEncryptionKey", SqlDbType.VarChar);
                comando.Parameters["@WSEncryptionKey"].Value = parametrosPraga.WSEncryptionKey;

                comando.Parameters.Add("@APIKey", SqlDbType.VarChar);
                comando.Parameters["@APIKey"].Value = parametrosPraga.APIKey;

                comando.Parameters.Add("@Currency", SqlDbType.VarChar);
                comando.Parameters["@Currency"].Value = parametrosPraga.Currency;
                
                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosPraga.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion
    }
}
