using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class PagosClubPago
    {
        public Guid UidPago { get; set; }
        public string IdReferencia { get; set; }
        public DateTime DtFechaRegistro { get; set; }
        public DateTime DtFechaOperacion { get; set; }
        public decimal DcmMonto { get; set; }
        public string VchTransaccion { get; set; }
        public string VchAutorizacion { get; set; }
        public string VchBanco { get; set; }
        public string VchCuenta { get; set; }
        public Guid UidPagoEstatus { get; set; }
    }
}
