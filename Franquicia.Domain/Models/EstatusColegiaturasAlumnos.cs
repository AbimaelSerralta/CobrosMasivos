using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class EstatusColegiaturasAlumnos
    {
        public Guid UidEstatusColeAlumnos { get; set; }
        public string VchDescripcion { get; set; }
        public string VchColor { get; set; }
        public int IntGerarquia { get; set; }
    }
}
