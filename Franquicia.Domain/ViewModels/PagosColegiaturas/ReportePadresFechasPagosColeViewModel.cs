using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class ReportePadresFechasPagosColeViewModel : PagosColegiaturas
    {
        public string VchNombre { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string NombreCompleto { get { return VchNombre + " " + VchApePaterno + " " + VchApeMaterno; } }
        public Guid UidFormaPago { get; set; }
        public string VchFormaPago { get; set; }
        public decimal DcmImportePagado { get; set; }
        public string VchEstatus { get; set; }
        public string VchColor { get; set; }

    }
}
