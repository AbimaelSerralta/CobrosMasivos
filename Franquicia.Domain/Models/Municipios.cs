using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Municipios
    {
        public Guid UidMunicipio { get; set; }
        public Guid UidEstado { get; set; }
        public string VchMunicipio { get; set; }
        public int IntOrden { get; set; }
    }
}
