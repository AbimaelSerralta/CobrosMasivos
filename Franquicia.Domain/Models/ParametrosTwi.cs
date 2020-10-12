using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class ParametrosTwi
    {
        public Guid UidParametroTwi { get; set; }
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string NumberFrom { get; set; }
    }
}
