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
    public class ComisionesTarjetasClientesTerminalRepository : SqlDataRepository
    {
        private ComisionesTarjetasClientesTerminal _comisionesTarjetasClientesTerminal = new ComisionesTarjetasClientesTerminal();
        public ComisionesTarjetasClientesTerminal comisionesTarjetasClientesTerminal
        {
            get { return _comisionesTarjetasClientesTerminal; }
            set { _comisionesTarjetasClientesTerminal = value; }
        }

        public List<ComisionesTarjetasClientesTerminal> CargarComisionesTarjetaTerminal(Guid UidCliente)
        {
            List<ComisionesTarjetasClientesTerminal> lsComisionesTarjetasClientesTerminal = new List<ComisionesTarjetasClientesTerminal>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasClientesTerminal where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasClientesTerminal.Add(new ComisionesTarjetasClientesTerminal()
                {
                    UidComicionTarjetaClienteTerminal = new Guid(item["UidComicionTarjetaClienteTerminal"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasClientesTerminal;
        }
        public List<ComisionesTarjetasClientesTerminal> CargarComisionesTarjetaTerminalTipoTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<ComisionesTarjetasClientesTerminal> lsComisionesTarjetasClientesTerminal = new List<ComisionesTarjetasClientesTerminal>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasClientesTerminal where UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasClientesTerminal.Add(new ComisionesTarjetasClientesTerminal()
                {
                    UidComicionTarjetaClienteTerminal = new Guid(item["UidComicionTarjetaClienteTerminal"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasClientesTerminal;
        }
        public bool RegistrarComisionesTarjetaTerminal(ComisionesTarjetasClientesTerminal comisionesTarjetasClientesTerminal)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClientesTerminalRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasClientesTerminal.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasClientesTerminal.DcmComision;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = comisionesTarjetasClientesTerminal.UidTipoTarjeta;
                
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = comisionesTarjetasClientesTerminal.UidCliente;

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
        public bool EliminarComisionesTarjetaTerminal(Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClientesTerminalEliminar";

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = UidCliente;

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
