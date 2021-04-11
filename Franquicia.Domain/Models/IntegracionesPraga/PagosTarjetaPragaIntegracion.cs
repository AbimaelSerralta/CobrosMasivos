using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.IntegracionesPraga
{
    public class PagosTarjetaPragaIntegracion
    {
        public Guid UidPagoTarjeta { get; set; }
        public string IdReferencia { get; set; }
        public string VchEstatus { get; set; }
        public string foliocpagos { get; set; }
        public string auth { get; set; }
        public string cd_response { get; set; }
        public string cd_error { get; set; }
        public string nb_error { get; set; }
        public DateTime DtmFechaDeRegistro { get; set; }
        public DateTime DtFechaOperacion { get; set; }
        public string nb_company { get; set; }
        public string nb_merchant { get; set; }
        public string cc_type { get; set; }
        public string tp_operation { get; set; }
        public string cc_name { get; set; }
        public string cc_number { get; set; }
        public string cc_expmonth { get; set; }
        public string cc_expyear { get; set; }
        public decimal amount { get; set; }
        public string emv_key_date { get; set; }
        public string id_url { get; set; }
        public string email { get; set; }
        public string payment_type { get; set; }
    }
}
