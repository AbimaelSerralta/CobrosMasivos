using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ReferenciasClubPago
    {
        public Guid UidReferencia { get; set; }
        public string VchFolio { get; set; }
        public string VchUrl { get; set; }
        public string VchCodigoBarra { get; set; }
        public string VchConcepto { get; set; }
        public string IdReferencia { get; set; }
        public Guid UidUsuario { get; set; }
        public string VchIdentificador { get; set; }
        public DateTime DtRegistro { get; set; }
        public DateTime DtVencimiento { get; set; }
        public Decimal DcmImporte { get; set; }
        public Decimal DcmPagado { get; set; }
        public Decimal DcmTotal { get; set; }
        public string VchAsunto { get; set; }
        public Guid UidPagoColegiatura { get; set; }
        public Guid UidPropietario { get; set; }
    }
}
