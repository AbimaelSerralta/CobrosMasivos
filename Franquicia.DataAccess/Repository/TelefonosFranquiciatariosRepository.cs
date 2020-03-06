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
    public class TelefonosFranquiciatariosRepository: SqlDataRepository
    {
        private TelefonosFranquiciatarios _telefonosFranquiciatarios = new TelefonosFranquiciatarios();
        public TelefonosFranquiciatarios telefonosFranquiciatarios
        {
            get { return _telefonosFranquiciatarios; }
            set { _telefonosFranquiciatarios = value; }
        }

        public TelefonosFranquiciatarios ObtenerTelefonoFranquiciatario(Guid UidFranquiciatario)
        {
            List<TelefonosFranquiciatarios> lsFranquiciatarios = new List<TelefonosFranquiciatarios>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TelefonosFranquiciatarios where UidFranquiciatario = '" + UidFranquiciatario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                telefonosFranquiciatarios = new TelefonosFranquiciatarios()
                {
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidTipoTelefono = new Guid(item["UidTipoTelefono"].ToString())
                };
            }

            return telefonosFranquiciatarios;
        }
    }
}
