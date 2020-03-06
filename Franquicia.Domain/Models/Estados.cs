using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Estados
    {
        public Guid UidEstado { get; set; }
        public Guid UidPais { get; set; }
        public string VchEstado { get; set; }
        public int IntOrden { get; set; }
    }
}
