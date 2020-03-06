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
    public class TiposTelefonosRepository : SqlDataRepository
    {
        TiposTelefonos _tiposTelefonos = new TiposTelefonos();
        public TiposTelefonos tiposTelefonos
        {
            get { return _tiposTelefonos; }
            set { _tiposTelefonos = value; }
        }

        public List<TiposTelefonos> CargarTiposTelefonos()
        {
            List<TiposTelefonos> lsTiposTelefonos = new List<TiposTelefonos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from TiposTelefonos";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposTelefonos.Add(new TiposTelefonos()
                {
                    UidTipoTelefono = Guid.Parse(item["UidTipoTelefono"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsTiposTelefonos;
        }
    }
}
