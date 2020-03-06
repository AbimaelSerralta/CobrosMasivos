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
    public class ParametrosEntradaRepository : SqlDataRepository
    {
        private ParametrosEntrada _parametrosEntrada = new ParametrosEntrada();
        public ParametrosEntrada parametrosEntrada
        {
            get { return _parametrosEntrada; }
            set { _parametrosEntrada = value; }
        }


        #region MetodosFranquicias
        public void ObtenerParametrosEntrada(Guid UidFranquiciatario)
        {
            parametrosEntrada = new ParametrosEntrada();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pe.* from Franquiciatarios fr, ParametrosEntrada pe where fr.UidFranquiciatarios = pe.UidPropietario and fr.UidFranquiciatarios = '" + UidFranquiciatario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosEntrada.UidParametroEntrada = Guid.Parse(item["UidParametroEntrada"].ToString());
                parametrosEntrada.IdCompany = item["IdCompany"].ToString();
                parametrosEntrada.IdBranch = item["IdBranch"].ToString();
                parametrosEntrada.VchModena = item["VchModena"].ToString();
                parametrosEntrada.VchUsuario = item["VchUsuario"].ToString();
                parametrosEntrada.VchPassword = item["VchPassword"].ToString();
                parametrosEntrada.VchCanal = item["VchCanal"].ToString();
                parametrosEntrada.VchData0 = item["VchData0"].ToString();
                parametrosEntrada.VchUrl = item["VchUrl"].ToString();
                parametrosEntrada.VchSemillaAES = item["VchSemillaAES"].ToString();
                parametrosEntrada.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());
            }
        }

        public bool RegistrarParametrosEntrada(ParametrosEntrada parametrosEntrada)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosEntradaRegistrar";

                comando.Parameters.Add("@IdCompany", SqlDbType.VarChar, 50);
                comando.Parameters["@IdCompany"].Value = parametrosEntrada.IdCompany;

                comando.Parameters.Add("@IdBranch", SqlDbType.VarChar, 50);
                comando.Parameters["@IdBranch"].Value = parametrosEntrada.IdBranch;

                comando.Parameters.Add("@VchModena", SqlDbType.VarChar, 50);
                comando.Parameters["@VchModena"].Value = parametrosEntrada.VchModena;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = parametrosEntrada.VchUsuario;

                comando.Parameters.Add("@VchPassword", SqlDbType.VarChar, 50);
                comando.Parameters["@VchPassword"].Value = parametrosEntrada.VchPassword;

                comando.Parameters.Add("@VchCanal", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCanal"].Value = parametrosEntrada.VchCanal;

                comando.Parameters.Add("@VchData0", SqlDbType.VarChar, 50);
                comando.Parameters["@VchData0"].Value = parametrosEntrada.VchData0;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUrl"].Value = parametrosEntrada.VchUrl;

                comando.Parameters.Add("@VchSemillaAES", SqlDbType.VarChar, 50);
                comando.Parameters["@VchSemillaAES"].Value = parametrosEntrada.VchSemillaAES;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosEntrada.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool ActualizarParametrosEntrada(ParametrosEntrada parametrosEntrada)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosEntradaActualizar";

                comando.Parameters.Add("@IdCompany", SqlDbType.VarChar, 50);
                comando.Parameters["@IdCompany"].Value = parametrosEntrada.IdCompany;

                comando.Parameters.Add("@IdBranch", SqlDbType.VarChar, 50);
                comando.Parameters["@IdBranch"].Value = parametrosEntrada.IdBranch;

                comando.Parameters.Add("@VchModena", SqlDbType.VarChar, 50);
                comando.Parameters["@VchModena"].Value = parametrosEntrada.VchModena;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = parametrosEntrada.VchUsuario;

                comando.Parameters.Add("@VchPassword", SqlDbType.VarChar, 50);
                comando.Parameters["@VchPassword"].Value = parametrosEntrada.VchPassword;

                comando.Parameters.Add("@VchCanal", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCanal"].Value = parametrosEntrada.VchCanal;

                comando.Parameters.Add("@VchData0", SqlDbType.VarChar, 50);
                comando.Parameters["@VchData0"].Value = parametrosEntrada.VchData0;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUrl"].Value = parametrosEntrada.VchUrl;

                comando.Parameters.Add("@VchSemillaAES", SqlDbType.VarChar, 50);
                comando.Parameters["@VchSemillaAES"].Value = parametrosEntrada.VchSemillaAES;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosEntrada.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }
        #endregion

        #region MetodosClientes
        public void ObtenerParametrosEntradaCliente(Guid UidCliente)
        {
            parametrosEntrada = new ParametrosEntrada();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pe.* from Clientes cl, ParametrosEntrada pe where cl.UidCliente = pe.UidPropietario and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosEntrada.UidParametroEntrada = Guid.Parse(item["UidParametroEntrada"].ToString());
                parametrosEntrada.IdCompany = item["IdCompany"].ToString();
                parametrosEntrada.IdBranch = item["IdBranch"].ToString();
                parametrosEntrada.VchModena = item["VchModena"].ToString();
                parametrosEntrada.VchUsuario = item["VchUsuario"].ToString();
                parametrosEntrada.VchPassword = item["VchPassword"].ToString();
                parametrosEntrada.VchCanal = item["VchCanal"].ToString();
                parametrosEntrada.VchData0 = item["VchData0"].ToString();
                parametrosEntrada.VchUrl = item["VchUrl"].ToString();
                parametrosEntrada.VchSemillaAES = item["VchSemillaAES"].ToString();
                parametrosEntrada.UidPropietario = Guid.Parse(item["UidPropietario"].ToString());
            }
        }

        public bool RegistrarParametrosEntradaCliente(ParametrosEntrada parametrosEntrada)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosEntradaRegistrarCliente";

                comando.Parameters.Add("@IdCompany", SqlDbType.VarChar, 50);
                comando.Parameters["@IdCompany"].Value = parametrosEntrada.IdCompany;

                comando.Parameters.Add("@IdBranch", SqlDbType.VarChar, 50);
                comando.Parameters["@IdBranch"].Value = parametrosEntrada.IdBranch;

                comando.Parameters.Add("@VchModena", SqlDbType.VarChar, 50);
                comando.Parameters["@VchModena"].Value = parametrosEntrada.VchModena;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = parametrosEntrada.VchUsuario;

                comando.Parameters.Add("@VchPassword", SqlDbType.VarChar, 50);
                comando.Parameters["@VchPassword"].Value = parametrosEntrada.VchPassword;

                comando.Parameters.Add("@VchCanal", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCanal"].Value = parametrosEntrada.VchCanal;

                comando.Parameters.Add("@VchData0", SqlDbType.VarChar, 50);
                comando.Parameters["@VchData0"].Value = parametrosEntrada.VchData0;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUrl"].Value = parametrosEntrada.VchUrl;

                comando.Parameters.Add("@VchSemillaAES", SqlDbType.VarChar, 50);
                comando.Parameters["@VchSemillaAES"].Value = parametrosEntrada.VchSemillaAES;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosEntrada.UidPropietario;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool ActualizarParametrosEntradaCliente(ParametrosEntrada parametrosEntrada)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ParametrosEntradaActualizarCliente";

                comando.Parameters.Add("@IdCompany", SqlDbType.VarChar, 50);
                comando.Parameters["@IdCompany"].Value = parametrosEntrada.IdCompany;

                comando.Parameters.Add("@IdBranch", SqlDbType.VarChar, 50);
                comando.Parameters["@IdBranch"].Value = parametrosEntrada.IdBranch;

                comando.Parameters.Add("@VchModena", SqlDbType.VarChar, 50);
                comando.Parameters["@VchModena"].Value = parametrosEntrada.VchModena;

                comando.Parameters.Add("@VchUsuario", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUsuario"].Value = parametrosEntrada.VchUsuario;

                comando.Parameters.Add("@VchPassword", SqlDbType.VarChar, 50);
                comando.Parameters["@VchPassword"].Value = parametrosEntrada.VchPassword;

                comando.Parameters.Add("@VchCanal", SqlDbType.VarChar, 50);
                comando.Parameters["@VchCanal"].Value = parametrosEntrada.VchCanal;

                comando.Parameters.Add("@VchData0", SqlDbType.VarChar, 50);
                comando.Parameters["@VchData0"].Value = parametrosEntrada.VchData0;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar, 50);
                comando.Parameters["@VchUrl"].Value = parametrosEntrada.VchUrl;

                comando.Parameters.Add("@VchSemillaAES", SqlDbType.VarChar, 50);
                comando.Parameters["@VchSemillaAES"].Value = parametrosEntrada.VchSemillaAES;

                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = parametrosEntrada.UidPropietario;

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
