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
    public class SuperComisionesTarjetasPragaRepository : SqlDataRepository
    {
        private SuperComisionesTarjetasPraga _superComisionesTarjetasPraga = new SuperComisionesTarjetasPraga();
        public SuperComisionesTarjetasPraga superComisionesTarjetasPraga
        {
            get { return _superComisionesTarjetasPraga; }
            set { _superComisionesTarjetasPraga = value; }
        }

        public List<SuperComisionesTarjetasPraga> CargarComisionesTarjeta()
        {
            List<SuperComisionesTarjetasPraga> lsSuperComisionesTarjetasPraga = new List<SuperComisionesTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SuperComisionesTarjetasPraga";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperComisionesTarjetasPraga.Add(new SuperComisionesTarjetasPraga()
                {
                    UidSuperComicionTarjeta = new Guid(item["UidSuperComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsSuperComisionesTarjetasPraga;
        }
        public List<SuperComisionesTarjetasPraga> CargarComisionesTarjetaTipoTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            List<SuperComisionesTarjetasPraga> lsSuperComisionesTarjetasPraga = new List<SuperComisionesTarjetasPraga>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from SuperComisionesTarjetasPraga where UidCliente = '" + UidCliente + "' and UidTipoTarjeta = '" + UidTipoTarjeta + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                lsSuperComisionesTarjetasPraga.Add(new SuperComisionesTarjetasPraga()
                {
                    UidSuperComicionTarjeta = new Guid(item["UidSuperComicionTarjeta"].ToString()),
                    BitComision = bool.Parse(item["BitComision"].ToString()),
                    DcmComision = decimal.Parse(item["DcmComision"].ToString()),
                    UidTipoTarjeta = new Guid(item["UidTipoTarjeta"].ToString())
                });
            }

            return lsSuperComisionesTarjetasPraga;
        }
        public bool RegistrarComisionesTarjeta(SuperComisionesTarjetasPraga SuperComisionesTarjetasPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaClubPagoRegistrar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = SuperComisionesTarjetasPraga.BitComision;

                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = SuperComisionesTarjetasPraga.DcmComision;

                comando.Parameters.Add("@UidTipoTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidTipoTarjeta"].Value = SuperComisionesTarjetasPraga.UidTipoTarjeta;
               
                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {

                throw;
            }
            return Resultado;
        }
        public bool ActualizarComisionesTarjeta(SuperComisionesTarjetasPraga SuperComisionesTarjetasPraga)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ComisionesTarjetaActualizar";

                comando.Parameters.Add("@BitComision", SqlDbType.Bit);
                comando.Parameters["@BitComision"].Value = SuperComisionesTarjetasPraga.BitComision;
                
                comando.Parameters.Add("@DcmComision", SqlDbType.Decimal);
                comando.Parameters["@DcmComision"].Value = SuperComisionesTarjetasPraga.DcmComision;

                comando.Parameters.Add("@UidSuperComicionTarjeta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidSuperComicionTarjeta"].Value = SuperComisionesTarjetasPraga.UidSuperComicionTarjeta;

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
