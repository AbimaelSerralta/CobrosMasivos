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
    public class BancosRepository : SqlDataRepository
    {
        public List<Bancos> CargarBancos()
        {
            List<Bancos> lsBancos = new List<Bancos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from Bancos order by VchDescripcion asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsBancos.Add(new Bancos()
                {
                    UidBanco = new Guid(item["UidBanco"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsBancos;
        }
    }
}
