using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class PrefijosTelefonicos
    {
        public Guid UidPrefijo { get; set; }
        public string VchCodISO2 { get; set; }
        public string VchCodISO3 { get; set; }
        public string VchPais { get; set; }
        public string VchCapital { get; set; }
        public string Prefijo { get; set; }
        public string VchCompleto { get { return VchPais + " (" + Prefijo + ")"; } }
    }
}
