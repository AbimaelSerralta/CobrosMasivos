using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasUrlsDetalleGridViewModel : LigasUrls
    {
        public int IntNum { get; set; }
        public string VchNombre { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string NombreCompleto { get { return VchNombre + " " + VchApePaterno + " " + VchApeMaterno; } }
        public string FechaPago { get; set; }
        public decimal DcmImportePromocion { get; set; }
        public string VchPromocion { get; set; }
    }
}
