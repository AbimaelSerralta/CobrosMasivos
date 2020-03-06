using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TelefonosClientes
    {
        public Guid UidTelefonoCliente { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidCliente { get; set; }
        public Guid UidTipoTelefono { get; set; }
    }
}
