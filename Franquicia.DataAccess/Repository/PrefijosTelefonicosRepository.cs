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
    public class PrefijosTelefonicosRepository : SqlDataRepository
    {
        private PrefijosTelefonicos _prefijosTelefonicos = new PrefijosTelefonicos();
        public PrefijosTelefonicos prefijosTelefonicos
        {
            get { return _prefijosTelefonicos; }
            set { _prefijosTelefonicos = value; }
        }

        public List<PrefijosTelefonicos> CargarPrefijosTelefonicos()
        {
            List<PrefijosTelefonicos> lsPrefijosTelefonicos = new List<PrefijosTelefonicos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from PrefijosTelefonicos order by VchPais asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPrefijosTelefonicos.Add(new PrefijosTelefonicos()
                {
                    UidPrefijo = new Guid(item["UidPrefijo"].ToString()),
                    VchPais = item["VchPais"].ToString(),
                    Prefijo = item["Prefijo"].ToString()
                });
            }

            return lsPrefijosTelefonicos;
        }

        public void ValidarPrefijoTelefonico(string Prefijo)
        {
            prefijosTelefonicos = new PrefijosTelefonicos();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select UidPrefijo, Prefijo from PrefijosTelefonicos where Prefijo = '" + Prefijo + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                prefijosTelefonicos.UidPrefijo = new Guid(item["UidPrefijo"].ToString());
                prefijosTelefonicos.Prefijo = item["Prefijo"].ToString();
            }
        }
    }
}
