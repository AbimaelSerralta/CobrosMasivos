using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Perfiles
    {
        public Guid UidSegPerfil { get; set; }
        public string VchNombre { get; set; }
        public Guid UidAppWeb { get; set; }
        public Guid UidEstatus { get; set; }
        public Guid UidFranquiciatario { get; set; }
        public Guid UidModuloInicial { get; set; }
        public Guid UidTipoPerfil { get; set; }
        public Guid UidTipoPerfilFranquicia { get; set; }
        public Guid UidCliente { get; set; }
    }
}
