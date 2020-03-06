using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasUrlsGridViewModel : LigasUrls
    {
        public string VchEstatus { get; set; }
        public string VchColor { get; set; }
    }
}
