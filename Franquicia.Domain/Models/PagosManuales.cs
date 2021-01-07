using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class PagosManuales
    {
        public Guid UidPagoManual { get; set; }
        public Guid UidBanco { get; set; }
        public string VchCuenta { get; set; }
        public DateTime DtFHPago { get; set; }
        public decimal DcmImporte { get; set; }
        public string VchFolio { get; set; }
        public Guid UidPagoColegiatura { get; set; }

    }
}
