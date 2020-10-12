using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Tarifas
    {
        public Guid UidTarifa { get; set; }
        public decimal DcmWhatsapp { get; set; }
        public decimal DcmSms { get; set; }
        public Guid UidEstatus { get; set; }
    }
}
