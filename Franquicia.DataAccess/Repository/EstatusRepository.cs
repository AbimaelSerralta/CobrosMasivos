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
    public class EstatusRepository : SqlDataRepository
    {
        public List<Estatus> CargarEstatus()
        {
            List<Estatus> lsEstatus = new List<Estatus>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from Estatus order by VchDescripcion asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatus.Add(new Estatus()
                {
                    UidEstatus = new Guid(item["UidEstatus"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsEstatus;
        }
    }
}
