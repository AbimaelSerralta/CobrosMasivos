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
    public class PagosClubPagoRepository : SqlDataRepository
    {       

        #region Metodos Escuela
        #region Pagos
        
        #endregion

        #region ReporteLigasPadres
        public List<PagosClubPago> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            List<PagosClubPago> lsPagosClubPago = new List<PagosClubPago>(); ;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select pcp.* from ReferenciasClubPago rcp, PagosClubPago pcp where (pcp.UidPagoEstatus = '9F512165-96A6-407F-925A-A27C2149F3B9' or pcp.UidPagoEstatus = 'E87E6D94-2B4E-4AEA-B5C4-2EAD3C4BC69B') and rcp.IdReferencia = pcp.IdReferencia and rcp.UidPagoColegiatura = '" + UidPagoColegiatura + "' order by pcp.DtFechaRegistro desc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsPagosClubPago.Add(new PagosClubPago()
                {
                    IdReferencia = item["IdReferencia"].ToString(),
                    DtFechaRegistro = DateTime.Parse(item["DtFechaRegistro"].ToString()),
                    VchTransaccion = item["VchTransaccion"].ToString(),
                    DcmMonto = decimal.Parse(item["DcmMonto"].ToString())
                });
            }

            return lsPagosClubPago;
        }
        #endregion
        #endregion
    }
}
