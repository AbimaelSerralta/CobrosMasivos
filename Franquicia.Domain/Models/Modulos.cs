using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Modulos
    {
        public Guid UidSegModulo { get; set; }
        public string VchNombre { get; set; }
        public Guid UidAppWeb { get; set; }
        public string VchUrl { get; set; }
        public string VchIcono { get; set; }
        public int IntGerarquia { get; set; }
    }
}
