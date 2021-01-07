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
    public class EstatusFechasPagosRepository : SqlDataRepository
    {
        public List<EstatusFechasPagos> CargarEstatusFechasPagos()
        {
            List<EstatusFechasPagos> lsEstatusFechasPagos = new List<EstatusFechasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from EstatusFechasPagos order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatusFechasPagos.Add(new EstatusFechasPagos()
                {
                    UidEstatusFechaPago = new Guid(item["UidEstatus"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsEstatusFechasPagos;
        }
        public List<EstatusFechasPagos> CargarEstatusFechasPagosApRe()
        {
            List<EstatusFechasPagos> lsEstatusFechasPagos = new List<EstatusFechasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from EstatusFechasPagos where UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or UidEstatusFechaPago = '77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatusFechasPagos.Add(new EstatusFechasPagos()
                {
                    UidEstatusFechaPago = new Guid(item["UidEstatusFechaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsEstatusFechasPagos;
        }

        #region PanelEscuela
        #region ReporteLigasEscuela
        public List<EstatusFechasPagos> CargarEstatusFechasPagosBusquedaRLE()
        {
            List<EstatusFechasPagos> lsEstatusFechasPagos = new List<EstatusFechasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from EstatusFechasPagos where UidEstatusFechaPago = '8720B2B9-5712-4E75-A981-932887AACDC9' or UidEstatusFechaPago = '77DB3F13-7EC8-4CE1-A3DB-E5C96D14A581' or UidEstatusFechaPago = 'F25E4AAB-6044-46E9-A575-98DCBCCF7604' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatusFechasPagos.Add(new EstatusFechasPagos()
                {
                    UidEstatusFechaPago = new Guid(item["UidEstatusFechaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchIcono = item["VchIcono"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsEstatusFechasPagos;
        }
        #endregion
        #endregion
    }
}
