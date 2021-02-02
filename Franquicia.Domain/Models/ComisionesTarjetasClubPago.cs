using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ComisionesTarjetasClubPago
    {
        public Guid UidComicionTarjeta { get; set; }
        public bool BitComision { get; set; }
        public decimal DcmComision { get; set; }
        public Guid UidTipoTarjeta { get; set; }
        public Guid UidCliente { get; set; }           
    }
}
