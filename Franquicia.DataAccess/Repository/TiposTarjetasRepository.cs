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
    public class TiposTarjetasRepository : SqlDataRepository
    {
        public List<TiposTarjetas> CargarTiposTarjetas()
        {
            List<TiposTarjetas> lsTiposTarjetas = new List<TiposTarjetas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TiposTarjetas order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposTarjetas.Add(new TiposTarjetas()
                {
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    BitPromociones = bool.Parse(item["BitPromociones"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchImagen = item["VchImagen"].ToString(),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsTiposTarjetas;
        }
        
        public List<TiposTarjetas> CargarTiposTarjetasCliente(Guid UidCliente)
        {
            List<TiposTarjetas> lsTiposTarjetas = new List<TiposTarjetas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select tp.* from TiposTarjetas tp, ComisionesTarjetasClientesTerminal cct where tp.UidTipoTarjeta = cct.UidTipoTarjeta and cct.BitComision = 1 and UidCliente = '" + UidCliente + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposTarjetas.Add(new TiposTarjetas()
                {
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    BitPromociones = bool.Parse(item["BitPromociones"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchImagen = item["VchImagen"].ToString(),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsTiposTarjetas;
        }
    }
}
