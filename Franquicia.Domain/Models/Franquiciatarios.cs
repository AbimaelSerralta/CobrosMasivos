using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Franquiciatarios
    {
        public Guid UidFranquiciatarios { get; set; }
        public string VchRFC  { get; set; }
        public string VchRazonSocial  { get; set; }
        public string VchNombreComercial  { get; set; }
        public DateTime DtFechaAlta { get; set; }
        public string VchCorreoElectronico { get; set; }
        public Guid UidEstatus { get; set; }

    }
}
