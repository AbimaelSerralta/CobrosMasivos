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
    public class TelefonosClientesRepository : SqlDataRepository
    {
        private TelefonosClientes _telefonosClientes = new TelefonosClientes();
        public TelefonosClientes telefonosClientes
        {
            get { return _telefonosClientes; }
            set { _telefonosClientes = value; }
        }

        public TelefonosClientes ObtenerTelefonoCliente(Guid UidCliente)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TelefonosClientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                telefonosClientes = new TelefonosClientes()
                {
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidTipoTelefono = new Guid(item["UidTipoTelefono"].ToString())
                };
            }

            return telefonosClientes;
        }
    }
}
