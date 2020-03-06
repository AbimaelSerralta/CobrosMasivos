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
    public class ColoniasRepository : SqlDataRepository
    {
        public List<Colonias> CargarColonias(string UidCiudad)
        {
            List<Colonias> lsColonias = new List<Colonias>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from CatClnas where UidCiudad = '" +  UidCiudad + "' order by VchColonia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsColonias.Add(new Colonias()
                {
                    UidColonia = new Guid(item["UidColonia"].ToString()),
                    UidCiudad = new Guid(item["UidCiudad"].ToString()),
                    VchColonia = item["VchColonia"].ToString(),
                    VchCodigoPostal = item["VchCodigoPostal"].ToString(),
                    IntOrden = int.Parse(item["IntOrden"].ToString())
                });
            }

            return lsColonias;
        }
    }
}
