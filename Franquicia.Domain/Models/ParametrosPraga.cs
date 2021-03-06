using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ParametrosPraga
    {
        public Guid UidParametro { get; set; }
        public string BusinessId { get; set; }
        public string VchUrl { get; set; }
        public string UserCode { get; set; }
        public string WSEncryptionKey { get; set; }
        public string APIKey { get; set; }
        public string Currency { get; set; }
        public Guid UidPropietario { get; set; }
    }
}
