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
    public class TiposEventosRepository : SqlDataRepository
    {
        public List<TiposEventos> CargarTiposEventos()
        {
            List<TiposEventos> lsTiposEventos = new List<TiposEventos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from TiposEventos order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposEventos.Add(new TiposEventos()
                {
                    UidTipoEvento = new Guid(item["UidTipoEvento"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }

            return lsTiposEventos;
        }
    }
}
