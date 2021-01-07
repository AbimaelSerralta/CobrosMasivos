using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.ClubPago
{
    public class GenerarRefereciaPago
    {
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Account { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string ExpirationDate { get; set; }
    }
}
