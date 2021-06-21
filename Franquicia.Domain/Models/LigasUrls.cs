using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class LigasUrls
    {
        public Guid UidLigaUrl { get; set; }
        public string VchUrl { get; set; }
        public string VchConcepto { get; set; }
        public string IdReferencia { get; set; }
        public Guid UidUsuario { get; set; }
        public string VchIdentificador { get; set; }
        public DateTime DtRegistro { get; set; }
        public DateTime DtVencimiento { get; set; }
        public Decimal DcmImporte { get; set; }
        public string VchAsunto { get; set; }
        public Guid UidLigaAsociado { get; set; }
        public Guid UidPromocion { get; set; }
        public Guid UidEvento { get; set; }
        public Guid UidPropietario { get; set; }
        public decimal DcmComisionBancaria { get; set; }
        public decimal DcmPromocionDePago { get; set; }
        public decimal DcmTotal { get; set; }
    }
}
