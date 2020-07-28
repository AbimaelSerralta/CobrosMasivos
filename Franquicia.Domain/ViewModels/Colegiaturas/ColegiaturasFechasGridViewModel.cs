using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class ColegiaturasFechasGridViewModel
    {
        public Guid UidFechaColegiatura { get; set; }
        public int IntNumero { get; set; }
        public DateTime DtFHInicio { get; set; }
        public string VchFHLimite { get; set; }
        public DateTime DtFHLimite { get; set; }
        public string VchFHVencimiento { get; set; }
        public DateTime DtFHVencimiento { get; set; }
        public Guid UidColegiatura { get; set; }
    }
}
