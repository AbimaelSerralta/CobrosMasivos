using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.IntegracionesClubPago
{
    public class EndPointClubPago
    {
        public Guid UidEndPoint { get; set; }
        public string VchEndPoint { get; set; }
        public Guid UidTipoEndPoint { get; set; }
        public Guid UidPropietario { get; set; }
    }
}
