using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class AlumnosRLEGridViewModel : Alumnos
    {
        public string NombreCompleto { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }
        public string VchDescripcion { get; set; }
        public string VchIcono { get; set; }
        public string VchBeca { get; set; }
        public Guid UidPrefijo { get; set; }
        public string VchTelefono { get; set; }
        public string VchEstatus { get; set; }

        public Guid UidUsuario { get; set; }
        public string TuNombre { get; set; }
        public string TuPaterno { get; set; }
        public string TuMaterno { get; set; }
        public string TuNombreCompleto { get { return TuNombre + " " + TuPaterno + " " + TuMaterno; } }
    }
}
