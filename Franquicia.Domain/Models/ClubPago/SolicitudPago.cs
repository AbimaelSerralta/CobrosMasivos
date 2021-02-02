using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class SolicitudPago
    {
        public string referencia { get; set; }
        public string fecha { get; set; }
        public string monto { get; set; }
        public string transaccion { get; set; }
    }
}
