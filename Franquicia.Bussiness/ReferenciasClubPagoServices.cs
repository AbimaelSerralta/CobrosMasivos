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
    public class ReferenciasClubPagoServices
    {
        private ReferenciasClubPagoRepository _referenciasClubPagoRepository = new ReferenciasClubPagoRepository();
        public ReferenciasClubPagoRepository referenciasClubPagoRepository
        {
            get { return _referenciasClubPagoRepository; }
            set { _referenciasClubPagoRepository = value; }
        }

        #region Metodos Escuela

        #region Pagos Padres
        public bool GenerarReferenciaPagosColegiatura(string VchFolio, string VchUrl, string VchCodigoBarra, string VchConcepto, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, decimal DcmPagado, decimal DcmTotal, string VchAsunto, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            Guid UidReferencia = Guid.NewGuid();

            bool result = false;
            if (referenciasClubPagoRepository.GenerarReferenciaPagosColegiatura(UidReferencia, VchFolio, VchUrl, VchCodigoBarra, VchConcepto, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, DcmImporte, DcmPagado, DcmTotal, VchAsunto, UidPagoColegiatura, UidPropietario))
            {
                result = true;
            }
            return result;
        }
        public Tuple<List<ReferenciasClubPago>, string, bool> ReimprimirReferenciaPagoColegiatura(Guid UidPagoColegiatura)
        {                       
            return referenciasClubPagoRepository.ReimprimirReferenciaPagoColegiatura(UidPagoColegiatura);
        }
        #endregion

        #endregion
    }
}
