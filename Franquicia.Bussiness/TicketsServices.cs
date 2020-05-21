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
    public class TicketsServices
    {
        private TicketsRepository _ticketsRepository = new TicketsRepository();
        public TicketsRepository ticketsRepository
        {
            get { return _ticketsRepository; }
            set { _ticketsRepository = value; }
        }
        
        
        #region Metodos Franquicia
        
        #endregion

        #region Metodos Cliente
        public void ObtenerDineroCuentaCliente(Guid UidCliente)
        {
            ticketsRepository.ObtenerDineroCuentaCliente(UidCliente);
        }
        public bool RegistrarTicketPago(string VchFolio, decimal DcmImporte, decimal DcmDescuento, decimal DcmTotal, string VchDescripcion, Guid UidPropietario, DateTime DtRegistro, int IntWA, int IntSMS, int IntCorreo, decimal DcmSaldo, decimal DcmOperacion, decimal DcmNuevoSaldo)
        {
            bool result = false;
            if (ticketsRepository.RegistrarTicketPago(
                new Tickets
                {
                    UidTicket = Guid.NewGuid(),
                    VchFolio = VchFolio,
                    DcmImporte = DcmImporte,
                    DcmDescuento = DcmDescuento,
                    DcmTotal = DcmTotal,
                    VchDescripcion = VchDescripcion,
                    UidPropietario = UidPropietario,
                    DtRegistro = DtRegistro,
                    UidHistorialPago = Guid.NewGuid(),
                    IntSMS = IntSMS,
                    IntWA = IntWA,
                    IntCorreo = IntCorreo
                },
                new HistorialPagos
                {
                    DcmSaldo = DcmSaldo,
                    DcmOperacion = DcmOperacion,
                    DcmNuevoSaldo = DcmNuevoSaldo
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarTicketPago(decimal DineroCuenta, Guid UidCliente, string IdReferencia)
        {
            bool result = false;
            if (ticketsRepository.ActualizarTicketPago(
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
        #endregion
    }
}
