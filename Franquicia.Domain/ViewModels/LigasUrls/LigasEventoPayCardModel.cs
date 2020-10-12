using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasEventoPayCardModel : LigasUrls
    {
        public string VchNombreComercial { get; set; }
        public string VchPago { get; set; }
    }
}
