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
    public class TelefonosAlumnosRepository : SqlDataRepository
    {
        private TelefonosAlumnos _telefonosAlumnos = new TelefonosAlumnos();
        public TelefonosAlumnos telefonosAlumnos
        {
            get { return _telefonosAlumnos; }
            set { _telefonosAlumnos = value; }
        }

        public TelefonosAlumnos ObtenerTelefonosAlumnos(Guid UidAlumno)
        {
            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TelefonosAlumnos where UidAlumno = '" + UidAlumno + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                telefonosAlumnos = new TelefonosAlumnos()
                {
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidPrefijo = new Guid(item["UidPrefijo"].ToString())
                };
            }

            return telefonosAlumnos;
        }
    }
}
