using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class ObtenerRefereciaPago
    {
        public string Reference { get; set; }
        public string BarCode { get; set; }
        public string PayFormat { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string Folio { get; set; }
        public string Date { get; set; }
    }
}
