using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Permisos
    {
        public Guid UidPermiso { get; set; }
        public string VchDescripcion { get; set; }
        public Guid UidSegPerfil { get; set; }
        public Guid UidSegModulo { get; set; }
        public string VchIcono { get; set; }
    }
}
