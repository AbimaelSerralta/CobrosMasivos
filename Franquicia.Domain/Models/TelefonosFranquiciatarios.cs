using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TelefonosFranquiciatarios
    {
        public Guid UidTelefonoFranquiciatarios { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidFranquiciatario { get; set; }
        public Guid UidTipoTelefono { get; set; }
    }
}
