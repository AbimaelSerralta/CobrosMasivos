using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository
{
    public class PromocionesRepository : SqlDataRepository
    {
        public List<Promociones> CargarPromociones()
        {
            List<Promociones> lsPromociones = new List<Promociones>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from promociones order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPromociones.Add(new Promociones()
                {
                    UidPromocion = new Guid(item["UidPromocion"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsPromociones;
        }

        #region Empresa
        public List<CBLPromocionesModel> CargarPromociones(Guid UidCliente)
        {
            List<CBLPromocionesModel> lsClientesCBLPromocionesModel = new List<CBLPromocionesModel>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cp.* from Clientes cl, ClientesPromociones cp where cl.UidCliente = cp.UidCliente and cl.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsClientesCBLPromocionesModel.Add(new CBLPromocionesModel()
                {
                    UidClientePromocion = new Guid(item["UidCliente"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidPromocion = new Guid(item["UidCliente"].ToString())
                });
            }
            // demo
            return lsClientesCBLPromocionesModel;
        }
        #endregion
    }
}
