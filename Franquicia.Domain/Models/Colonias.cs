using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Colonias
    {
        public Guid UidColonia { get; set; }
        public Guid UidCiudad { get; set; }
        public string VchColonia { get; set; }
        public string VchCodigoPostal { get; set; }
        public int IntOrden { get; set; }
    }
}
