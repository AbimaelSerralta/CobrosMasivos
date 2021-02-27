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
    public class TiposTarjetasPragaRepository : SqlDataRepository
    {
        public List<TiposTarjetasPraga> CargarTiposTarjetas()
        {
            List<TiposTarjetasPraga> lsTiposTarjetasPraga = new List<TiposTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from TiposTarjetasPraga order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposTarjetasPraga.Add(new TiposTarjetasPraga()
                {
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    BitContado = bool.Parse(item["BitContado"].ToString()),
                    BitPromociones = bool.Parse(item["BitPromociones"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchImagen = item["VchImagen"].ToString(),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsTiposTarjetasPraga;
        }

        public List<TiposTarjetasPraga> CargarTiposTarjetasCliente(Guid UidCliente)
        {
            List<TiposTarjetasPraga> lsTiposTarjetasPraga = new List<TiposTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select ttp.* from TiposTarjetasPraga ttp, ComisionesTarjetasPraga ctp where ttp.UidTipoTarjeta = ctp.UidTipoTarjeta and ctp.BitComision = 1 and UidCliente = '" + UidCliente + "' order by IntGerarquia asc";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsTiposTarjetasPraga.Add(new TiposTarjetasPraga()
                {
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    BitPromociones = bool.Parse(item["BitPromociones"].ToString()),
                    IntGerarquia = int.Parse(item["IntGerarquia"].ToString()),
                    VchImagen = item["VchImagen"].ToString(),
                    VchColor = item["VchColor"].ToString()
                });
            }

            return lsTiposTarjetasPraga;
        }
    }
}
