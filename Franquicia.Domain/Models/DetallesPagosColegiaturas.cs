using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class DetallesPagosColegiaturas
    {
        public Guid UidDetallePagoColegiatura { get; set; }
        public int IntNum { get; set; }
        public string VchDescripcion { get; set; }
        public decimal DcmImporte { get; set; }
        public Guid UidPagoColegiatura { get; set; }
    }
}
