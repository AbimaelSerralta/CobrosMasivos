using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TelefonosUsuarios
    {
        public Guid UidTelefonoUsuario { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidUsuario { get; set; }
        public Guid UidTipoTelefono { get; set; }
    }
}
