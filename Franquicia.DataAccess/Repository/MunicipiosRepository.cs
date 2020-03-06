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
    public class MunicipiosRepository : SqlDataRepository
    {
        public List<Municipios> CargarMunicipios(string UidEstado)
        {
            List<Municipios> lsMunicipios = new List<Municipios>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from CatMpios where UidEstado = '" + UidEstado + "' order by VchMunicipio asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsMunicipios.Add(new Municipios()
                {
                    UidMunicipio = new Guid(item["UidMunicipio"].ToString()),
                    UidEstado = new Guid(item["UidEstado"].ToString()),
                    VchMunicipio = item["VchMunicipio"].ToString(),
                    IntOrden = int.Parse(item["IntOrden"].ToString())
                });
            }

            return lsMunicipios;
        }
    }
}
