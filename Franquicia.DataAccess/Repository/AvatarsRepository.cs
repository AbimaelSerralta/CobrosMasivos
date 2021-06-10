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
    public class AvatarsRepository : SqlDataRepository
    {
        public List<Avatars> CargarAvatars()
        {
            List<Avatars> lsAvatars = new List<Avatars>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "Select * from Avatars";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsAvatars.Add(new Avatars()
                {
                    UidAvatar = new Guid(item["UidAvatar"].ToString()),
                    VchUrl = item["VchUrl"].ToString()
                });
            }

            return lsAvatars;
        }
    }
}
