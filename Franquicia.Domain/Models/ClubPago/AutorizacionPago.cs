using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class AutorizacionPago
    {
        public int codigo { get; set; }
        public string autorizacion { get; set; }
        public string mensaje { get; set; }
        public string transaccion { get; set; }
        public string fecha { get; set; }
        public string notificacion_sms { get; set; }
        public string mensaje_sms { get; set; }
        public string mensaje_ticket { get; set; }        
    }
}
