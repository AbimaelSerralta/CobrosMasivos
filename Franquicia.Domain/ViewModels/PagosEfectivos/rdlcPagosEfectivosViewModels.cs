using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class rdlcPagosEfectivosViewModels : PagosEfectivos
    {
        public string VchMatricula { get; set; }
        public string NombreCompleto { get; set; }
        public decimal DcmTotal { get; set; }
        public string FormaPago { get; set; }
        public string TipoTarjeta { get; set; }
        public string PromocionTerminal { get; set; }
    }
}
