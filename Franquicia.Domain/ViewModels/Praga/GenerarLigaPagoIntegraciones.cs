using Franquicia.Domain.Models.Praga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels.Praga
{
    public class GenerarLigaPagoIntegraciones : UrlV3PaymentRequest
    {
        public string integrationID { get; set; }
        public string schoolID { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public string UidTipoPagoIntegracion { get; set; }
    }
}
