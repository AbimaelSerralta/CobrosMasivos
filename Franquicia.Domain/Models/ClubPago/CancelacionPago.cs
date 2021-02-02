using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class CancelacionPago
    {
        public string referencia { get; set; }
        public DateTime fecha { get; set; }
        public decimal monto { get; set; }
        public string transaccion { get; set; }
        public int autorizacion { get; set; }
    }
}
