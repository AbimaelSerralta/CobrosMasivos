using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class TelefonosUsuariosRepository : SqlDataRepository
    {
        private TelefonosUsuarios _telefonosUsuarios = new TelefonosUsuarios();
        public TelefonosUsuarios telefonosUsuarios
        {
            get { return _telefonosUsuarios; }
            set { _telefonosUsuarios = value; }
        }

        public TelefonosUsuarios ObtenerTelefonoUsuario(Guid UidUsuario)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TelefonosUsuarios where UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                telefonosUsuarios = new TelefonosUsuarios()
                {
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidTipoTelefono = new Guid(item["UidTipoTelefono"].ToString()),
                    UidPrefijo = new Guid(item["UidPrefijo"].ToString())
                };
            }

            return telefonosUsuarios;
        }

        public TelefonosUsuarios ObtenerTelefonoUsuarioSinPrefijo(Guid UidUsuario)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TelefonosUsuarios where UidUsuario = '" + UidUsuario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                telefonosUsuarios = new TelefonosUsuarios()
                {
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidTipoTelefono = new Guid(item["UidTipoTelefono"].ToString())
                };
            }

            return telefonosUsuarios;
        }

        #region Twilio
        public bool ActualizarEstatusWhats(Guid UidTelefono, Guid UidPermisoWhats)
        {
            bool result = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_PermisoWhatsActualizar";

                comando.Parameters.Add("@UidTelefono", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTelefono"].Value = UidTelefono;

                comando.Parameters.Add("@UidPermisoWhats", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPermisoWhats"].Value = UidPermisoWhats;

                result = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }

            return result;
        }
        public string ObtenerIdTelefono(string Telefono)
        {
            string result = string.Empty;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidTelefonoUsuario from TelefonosUsuarios where VchTelefono = '" + Telefono + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = item["UidTelefonoUsuario"].ToString();
            }

            return result;
        }
        #endregion
    }
}
