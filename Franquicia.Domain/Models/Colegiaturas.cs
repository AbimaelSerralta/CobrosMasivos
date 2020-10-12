using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Colegiaturas
    {
        public Guid UidColegiatura { get; set; }
        public string VchIdentificador { get; set; }
        public decimal DcmImporte { get; set; }
        public int IntCantPagos { get; set; }
        public Guid UidPeriodicidad { get; set; }
        public DateTime DtFHInicio { get; set; }
        public bool BitFHLimite { get; set; }
        public DateTime DtFHLimite { get; set; }
        public bool BitFHVencimiento { get; set; }
        public DateTime DtFHVencimiento { get; set; }
        public bool BitRecargo { get; set; }
        public string VchTipoRecargo { get; set; }
        public decimal DcmRecargo { get; set; }
        public string VchDcmRecargo { get; set; }
        public bool BitRecargoPeriodo { get; set; }
        public string VchTipoRecargoPeriodo { get; set; }
        public decimal DcmRecargoPeriodo { get; set; }
        public string VchDcmRecargoPeriodo { get; set; }
        public Guid UidEstatus { get; set; }
        public Guid UidCliente { get; set; }
    }
}
