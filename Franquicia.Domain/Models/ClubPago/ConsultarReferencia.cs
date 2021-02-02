using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class ConsultarReferencia
    {
        public int codigo { get; set; }
        public string mensaje { get; set; }
        public string monto { get; set; }
        public string referencia { get; set; }
        public string transaccion { get; set; }
        public bool? parcial { get; set; }
    }
}
