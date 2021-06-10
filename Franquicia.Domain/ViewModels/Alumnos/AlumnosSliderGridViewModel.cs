using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class AlumnosSliderGridViewModel : Alumnos
    {
        public string Alumno { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }
        public bool blSelect { get; set; }
        public string VchColor { get; set; }
        public string Avatar { get; set; }
        public bool blActivarCorto { get; set; }
        public bool blActivarAvatar { get; set; }

        public string VchNombreCorto { get { return VchNombres.Substring(0, 1) + VchApePaterno.Substring(0, 1); } }

    }
}
