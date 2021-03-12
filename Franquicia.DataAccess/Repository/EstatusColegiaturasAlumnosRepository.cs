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
    public class EstatusColegiaturasAlumnosRepository : SqlDataRepository
    {
        public List<EstatusColegiaturasAlumnos> CargarEstatusColegiaturasAlumnos()
        {
            List<EstatusColegiaturasAlumnos> lsEstatusColegiaturasAlumnos = new List<EstatusColegiaturasAlumnos>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from EstatusColegiaturasAlumnos order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatusColegiaturasAlumnos.Add(new EstatusColegiaturasAlumnos()
                {
                    UidEstatusColeAlumnos = new Guid(item["UidEstatusColeAlumnos"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsEstatusColegiaturasAlumnos;
        }
    }
}
