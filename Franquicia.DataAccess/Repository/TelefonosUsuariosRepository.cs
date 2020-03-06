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
                    UidTipoTelefono = new Guid(item["UidTipoTelefono"].ToString())
                };
            }

            return telefonosUsuarios;
        }
    }
}
