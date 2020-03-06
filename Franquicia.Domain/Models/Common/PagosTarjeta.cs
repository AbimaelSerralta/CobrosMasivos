using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models.Common
{
    public class PagosTarjeta
    {
        public Guid UidPagoTarjeta { get; set; }
        public string VchReferencia { get; set; }
        public string VchEstatus { get; set; }
        public int BiFolio { get; set; }
        public string VchTipoDeTarjeta { get; set; }
        public string VchTipoDeOperacion { get; set; }
        public string UidReferencia { get; set; }
        public DateTime DtmFechaDeRegistro { get; set; }
        public decimal MMonto { get; set; }
        public string Autorizacion { get; set; }
        public string nb_company { get; set; }
        public string nb_merchant { get; set; }
        public string id_url { get; set; }
        public string cd_error { get; set; }
        public string nb_error { get; set; }
        public string cc_number { get; set; }
        public string cc_mask { get; set; }
        public string FolioPago { get; set; }
    }
}
