using Franquicia.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class ValidacionesRepository : SqlDataRepository
    {
        public bool ExisteCorreo(string Correo)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchCorreo from Usuarios where VchCorreo = '" + Correo + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }

        public bool ExisteUsuario(string Usuario)
        {
            bool result = false;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select VchUsuario from SegUsuarios where VchUsuario = '" + Usuario + "'";

            DataTable dt = this.Busquedas(query);

            if (dt.Rows.Count >= 1)
            {
                result = true;
            }

            return result;
        }
    }
}
