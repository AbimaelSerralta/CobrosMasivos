using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PagosColegiaturasViewModel : Colegiaturas
    {
        public string VchMatricula { get; set; }
        public string VchNombreComercial { get; set; }

        public string VchNombres { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string NombreCompleto { get { return VchNombres + " " + VchApePaterno + " " + VchApeMaterno; } }
        public bool BitBeca { get; set; }
        public string VchTipoBeca { get; set; }
        public decimal DcmBeca { get; set; }

        public Guid UidFechaColegiatura { get; set; }
        public int IntNum { get; set; }
        public string VchNum { get; set; }
        public string VchFHLimite { get; set; }
        public string VchFHVencimiento { get; set; }
        public DateTime DtFHFinPeriodo { get; set; }
        public string VchDescripcion { get; set; }
        public Guid UidEstatusFechaColegiatura { get; set; }
        
        
        public string VchEstatusFechas { get; set; }
        public string VchColor { get; set; }
        
        public bool blPagar { get; set; }
        public string VchPeriodicidad { get; set; }
    }
}
