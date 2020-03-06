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
    public class TiposPerfilesRepository : SqlDataRepository
    {
        public List<TiposPerfiles> CargarTipoPerfil(Guid UidAppWeb)
        {
            List<TiposPerfiles> lsTipoPerfil = new List<TiposPerfiles>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TiposPerfiles where UidAppWeb = '" + UidAppWeb + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTipoPerfil.Add(new TiposPerfiles()
                {
                    UidTipoPerfil = new Guid(item["UidTipoPerfil"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsTipoPerfil;
        }
    }
}
