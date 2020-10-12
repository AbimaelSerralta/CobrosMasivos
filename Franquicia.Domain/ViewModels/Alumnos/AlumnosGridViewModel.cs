using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class AlumnosGridViewModel : Alumnos
    {
        public string NombreCompleto { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }
        public bool blSeleccionado { get; set; }
        public bool blVisibleDesasociar { get; set; }
        public string VchDescripcion { get; set; }
        public string VchIcono { get; set; }
        public string VchBeca { get; set; }
        public Guid UidPrefijo { get; set; }
        public string VchTelefono { get; set; }
        
        public string VchEstatus { get; set; }
        
        public string VchDescripcionAsociado { get; set; }
        public string VchIconoAsociado { get; set; }
        public int IntCantPadres { get; set; }
        public string ColorNotification { get; set; }
    }
}
