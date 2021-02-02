using Franquicia.DataAccess.Common;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.DataAccess.Repository.ClubPago
{
    public class HeaderClubPagoRepository : SqlDataRepository
    {
        public HeaderClubPago ObtenerHeaderClubPago()
        {
            HeaderClubPago headerClubPago = new HeaderClubPago();

            try
            {
                SqlCommand query = new SqlCommand();
                query.CommandType = CommandType.Text;

                query.CommandText = "select * from HeadersClubPago";

                DataTable dt = this.Busquedas(query);

                foreach (DataRow item in dt.Rows)
                {
                    headerClubPago = new HeaderClubPago()
                    {
                        XOrigin = item["VchXOrigin"].ToString(),
                        UserAgent = item["VchUserAgent"].ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                string mns = ex.Message;
            }

            return headerClubPago;
        }
    }
}
