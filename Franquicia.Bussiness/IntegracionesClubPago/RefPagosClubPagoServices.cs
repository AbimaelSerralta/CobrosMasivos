using Franquicia.DataAccess.Repository.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesClubPago
{
    public class RefPagosClubPagoServices
    {
        private RefPagosClubPagoRepository _refPagosClubPagoRepository = new RefPagosClubPagoRepository();
        public RefPagosClubPagoRepository refPagosClubPago
        {
            get { return _refPagosClubPagoRepository; }
            set { _refPagosClubPagoRepository = value; }
        }

        public bool RegistrarPagoClubPago(Guid UidPago, string IdReferencia, DateTime FechaRegistro, DateTime FechaOperacion, decimal Monto, string Transaccion, string Autorizacion, Guid UidPagoEstatus)
        {
            return refPagosClubPago.RegistrarPagoClubPago(UidPago, IdReferencia, FechaRegistro, FechaOperacion, Monto, Transaccion, Autorizacion, UidPagoEstatus);
        }
    }
}
