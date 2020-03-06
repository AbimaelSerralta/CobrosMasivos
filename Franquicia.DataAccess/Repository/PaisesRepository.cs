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
    public class PaisesRepository : SqlDataRepository
    {
        public List<Paises> CargarPaises()
        {
            List<Paises> lsPaises = new List<Paises>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from CatPaises order by IntOrden asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPaises.Add(new Paises()
                {
                    UidPais = new Guid(item["UidPais"].ToString()),
                    VchPais = item["VchPais"].ToString(),
                    IntOrden = int.Parse(item["IntOrden"].ToString())
                });
            }

            return lsPaises;
        }
    }
}
