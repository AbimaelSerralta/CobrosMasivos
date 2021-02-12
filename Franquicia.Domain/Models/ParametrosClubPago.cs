using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ParametrosClubPago
    {
        public Guid UidParametro { get; set; }
        public string VchUrlAuth { get; set; }
        public string VchUrlGenerarRef { get; set; }
        public string VchUser { get; set; }
        public string VchPass { get; set; }
    }
}
