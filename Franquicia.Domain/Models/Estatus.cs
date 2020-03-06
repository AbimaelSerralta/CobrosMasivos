using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Estatus
    {
        public Guid UidEstatus { get; set; }
        public string VchDescripcion { get; set; }
        public string VchIcono { get; set; }
    }
}
