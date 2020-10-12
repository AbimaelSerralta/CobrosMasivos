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
    public class TiposEnsenianzasRepository : SqlDataRepository
    {
        public List<TiposEnsenianzas> CargarTiposEnsenianzas(Guid UidProcesoEnsenianza)
        {
            List<TiposEnsenianzas> lsTiposEnsenianzas = new List<TiposEnsenianzas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from TiposEnsenianzas where UidProcesoEnsenianza ='" + UidProcesoEnsenianza + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposEnsenianzas.Add(new TiposEnsenianzas()
                {
                    UidTipoEnsenianza = new Guid(item["UidTipoEnsenianza"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    UidProcesoEnsenianza = new Guid(item["UidProcesoEnsenianza"].ToString())
                });
            }

            return lsTiposEnsenianzas;
        }
    }
}
