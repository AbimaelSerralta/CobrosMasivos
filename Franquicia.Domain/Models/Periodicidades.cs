using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Periodicidades
    {
        public Guid UidPeriodicidad { get; set; }
        public string VchDescripcion { get; set; }
        public int IntGerarquia { get; set; }
    }
}
