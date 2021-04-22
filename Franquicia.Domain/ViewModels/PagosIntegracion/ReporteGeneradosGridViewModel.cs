using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class ReporteGeneradosGridViewModel : PagosIntegracion
    {
        public DateTime DtRegistro { get; set; }
        public string IdReferencia { get; set; }
        public DateTime DtVencimiento { get; set; }
        public string VchFormaPago { get; set; }
    }
}
