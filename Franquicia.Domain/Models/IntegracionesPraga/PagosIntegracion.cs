using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.IntegracionesPraga
{
    public class PagosIntegracion
    {
        public Guid UidPagoIntegracion { get; set; }
        public int IntFolio { get; set; }
        public decimal DcmImporte { get; set; }
        public decimal DcmImportePagado { get; set; }
        public decimal DcmImporteNuevo { get; set; }
        public Guid UidFormaPago { get; set; }
        public Guid UidEstatusFechaPago { get; set; }
    }
}
