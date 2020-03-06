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
    public class CiudadesRepository : SqlDataRepository
    {
        public List<Ciudades> CargarCiudades(string UidMunicipio)
        {
            List<Ciudades> lsCiudades = new List<Ciudades>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from CatCddes where UidMunicipio = '" + UidMunicipio+ "' order by UidMunicipio asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsCiudades.Add(new Ciudades()
                {
                    UidCiudad = new Guid(item["UidCiudad"].ToString()),
                    UidMunicipio = new Guid(item["UidMunicipio"].ToString()),
                    VchCiudad = item["VchCiudad"].ToString(),
                    IntOrden = int.Parse(item["IntOrden"].ToString())
                });
            }

            return lsCiudades;
        }
    }
}
