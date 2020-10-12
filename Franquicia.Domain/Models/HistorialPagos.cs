using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class HistorialPagos
    {
        public Guid UidHistorialPago { get; set; }
        public decimal DcmSaldo { get; set; }
        public decimal DcmOperacion { get; set; }
        public decimal DcmNuevoSaldo { get; set; }
        public string IdReferencia { get; set; }
        public Guid UidEstatusPago { get; set; }
    }
}
