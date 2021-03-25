using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class AlumnosFiltrosGridViewModel : Alumnos
    {
        public string Alumno { get { return VchMatricula + ": " + VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }

    }
}
