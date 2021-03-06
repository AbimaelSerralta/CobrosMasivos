﻿using Franquicia.DataAccess.Common;
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
    public class EstatusFechasColegiaturasRepository : SqlDataRepository
    {
        public List<EstatusFechasColegiaturas> CargarEstatusFechasColegiaturas()
        {
            List<EstatusFechasColegiaturas> lsEstatusFechasColegiaturas = new List<EstatusFechasColegiaturas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from EstatusFechasColegiaturas order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsEstatusFechasColegiaturas.Add(new EstatusFechasColegiaturas()
                {
                    UidEstatusFechaColegiatura = new Guid(item["UidEstatusFechaColegiatura"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString()
                });
            }

            return lsEstatusFechasColegiaturas;
        }
    }
}
