using Franquicia.DataAccess.Repository.IntegracionesClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesClubPago
{
    public class RefClubPagoServices
    {
        private RefClubPagoRepository _refClubPagoRepository = new RefClubPagoRepository();
        public RefClubPagoRepository refClubPagoRepository
        {
            get { return _refClubPagoRepository; }
            set { _refClubPagoRepository = value; }
        }

        public bool RegistrarReferencia(string VchFolio, int IdIntegracion, int IdEscuela, string VchUrl, string VchCodigoBarra, string VchConcepto, string IdReferencia, string VchCuenta, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchEmail, string VchUsuario, Guid UidPagoIntegracion)
        {
            Guid UidReferencia = Guid.NewGuid();

            bool result = false;
            if (refClubPagoRepository.RegistrarReferencia(UidReferencia, VchFolio, IdIntegracion, IdEscuela, VchUrl, VchCodigoBarra, VchConcepto, IdReferencia, VchCuenta, DtRegistro, DtVencimiento, DcmImporte, VchEmail, VchUsuario, UidPagoIntegracion))
            {
                result = true;
            }
            return result;
        }
    }
}
