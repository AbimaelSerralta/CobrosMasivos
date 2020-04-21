using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasUrlsConstruirLigaModel : LigasUrls
    {
        public string VchNombreComercial { get; set; }
        public string VchNombre { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string NombreCompleto { get { return VchNombre + " " + VchApePaterno + " " + VchApeMaterno; } }
    }
}
