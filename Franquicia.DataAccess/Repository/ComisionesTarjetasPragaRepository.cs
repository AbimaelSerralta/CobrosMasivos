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
    public class ComisionesTarjetasPragaRepository : SqlDataRepository
    {
        private ComisionesTarjetasPraga _comisionesTarjetasPraga = new ComisionesTarjetasPraga();
        public ComisionesTarjetasPraga comisionesTarjetasPraga
        {
            get { return _comisionesTarjetasPraga; }
            set { _comisionesTarjetasPraga = value; }
        }

        public List<ComisionesTarjetasPraga> CargarComisionesTarjeta(Guid UidCliente)
        {
            List<ComisionesTarjetasPraga> lsComisionesTarjetasPraga = new List<ComisionesTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasPraga where UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasPraga.Add(new ComisionesTarjetasPraga()
                {
                    UidComicionTarjeta = new Guid(item["UidComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasPraga;
        }
        public List<ComisionesTarjetasPraga> CargarComisionesTarjetaTipoTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<ComisionesTarjetasPraga> lsComisionesTarjetasPraga = new List<ComisionesTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from ComisionesTarjetasPraga where UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsComisionesTarjetasPraga.Add(new ComisionesTarjetasPraga()
                {
                    UidComicionTarjeta = new Guid(item["UidComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidCliente = new Guid(item["UidCliente"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsComisionesTarjetasPraga;
        }
        public bool RegistrarComisionesTarjeta(ComisionesTarjetasPraga comisionesTarjetasPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClubPagoRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasPraga.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasPraga.DcmComision;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = comisionesTarjetasPraga.UidTipoTarjeta;
                
                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = comisionesTarjetasPraga.UidCliente;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarComisionesTarjeta(ComisionesTarjetasPraga comisionesTarjetasPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaActualizar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = comisionesTarjetasPraga.BitComision;
                
                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = comisionesTarjetasPraga.DcmComision;

                comando.Parameters.Add("@UidComicionTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidComicionTarjeta"].Value = comisionesTarjetasPraga.UidComicionTarjeta;

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
