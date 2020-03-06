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
    public class AppWebRepository : SqlDataRepository
    {
        Modulos modulos = new Modulos();

        private AppWeb _appWeb = new AppWeb();
        public AppWeb appWeb
        {
            get { return _appWeb; }
            set { _appWeb = value; }
        }

        public void ObtenerAppWeb(Guid UidAppWeb)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select semo.UidSegModulo, semo.VchNombre, semo.VchUrl, app.* from SegModulos semo, AppWeb app where semo.UidAppWeb = app.UidAppWeb and app.UidAppWeb = '" + UidAppWeb.ToString() + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                appWeb = new AppWeb()
                {
                    UidAppWeb = new Guid(item["UidAppWeb"].ToString()),
                    VchNombre = item["VchNombre"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                };
                modulos.UidSegModulo = new Guid(item["UidSegModulo"].ToString());
                modulos.VchUrl = item["VchUrl"].ToString();
            }
        }
    }
}
