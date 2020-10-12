using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class DesglosePagosGridViewModel
    {
        public int IntNum { get; set; }
        public string VchConcepto { get; set; }
        public decimal DcmImporte { get; set; }
        public string VchCoResta { get; set; }
    }
}
