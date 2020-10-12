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
    public class ComisionesTarjetasClientesRepository : SqlDataRepository
    {
        private ComisionesTarjetasClientes _comisionesTarjetasClientes = new ComisionesTarjetasClientes();
        public ComisionesTarjetasClientes comisionesTarjetasClientes
        {
            get { return _comisionesTarjetasClientes; }
            set { _comisionesTarjetasClientes = value; }
        }

        public List<ComisionesTarjetasClientes> CargarComisionesTarjeta(Guid UidCliente)
        {
            List<ComisionesTarjetasClientes> lsComisionesTarjetasClientes = new List<ComisionesTarjetasClientes>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasClientes where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasClientes.Add(new ComisionesTarjetasClientes()
                {
                    UidComicionTarjetaCliente = new Guid(item["UidComicionTarjetaCliente"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString())
                });
            }

            return lsComisionesTarjetasClientes;
        }
        public bool RegistrarComisionesTarjeta(ComisionesTarjetasClientes comisionesTarjetasClientes)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasClientes.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasClientes.DcmComision;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = comisionesTarjetasClientes.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarComisionesTarjeta(ComisionesTarjetasClientes comisionesTarjetasClientes)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaActualizar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasClientes.BitComision;
                
                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasClientes.DcmComision;

                comando.Parameters.Add("@UidComicionTarjetaCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidComicionTarjetaCliente"].Value = comisionesTarjetasClientes.UidComicionTarjetaCliente;

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
