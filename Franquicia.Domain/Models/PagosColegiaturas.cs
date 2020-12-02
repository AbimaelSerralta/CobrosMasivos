using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class PagosColegiaturas
    {
        public Guid UidPagoColegiatura { get; set; }
        public int IntFolio { get; set; }
        public DateTime DtFHPago { get; set; }
        public string VchPromocionDePago { get; set; }
        public string VchComisionBancaria { get; set; }
        public bool BitSubtotal { get; set; }
        public decimal DcmSubtotal { get; set; }
        public bool BitComisionBancaria { get; set; }
        public decimal DcmComisionBancaria { get; set; }
        public bool BitPromocionDePago { get; set; }
        public decimal DcmPromocionDePago { get; set; }
        public bool BitValidarImporte { get; set; }
        public decimal DcmValidarImporte { get; set; }
        public decimal DcmTotal { get; set; }
        public Guid UidUsuario { get; set; }
        public Guid UidEstatusPagoColegiatura { get; set; }
    }
}
