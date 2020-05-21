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
    public class TicketsRepository : SqlDataRepository
    {
        private Tickets _tickets = new Tickets();
        public Tickets tickets
        {
            get { return _tickets; }
            set { _tickets = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        public void ObtenerDineroCuentaCliente(Guid UidCliente)
        {
            //clienteCuenta = new ClienteCuenta();

            //SqlCommand query = new SqlCommand();
            //query.CommandType = CommandType.Text;

            //query.CommandText = "select cc.* from Clientes cl, ClienteCuenta cc where cl.UidCliente = cc.UidCliente and cc.UidCliente = '" + UidCliente + "'";

            //DataTable dt = this.Busquedas(query);

            //foreach (DataRow item in dt.Rows)
            //{
            //    clienteCuenta.UidClienteCuenta = Guid.Parse(item["UidClienteCuenta"].ToString());
            //    clienteCuenta.DcmDineroCuenta = decimal.Parse(item["DcmDineroCuenta"].ToString());
            //}
        }

        public bool RegistrarTicketPago(Tickets tickets, HistorialPagos historialPagos)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_TicketsRegistrar";

                comando.Parameters.Add("@VchFolio", SqlDbType.VarChar, 100);
                comando.Parameters["@VchFolio"].Value = tickets.VchFolio;

                comando.Parameters.Add("@DcmImporte", SqlDbType.Decimal);
                comando.Parameters["@DcmImporte"].Value = tickets.DcmImporte;
                
                comando.Parameters.Add("@DcmDescuento", SqlDbType.Decimal);
                comando.Parameters["@DcmDescuento"].Value = tickets.DcmDescuento;
                
                comando.Parameters.Add("@DcmTotal", SqlDbType.Decimal);
                comando.Parameters["@DcmTotal"].Value = tickets.DcmTotal;

                comando.Parameters.Add("@VchDescripcion", SqlDbType.VarChar, 50);
                comando.Parameters["@VchDescripcion"].Value = tickets.VchDescripcion;
                
                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = tickets.UidPropietario;
                
                comando.Parameters.Add("@DtRegistro", SqlDbType.DateTime);
                comando.Parameters["@DtRegistro"].Value = tickets.DtRegistro;
                
                comando.Parameters.Add("@UidHistorialPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidHistorialPago"].Value = tickets.UidHistorialPago;
                
                comando.Parameters.Add("@IntWA", SqlDbType.Int);
                comando.Parameters["@IntWA"].Value = tickets.IntWA;
                
                comando.Parameters.Add("@IntSMS", SqlDbType.Int);
                comando.Parameters["@IntSMS"].Value = tickets.IntSMS;
                
                comando.Parameters.Add("@IntCorreo", SqlDbType.Int);
                comando.Parameters["@IntCorreo"].Value = tickets.IntCorreo;

                //====================HISTORIALPAGOS=======================================

                comando.Parameters.Add("@DcmSaldo", SqlDbType.Decimal);
                comando.Parameters["@DcmSaldo"].Value = historialPagos.DcmSaldo;

                comando.Parameters.Add("@DcmOperacion", SqlDbType.Decimal);
                comando.Parameters["@DcmOperacion"].Value = historialPagos.DcmOperacion;

                comando.Parameters.Add("@DcmNuevoSaldo", SqlDbType.Decimal);
                comando.Parameters["@DcmNuevoSaldo"].Value = historialPagos.DcmNuevoSaldo;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool ActualizarTicketPago(ClienteCuenta clienteCuenta, string IdReferencia)
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
