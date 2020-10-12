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
    public class ComisionesTarjetasRepository : SqlDataRepository
    {
        private ComisionesTarjetas _comisionesTarjetas = new ComisionesTarjetas();
        public ComisionesTarjetas comisionesTarjetas
        {
            get { return _comisionesTarjetas; }
            set { _comisionesTarjetas = value; }
        }

        public List<ComisionesTarjetas> CargarComisionesTarjeta()
        {
            List<ComisionesTarjetas> lsComisionesTarjetas = new List<ComisionesTarjetas>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetas";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetas.Add(new ComisionesTarjetas()
                {
                    UidComicionTarjeta = new Guid(item["UidComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString())
                });
            }

            return lsComisionesTarjetas;
        }
        public bool RegistrarComisionesTarjeta(ComisionesTarjetas comisionesTarjetas)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaSupRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetas.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetas.DcmComision;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarComisionesTarjeta(ComisionesTarjetas comisionesTarjetas)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaSupActualizar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetas.BitComision;
                
                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetas.DcmComision;
                
                comando.Parameters.Add("@UidComicionTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidComicionTarjeta"].Value = comisionesTarjetas.UidComicionTarjeta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
    }
}
