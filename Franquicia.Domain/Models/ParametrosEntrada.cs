using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ParametrosEntrada
    {
        public Guid UidParametroEntrada { get; set; }
        public string IdCompany { get; set; }
        public string IdBranch { get; set; }
        public string VchModena { get; set; }
        public string VchUsuario { get; set; }
        public string VchPassword { get; set; }
        public string VchCanal { get; set; }
        public string VchData0 { get; set; }
        public string VchUrl { get; set; }
        public string VchSemillaAES { get; set; }
        public Guid UidPropietario { get; set; }
        public bool BitImporteLiga { get; set; }
        public decimal DcmImporteMin { get; set; }
        public decimal DcmImporteMax { get; set; }
    }
}
