using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ComisionesTarjetasClientes
    {
        public Guid UidComicionTarjetaCliente { get; set; }
        public bool BitComision { get; set; }
        public decimal DcmComision { get; set; }
        public Guid UidCliente { get; set; }           
    }
}
