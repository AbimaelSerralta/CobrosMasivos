using Franquicia.DataAccess.Repository.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class CancelacionPagoServices
    {
        private CancelacionPagoRepository _cancelacionPagoRepository = new CancelacionPagoRepository();
        public CancelacionPagoRepository cancelacionPagoRepository
        {
            get { return _cancelacionPagoRepository; }
            set { _cancelacionPagoRepository = value; }
        }

        public CancelacionPagoResp CancelacionPagoClubPago(string Transaccion, string Fecha, decimal Monto, string Referencia, int Autorizacion)
        {
            //Consulto la referencia, despues la cancelo si la encuentra
            return cancelacionPagoRepository.ConsultarReferenciaClubPago(Transaccion, Fecha, Monto, Referencia, Autorizacion);
        }
    }
}
