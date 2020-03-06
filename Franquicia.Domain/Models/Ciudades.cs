using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Ciudades
    {
        public Guid UidCiudad { get; set; }
        public Guid UidMunicipio { get; set; }
        public string VchCiudad { get; set; }
        public int IntOrden { get; set; }
    }
}
