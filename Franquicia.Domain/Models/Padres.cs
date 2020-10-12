using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Padres : UsuariosCompletos
    {
        public string VchMatricula { get; set; }
        public string StrTelefono { get; set; }
        public int IntCantAlumnos { get; set; }
        public string ColorNotification { get; set; }
    }
}
