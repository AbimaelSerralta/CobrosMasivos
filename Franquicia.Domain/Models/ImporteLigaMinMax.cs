using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ImporteLigaMinMax
    {
        public Guid UidImporteLiga { get; set; }
        public decimal DcmImporteMin { get; set; }
        public decimal DcmImporteMax { get; set; }
    }
}
