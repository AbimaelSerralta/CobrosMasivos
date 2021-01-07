using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PagosManualesReporteEscuelaViewModel : PagosManuales
    {
        public string VchBanco { get; set; }
        
        public Guid UidEstatusFechaPago { get; set; }
        public string VchFormaPago { get; set; }
    }
}
