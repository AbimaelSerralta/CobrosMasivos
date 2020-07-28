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
    public class ProcesosEnsenianzasRepository : SqlDataRepository
    {
        public List<ProcesosEnsenianzas> CargarProcesosEnsenianzas()
        {
            List<ProcesosEnsenianzas> lsProcesosEnsenianzas = new List<ProcesosEnsenianzas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from ProcesosEnsenianzas order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsProcesosEnsenianzas.Add(new ProcesosEnsenianzas()
                {
                    UidProcesoEnsenianza = new Guid(item["UidProcesoEnsenianza"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }

            return lsProcesosEnsenianzas;
        }
    }
}
