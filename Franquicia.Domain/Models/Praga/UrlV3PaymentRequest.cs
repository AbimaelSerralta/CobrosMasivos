using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.Praga
{
    public class UrlV3PaymentRequest
    {
        public decimal ammount { get; set; }
        public string businessId { get; set; }
        public string currency { get; set; }
        public string effectiveDate { get; set; }
        public string id { get; set; }
        public string paymentTypes { get; set; }
        public string reference { get; set; }
        public string station { get; set; }
        public string userCode { get; set; }
        public string[] valuePairs { get; set; }
    }
}
