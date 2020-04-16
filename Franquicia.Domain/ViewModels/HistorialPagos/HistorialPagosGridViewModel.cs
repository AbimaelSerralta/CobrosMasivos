using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class HistorialPagosGridViewModel : HistorialPagos
    {
        public DateTime DtRegistro { get; set; }
        public string VchIdentificador { get; set; }
    }
}
