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
    public class FormasPagosRepository : SqlDataRepository
    {
        public List<FormasPagos> CargarFormasPagos()
        {
            List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from FormasPagos order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsFormasPagos.Add(new FormasPagos()
                {
                    UidFormaPago = new Guid(item["UidEstatus"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }

            return lsFormasPagos;
        }
    }
}
