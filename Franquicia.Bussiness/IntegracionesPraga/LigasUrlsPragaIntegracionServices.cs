using Franquicia.DataAccess.Repository.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesPraga
{
    public class LigasUrlsPragaIntegracionServices
    {
        private LigasUrlsPragaIntegracionRepository _ligasUrlsPragaIntegracion = new LigasUrlsPragaIntegracionRepository();
        public LigasUrlsPragaIntegracionRepository ligasUrlsPragaIntegracion
        {
            get { return _ligasUrlsPragaIntegracion; }
            set { _ligasUrlsPragaIntegracion = value; }
        }

        public bool RegistrarLiga(int IdIntegracion, int IdEscuela, int IdComercio, string VchUrl, string VchConcepto, string IdReferencia, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchFormaPago, Guid UidPagoIntegracion)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (ligasUrlsPragaIntegracion.RegistrarLiga(UidLigaUrl, IdIntegracion, IdEscuela, IdComercio, VchUrl, VchConcepto, IdReferencia, DtRegistro, DtVencimiento, DcmImporte, VchFormaPago, UidPagoIntegracion))
            {
                result = true;
            }
            return result;
        }
    }
}
