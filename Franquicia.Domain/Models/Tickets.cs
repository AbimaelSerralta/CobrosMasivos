using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Tickets
    {
        public Guid UidTicket { get; set; }
        public string VchFolio { get; set; }
        public decimal DcmImporte { get; set; }
        public decimal DcmDescuento { get; set; }
        public decimal DcmTotal { get; set; }
        public string VchDescripcion { get; set; }
        public Guid UidPropietario { get; set; }
        public DateTime DtRegistro { get; set; }
        public Guid UidHistorialPago { get; set; }
        public int IntWA { get; set; }
        public int IntSMS { get; set; }
        public int IntCorreo { get; set; }
    }
}
