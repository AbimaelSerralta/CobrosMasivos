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
    public class NivelesEnsenianzasRepository : SqlDataRepository
    {
        public List<NivelesEnsenianzas> CargarNivelesEnsenianzas(Guid UidTipoEnsenianza)
        {
            List<NivelesEnsenianzas> lsNivelesEnsenianzas = new List<NivelesEnsenianzas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from NivelesEnsenianzas where UidTipoEnsenianza = '" + UidTipoEnsenianza + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsNivelesEnsenianzas.Add(new NivelesEnsenianzas()
                {
                    UidNivelEnsenianza = new Guid(item["UidNivelEnsenianza"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    UidTipoEnsenianza = new Guid(item["UidTipoEnsenianza"].ToString())
                });
            }

            return lsNivelesEnsenianzas;
        }
    }
}
