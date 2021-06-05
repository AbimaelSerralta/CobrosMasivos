using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class ColegiaturasAlumnosViewModel : Colegiaturas
    {
        public Guid UidAlumno { get; set; }
        public string Alumno { get; set; }
        public string VchNombreCorto { get; set; }
    }
}
