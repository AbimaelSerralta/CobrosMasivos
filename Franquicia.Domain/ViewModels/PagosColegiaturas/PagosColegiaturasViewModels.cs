using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PagosColegiaturasViewModels:PagosColegiaturas
    {
        public string VchMatricula { get; set; }
        public string VchNombres { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string VchAlumno { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }

        public decimal DcmImporteCole { get; set; }
    }
}
