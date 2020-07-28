using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class WhatsAppPendientes
    {
        public Guid UidWhatsAppPendiente { get; set; }
        public string VchUrl { get; set; }
        public DateTime DtVencimiento { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidPropietario { get; set; }
        public Guid UidUsuario { get; set; }
        public Guid UidHistorialPago { get; set; }
        public string VchDescripcion { get; set; }
        public Guid UidEstatusWhatsApp { get; set; }
    }
}
