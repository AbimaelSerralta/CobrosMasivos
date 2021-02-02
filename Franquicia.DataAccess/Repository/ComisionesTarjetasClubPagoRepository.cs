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
    public class ComisionesTarjetasClubPagoRepository : SqlDataRepository
    {
        private ComisionesTarjetasClubPago _comisionesTarjetasClubPago = new ComisionesTarjetasClubPago();
        public ComisionesTarjetasClubPago comisionesTarjetasClubPago
        {
            get { return _comisionesTarjetasClubPago; }
            set { _comisionesTarjetasClubPago = value; }
        }

        public List<ComisionesTarjetasClubPago> CargarComisionesTarjeta(Guid UidCliente)
        {
            List<ComisionesTarjetasClubPago> lsComisionesTarjetasClubPago = new List<ComisionesTarjetasClubPago>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasClubPago where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasClubPago.Add(new ComisionesTarjetasClubPago()
                {
                    UidComicionTarjeta = new Guid(item["UidComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasClubPago;
        }
        public List<ComisionesTarjetasClubPago> CargarComisionesTarjetaTipoTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<ComisionesTarjetasClubPago> lsComisionesTarjetasClubPago = new List<ComisionesTarjetasClubPago>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasClientesTerminal where UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasClubPago.Add(new ComisionesTarjetasClubPago()
                {
                    UidComicionTarjeta = new Guid(item["UidComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasClubPago;
        }
        public bool RegistrarComisionesTarjeta(ComisionesTarjetasClubPago comisionesTarjetasClubPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClubPagoRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasClubPago.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasClubPago.DcmComision;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = comisionesTarjetasClubPago.UidTipoTarjeta;
                
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = comisionesTarjetasClubPago.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarComisionesTarjeta(ComisionesTarjetasClubPago comisionesTarjetasClubPago)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaActualizar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasClubPago.BitComision;
                
                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasClubPago.DcmComision;

                comando.Parameters.Add("@UidComicionTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidComicionTarjeta"].Value = comisionesTarjetasClubPago.UidComicionTarjeta;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool EliminarComisionesTarjeta(Guid UidCliente)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClubPagoEliminar";

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
