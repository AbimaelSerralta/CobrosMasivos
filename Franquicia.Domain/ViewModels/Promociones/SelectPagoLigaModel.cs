using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class SelectPagoLigaModel : Promociones
    {
        public string IdReferencia { get; set; }
        public string VchConcepto { get; set; }
        public DateTime DtVencimiento { get; set; }
        public string VchUrl { get; set; }
        public Decimal DcmImporte { get; set; }
    }
}
