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
    public class ClienteCuentaRepository : SqlDataRepository
    {
        private ClienteCuenta _clienteCuenta = new ClienteCuenta();
        public ClienteCuenta clienteCuenta
        {
            get { return _clienteCuenta; }
            set { _clienteCuenta = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        public void ObtenerDineroCuentaCliente(Guid UidCliente)
        {
            clienteCuenta = new ClienteCuenta();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select cc.* from Clientes cl, ClienteCuenta cc where cl.UidCliente = cc.UidCliente and cc.UidCliente = '" + UidCliente + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                clienteCuenta.UidClienteCuenta = Guid.Parse(item["UidClienteCuenta"].ToString());
                clienteCuenta.DcmDineroCuenta = decimal.Parse(item["DcmDineroCuenta"].ToString());
            }
        }

        public bool RegistrarDineroCuentaCliente(ClienteCuenta clienteCuenta, string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClienteCuentaRegistrar";

                comando.Parameters.Add("@UidClienteCuenta", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidClienteCuenta"].Value = clienteCuenta.UidClienteCuenta;

                comando.Parameters.Add("@DcmDineroCuenta", SqlDbType.Decimal);
                comando.Parameters["@DcmDineroCuenta"].Value = clienteCuenta.DcmDineroCuenta;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = clienteCuenta.UidCliente;
                
                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool ActualizarDineroCuentaCliente(ClienteCuenta clienteCuenta, string IdReferencia)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_ClienteCuentaActualizar";

                comando.Parameters.Add("@DcmDineroCuenta", SqlDbType.Decimal);
                comando.Parameters["@DcmDineroCuenta"].Value = clienteCuenta.DcmDineroCuenta;

                comando.Parameters.Add("@UidCliente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidCliente"].Value = clienteCuenta.UidCliente;
                
                comando.Parameters.Add("@IdReferencia", SqlDbType.VarChar);
                comando.Parameters["@IdReferencia"].Value = IdReferencia;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        #endregion
    }
}
