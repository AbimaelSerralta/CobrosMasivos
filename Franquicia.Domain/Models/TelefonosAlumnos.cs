using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TelefonosAlumnos
    {
        public Guid UidTelefonoAlumno { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidAlumno { get; set; }
        public Guid UidTipoTelefono { get; set; }
        public Guid UidPrefijo { get; set; }
    }
}
