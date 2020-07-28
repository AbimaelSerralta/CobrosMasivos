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
    public class WhatsAppPendientesRepository : SqlDataRepository
    {
        private WhatsAppPendientes _whatsAppPendientes = new WhatsAppPendientes();
        public WhatsAppPendientes whatsAppPendientes
        {
            get { return _whatsAppPendientes; }
            set { _whatsAppPendientes = value; }
        }

        #region MetodosFranquicias

        #endregion

        #region MetodosClientes
        public List<WhatsAppPendientes> ObtenerWhatsPendiente(string Telefono)
        {
            List<WhatsAppPendientes> lsWhatsAppPendientes = new List<WhatsAppPendientes>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select * from WhatsAppPendientes where UidEstatusWhatsApp = '06BDD7B7-C463-4A23-982B-D1D95F695F80' and VchTelefono = '" + Telefono + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                whatsAppPendientes = new WhatsAppPendientes()
                {
                    UidWhatsAppPendiente = Guid.Parse(item["UidWhatsAppPendiente"].ToString()),
                    VchUrl = item["VchUrl"].ToString(),
                    DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                    VchTelefono = item["VchTelefono"].ToString(),
                    UidPropietario = Guid.Parse(item["UidPropietario"].ToString()),
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    UidHistorialPago = Guid.Parse(item["UidHistorialPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    UidEstatusWhatsApp = Guid.Parse(item["UidEstatusWhatsApp"].ToString())
                };

                lsWhatsAppPendientes.Add(whatsAppPendientes);
            }

            return lsWhatsAppPendientes;
        }
        public bool ActualizarWhatsPendiente(Guid UidWhatsAppPendiente, Guid UidEstatusWhatsApp)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_WhatsAppPendientesActualizar";

                comando.Parameters.Add("@UidWhatsAppPendiente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidWhatsAppPendiente"].Value = UidWhatsAppPendiente;
                
                comando.Parameters.Add("@UidEstatusWhatsApp", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidEstatusWhatsApp"].Value = UidEstatusWhatsApp;

                Resultado = this.ManipulacionDeDatos(comando);
            }
            catch (Exception)
            {
                throw;
            }
            return Resultado;
        }

        public bool RegistrarWhatsPendiente(WhatsAppPendientes whatsAppPendientes)
        {
            bool Resultado = false;

            SqlCommand comando = new SqlCommand();
            try
            {
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_WhatsAppPendientesRegistrar";

                comando.Parameters.Add("@UidWhatsAppPendiente", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidWhatsAppPendiente"].Value = whatsAppPendientes.UidWhatsAppPendiente;

                comando.Parameters.Add("@VchUrl", SqlDbType.VarChar);
                comando.Parameters["@VchUrl"].Value = whatsAppPendientes.VchUrl;

                comando.Parameters.Add("@DtVencimiento", SqlDbType.Date);
                comando.Parameters["@DtVencimiento"].Value = whatsAppPendientes.DtVencimiento;

                comando.Parameters.Add("@VchTelefono", SqlDbType.VarChar);
                comando.Parameters["@VchTelefono"].Value = whatsAppPendientes.VchTelefono;
                
                comando.Parameters.Add("@UidPropietario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidPropietario"].Value = whatsAppPendientes.UidPropietario;
                
                comando.Parameters.Add("@UidUsuario", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidUsuario"].Value = whatsAppPendientes.UidUsuario;
                
                comando.Parameters.Add("@UidHistorialPago", SqlDbType.UniqueIdentifier);
                comando.Parameters["@UidHistorialPago"].Value = whatsAppPendientes.UidHistorialPago;

                comando.Parameters.Add("@VchDescripcion", SqlDbType.VarChar);
                comando.Parameters["@VchDescripcion"].Value = whatsAppPendientes.VchDescripcion;

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

        public int WhatsPendientes(Guid UidHistorialPago)
        {
            int result = 0;

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select COUNT(UidHistorialPago) as Total from WhatsAppPendientes where UidHistorialPago = '" + UidHistorialPago + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                result = int.Parse(item["Total"].ToString());
            }

            return result;
        }
        #endregion

        #region Metodos PayCardController
        public List<WhatsAppPendientes> ObtenerWhatsPntHistPago(Guid UidPropietario)
        {
            List<WhatsAppPendientes> lsWhatsAppPendientes = new List<WhatsAppPendientes>();

            SqlCommand query = new SqlCommand();
            query.CommandType = CommandType.Text;

            query.CommandText = "select wp.*, pt.Prefijo from WhatsAppPendientes wp, Usuarios us, TelefonosUsuarios tu, PrefijosTelefonicos pt where wp.UidUsuario = us.UidUsuario and us.UidUsuario = tu.UidUsuario and tu.UidPrefijo = pt.UidPrefijo and  UidEstatusWhatsApp = '06BDD7B7-C463-4A23-982B-D1D95F695F80' and UidPropietario = '" + UidPropietario + "'";

            DataTable dt = this.Busquedas(query);

            foreach (DataRow item in dt.Rows)
            {
                whatsAppPendientes = new WhatsAppPendientes()
                {
                    UidWhatsAppPendiente = Guid.Parse(item["UidWhatsAppPendiente"].ToString()),
                    VchUrl = item["VchUrl"].ToString(),
                    DtVencimiento = DateTime.Parse(item["DtVencimiento"].ToString()),
                    VchTelefono = "(" + item["Prefijo"].ToString() + ")"+ item["VchTelefono"].ToString(),
                    UidPropietario = Guid.Parse(item["UidPropietario"].ToString()),
                    UidUsuario = Guid.Parse(item["UidUsuario"].ToString()),
                    UidHistorialPago = Guid.Parse(item["UidHistorialPago"].ToString()),
                    VchDescripcion = item["VchDescripcion"].ToString(),
                    UidEstatusWhatsApp = Guid.Parse(item["UidEstatusWhatsApp"].ToString())
                };

                lsWhatsAppPendientes.Add(whatsAppPendientes);
            }

            return lsWhatsAppPendientes;
        }
        #endregion
    }
}
