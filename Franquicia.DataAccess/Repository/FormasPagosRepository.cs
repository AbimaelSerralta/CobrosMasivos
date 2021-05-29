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
                    UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString())
                });
            }

            return lsFormasPagos;
        }

        #region Metodos PanelTutor
        #region Pagos
        public List<FormasPagos> CargarFormasPagosPadres()
        {
            List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FormasPagos where IntGerarquia = 4 or IntGerarquia = 6 or IntGerarquia = 7 order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchImagen = string.Empty;

                lsFormasPagos.Add(new FormasPagos()
                {
                    UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchImagen = item["VchImagen"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsFormasPagos;
        }
        
        public List<FormasPagos> CargarFormasPagosPadres2()
        {
            List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            //Con forma pago "LIGA" query.CommandText = "select * from FormasPagos where IntGerarquia = 3 or IntGerarquia = 5 or IntGerarquia = 8 order by IntGerarquia asc";
            query.CommandText = "select * from FormasPagos where IntGerarquia = 5 or IntGerarquia = 8 order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchImagen = string.Empty;

                lsFormasPagos.Add(new FormasPagos()
                {
                    UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchImagen = item["VchImagen"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsFormasPagos;
        }
        #endregion

        #region ReportePagosPadres
        public List<FormasPagos> CargarFormasPagosReporteLigasPadres()
        {
            List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FormasPagos where IntGerarquia = 4 or IntGerarquia = 6 or IntGerarquia = 7 order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchImagen = string.Empty;

                lsFormasPagos.Add(new FormasPagos()
                {
                    UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    VchImagen = item["VchImagen"].ToString(),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsFormasPagos;
        }
        #endregion

        #region ReporteLigasEscuelas
        public List<FormasPagos> CargarFormasPagosReporteLigasEscuelas(Guid UidCliente)
        {
            List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

            SqlCommand query2 = new SqlCommand();
            query2.CommandType = CommandType.Text;

            query2.CommandText = "select * from ComisionesTarjetasClientesTerminal where BitComision = 1 and UidCliente = '" + UidCliente + "'";

            DataTable dt2 = this.Busquedas(query2);

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from FormasPagos where IntGerarquia = 1 or IntGerarquia = 2 order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                string VchImagen = string.Empty;
                if (new Guid(item["UidFormaPago"].ToString()) == Guid.Parse("3359D33E-C879-4A8B-96D3-C6A211AF014F"))
                {
                    if (dt2.Rows.Count >= 1)
                    {
                        lsFormasPagos.Add(new FormasPagos()
                        {
                            UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                            VchDescripcion = item["VchDescripcion"].ToString(),
                            VchImagen = item["VchImagen"].ToString(),
                            IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                            VchColor = item["VchColor"].ToString()
                        });
                    }
                }
                else
                {
                    lsFormasPagos.Add(new FormasPagos()
                    {
                        UidFormaPago = new Guid(item["UidFormaPago"].ToString()),
                        VchDescripcion = item["VchDescripcion"].ToString(),
                        VchImagen = item["VchImagen"].ToString(),
                        IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                        VchColor = item["VchColor"].ToString()
                    });
                }
            }

            return lsFormasPagos;
        }
        #endregion
        #endregion
    }
}
