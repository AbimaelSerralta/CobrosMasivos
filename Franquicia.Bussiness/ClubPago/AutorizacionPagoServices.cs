using Franquicia.DataAccess.Repository.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class AutorizacionPagoServices
    {
        private AutorizacionPagoRepository _autorizacionPagoRepository = new AutorizacionPagoRepository();
        public AutorizacionPagoRepository autorizacionPagoRepository
        {
            get { return _autorizacionPagoRepository; }
            set { _autorizacionPagoRepository = value; }
        }

        public AutorizacionPago AutorizacionPagoClubPago(string IdReferencia, DateTime FechaRegistro, string Fecha, decimal Monto, string Transaccion)
        {
            return autorizacionPagoRepository.AutorizacionPagoClubPago(IdReferencia, FechaRegistro, Fecha, Monto, Transaccion);
        }
        
        public bool RegistrarPagoClubPago(Guid UidPago, string IdReferencia, DateTime FechaRegistro, DateTime FechaOperacion, decimal Monto, string Transaccion, string Autorizacion, Guid UidPagoEstatus)
        {
            return autorizacionPagoRepository.RegistrarPagoClubPago(UidPago, IdReferencia, FechaRegistro, FechaOperacion, Monto, Transaccion, Autorizacion, UidPagoEstatus);
        }
    }
}
