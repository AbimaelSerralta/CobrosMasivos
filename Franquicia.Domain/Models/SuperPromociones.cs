using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class SuperPromociones
    {
        public Guid UidSuperPromocion { get; set; }
        public Guid UidPromocion { get; set; }
        public decimal DcmComicion { get; set; }
        public decimal DcmApartirDe { get; set; }
    }
}
