using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Accesos
    {
        public Guid UidUsuario { get; set; }
        public Guid UidPerfil { get; set; }
        public Guid UidModulo { get; set; }
        public string VchURL { get; set; }
    }
}
