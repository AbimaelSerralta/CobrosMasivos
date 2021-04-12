using Franquicia.DataAccess.Repository.IntegracionesClubPago;
using Franquicia.Domain.Models.IntegracionesClubPago;
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

        public List<RefPagosClubPago> lsRefPagosClubPago = new List<RefPagosClubPago>();

        public bool RegistrarPagoClubPago(Guid UidPago, string IdReferencia, DateTime FechaRegistro, DateTime FechaOperacion, decimal Monto, string Transaccion, string Autorizacion, Guid UidPagoEstatus)
        {
            return refPagosClubPago.RegistrarPagoClubPago(UidPago, IdReferencia, FechaRegistro, FechaOperacion, Monto, Transaccion, Autorizacion, UidPagoEstatus);
        }
        public bool EliminarPagoClubPago(int Autorizacion, decimal Monto, string Transaccion, string IdReferencia)
        {
            return refPagosClubPago.EliminarPagoClubPago(Autorizacion, Monto, Transaccion, IdReferencia);
        }


        #region Metodos Web
        #region CheckReference
        public List<RefPagosClubPago> ObtenerPagoReferencia(Guid UidIntegracion, string IdReferencia)
        {
            lsRefPagosClubPago = new List<RefPagosClubPago>();

            return lsRefPagosClubPago = refPagosClubPago.ObtenerPagoReferencia(UidIntegracion, IdReferencia);
        }
        #endregion
        #endregion
    }
}
