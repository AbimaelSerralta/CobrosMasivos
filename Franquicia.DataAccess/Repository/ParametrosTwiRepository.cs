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
    public class ParametrosTwiRepository : SqlDataRepository
    {
        private ParametrosTwi _parametrosTwi = new ParametrosTwi();
        public ParametrosTwi parametrosTwi
        {
            get { return _parametrosTwi; }
            set { _parametrosTwi = value; }
        }


        #region MetodosFranquicias
        public void CargarParametrosTwi()
        {
            parametrosTwi = new ParametrosTwi();

            //Twi
            //parametrosTwi.UidParametroTwi = Guid.Empty;
            //parametrosTwi.AccountSid = "ACcf4d1380ccb0be6d47e78a73036a29ab";
            //parametrosTwi.AuthToken = "915ce4d30dc09473b5ed753490436281";
            //parametrosTwi.NumberFrom = "+14582243212";

            //Prueba
            parametrosTwi.UidParametroTwi = Guid.Empty;
            parametrosTwi.AccountSid = "ACc7561cb09df3180ee1368e40055eedf5";
            parametrosTwi.AuthToken = "3f914e588826df9a93ed849cee73eae2";
            //parametrosTwi.NumberFrom = "+14158739087"; //SMS
            parametrosTwi.NumberFrom = "+14155238886"; //Whats
        }

        public void ObtenerParametrosTwi()
        {
            parametrosTwi = new ParametrosTwi();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pe.* from Franquiciatarios fr, ParametrosEntrada pe where fr.UidFranquiciatarios = pe.UidPropietario";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                parametrosTwi.UidParametroTwi = Guid.Parse(item["UidParametroTwi"].ToString());
                parametrosTwi.AccountSid = item["AccountSid"].ToString();
                parametrosTwi.AuthToken = item["AuthToken"].ToString();
            }
        }

        public bool RegistrarParametrosTwi(ParametrosEntrada parametrosEntrada)
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

        public bool ActualizarParametrosTwi(ParametrosEntrada parametrosEntrada)
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
    }
}
