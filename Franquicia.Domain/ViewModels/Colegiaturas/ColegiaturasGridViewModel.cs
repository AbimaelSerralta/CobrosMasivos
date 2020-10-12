using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class ColegiaturasGridViewModel: Colegiaturas
    {
        public string VchFHLimite { get; set; }
        public string VchFHVencimiento { get; set; }
        public string VchDescripcion { get; set; }
        public string VchEstatus { get; set; }
        public string VchIconoEstatus { get; set; }
        public bool blEditar { get; set; }
    }
}
