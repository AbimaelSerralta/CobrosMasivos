using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class PagosEfectivos
    {
        public Guid UidPagosEfectivos { get; set; }
        public Guid VchBanco { get; set; }
        public string VchCuenta { get; set; }
        public DateTime DtFHPago { get; set; }
        public decimal DcmImporte { get; set; }
        public string VchFolio { get; set; }
        public bool BitTipoTarjeta { get; set; }
        public Guid UidTipoTarjeta { get; set; }
        public bool BitPromocionTT { get; set; }
        public Guid UidPromocionTerminal { get; set; }
        public Guid UidPagoColegiatura { get; set; }

    }
}
