using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class WhatsAppPendientesServices
    {
        private WhatsAppPendientesRepository _whatsAppPendientesRepository = new WhatsAppPendientesRepository();
        public WhatsAppPendientesRepository whatsAppPendientesRepository
        {
            get { return _whatsAppPendientesRepository; }
            set { _whatsAppPendientesRepository = value; }
        }

        public List<WhatsAppPendientes> lsWhatsAppPendientes = new List<WhatsAppPendientes>();

        #region Metodos Franquicia

        #endregion

        #region Metodos Cliente
        public void ObtenerWhatsPendiente(string Telefono)
        {
            lsWhatsAppPendientes = whatsAppPendientesRepository.ObtenerWhatsPendiente(Telefono);
        }
        public bool ActualizarWhatsPendiente(Guid UidWhatsAppPendiente, Guid UidEstatusWhatsApp)
        {
            bool result = false;
            if (whatsAppPendientesRepository.ActualizarWhatsPendiente(UidWhatsAppPendiente, UidEstatusWhatsApp))
            {
                result = true;
            }
            return result;
        }

        public bool RegistrarWhatsPendiente(string Url, DateTime Vencimiento, string Telefono, Guid UidPropietario, Guid UidUsuario, Guid UidHistorialPago, string VchDescripcion)
        {
            bool result = false;
            if (whatsAppPendientesRepository.RegistrarWhatsPendiente(
                new WhatsAppPendientes
                {
                    UidWhatsAppPendiente = Guid.NewGuid(),
                    VchUrl = Url,
                    DtVencimiento = Vencimiento,
                    VchTelefono = Telefono,
                    UidPropietario = UidPropietario,
                    UidUsuario = UidUsuario,
                    UidHistorialPago = UidHistorialPago,
                    VchDescripcion = VchDescripcion
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarDineroCuentaCliente(decimal DineroCuenta, Guid UidCliente, string IdReferencia)
        {
            bool result = false;
            if (whatsAppPendientesRepository.ActualizarDineroCuentaCliente(
                new ClienteCuenta
                {
                    DcmDineroCuenta = DineroCuenta,
                    UidCliente = UidCliente
                }, IdReferencia
                ))
            {
                result = true;
            }
            return result;
        }

        public int WhatsPendientes(Guid UidHistorialPago)
        {
            return whatsAppPendientesRepository.WhatsPendientes(UidHistorialPago);
        }
        #endregion

        #region Metodos PayCardController
        public void ObtenerWhatsPntHistPago(Guid UidPropietario)
        {
            lsWhatsAppPendientes = whatsAppPendientesRepository.ObtenerWhatsPntHistPago(UidPropietario);
        }
        #endregion
    }
}
