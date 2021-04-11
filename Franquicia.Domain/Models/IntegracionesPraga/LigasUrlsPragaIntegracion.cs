using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.IntegracionesPraga
{
    public class LigasUrlsPragaIntegracion
    {
        public Guid UidLigaUrl { get; set; }
        public int IdIntegracion { get; set; }
        public int IdEscuela { get; set; }
        public int IdComercio { get; set; }
        public string VchUrl { get; set; }
        public string VchConcepto { get; set; }
        public string IdReferencia { get; set; }
        public DateTime DtRegistro { get; set; }
        public DateTime DtVencimiento { get; set; }
        public decimal DcmImporte { get; set; }
        public string VchFormaPago { get; set; }
        public Guid UidTipoTarjeta { get; set; }
        public Guid UidPromocion { get; set; }
        public Guid UidPagoIntegracion { get; set; }
    }
}
