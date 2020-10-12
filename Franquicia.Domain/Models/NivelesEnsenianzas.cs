using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class NivelesEnsenianzas
    {
        public Guid UidNivelEnsenianza { get; set; }
        public string VchDescripcion { get; set; }
        public int IntGerarquia { get; set; }
        public Guid UidTipoEnsenianza { get; set; }
    }
}
