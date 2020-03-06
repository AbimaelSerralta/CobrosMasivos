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
    public class EstadosRepository : SqlDataRepository
    {
        public List<Estados> CargarEstados(string UidPais)
        {
            List<Estados> lsEstados = new List<Estados>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from CatEstados where UidPais = '" + UidPais+ "' order by VchEstado asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstados.Add(new Estados()
                {
                    UidEstado = new Guid(item["UidEstado"].ToString()),
                    UidPais = new Guid(item["UidPais"].ToString()),
                    VchEstado = item["VchEstado"].ToString(),
                    IntOrden = int.Parse(item["IntOrden"].ToString())
                });
            }

            return lsEstados;
        }
    }
}
