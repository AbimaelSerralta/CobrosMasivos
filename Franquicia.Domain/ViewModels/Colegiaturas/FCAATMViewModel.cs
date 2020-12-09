using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class FCAATMViewModel
    {
        public Guid UidFechaColegiaturaAlumno { get; set; }
        public Guid UidFechaColegiatura { get; set; }
        public Guid UidAlumno { get; set; }
        public DateTime DtFechaPago { get; set; }
        public decimal DcmImporteResta { get; set; }

        public string FHInicio { get; set; }
        public string FHLimite { get; set; }
        public string FHVencimiento { get; set; }
        public string EstatusFechaColegiatura { get; set; }
        public Guid UidEstatusFechaColegiatura { get; set; }
    }
}
