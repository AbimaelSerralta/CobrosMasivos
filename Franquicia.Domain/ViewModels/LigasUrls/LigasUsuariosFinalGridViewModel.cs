using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasUsuariosFinalGridViewModel : LigasUrls
    {
        public string VchEstatus { get; set; }
        public string VchEstatusIcono { get; set; }
        public string VchColor { get; set; }
        public decimal DcmImportePromocion { get; set; }
        public string VchPromocion { get; set; }
        public string VchPromocionIcono { get; set; }
        public string VchNombre { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string NombreCompleto { get { return VchNombre + " " + VchApePaterno + " " + VchApeMaterno; } }
        public bool blPagar { get; set; }
        public bool blOcultarPagar { get; set; }
        public string VchNombreComercial { get; set; }

        public decimal Comisiones { get { return DcmComisionBancaria + DcmPromocionDePago; } }
    }
}
