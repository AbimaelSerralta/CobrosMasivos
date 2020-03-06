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
    public class TiposPerfilesFranquiciasRepository : SqlDataRepository
    {
        public List<TiposPerfilesFranquicia> CargarTipoPerfilFranquicia()
        {
            List<TiposPerfilesFranquicia> lsTiposPerfilesFranquicia = new List<TiposPerfilesFranquicia>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TiposPerfilesFranquicias";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposPerfilesFranquicia.Add(new TiposPerfilesFranquicia()
                {
                    UidTipoPerfilFranquicia = new Guid(item["UidTipoPerfilFranquicia"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsTiposPerfilesFranquicia;
        }
    }
}
