using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ClienteCuenta
    {
        public Guid UidClienteCuenta { get; set; }
        public decimal DcmDineroCuenta { get; set; }
        public Guid UidCliente { get; set; }
    }
}
