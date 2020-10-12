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
    public class PeriodicidadesRepository : SqlDataRepository
    {
        public List<Periodicidades> CargarPeriodicidades()
        {
            List<Periodicidades> lsPeriodicidades = new List<Periodicidades>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from Periodicidades order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPeriodicidades.Add(new Periodicidades()
                {
                    UidPeriodicidad = new Guid(item["UidPeriodicidad"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPeriodicidades;
        }
    }
}
