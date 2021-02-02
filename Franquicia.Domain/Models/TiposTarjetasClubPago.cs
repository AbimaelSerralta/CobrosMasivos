using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TiposTarjetasClubPago
    {
        public Guid UidTipoTarjeta { get; set; }
        public string VchDescripcion { get; set; }
        public bool BitPromociones { get; set; }
        public int IntGerarquia { get; set; }
        public string VchImagen { get; set; }
        public string VchColor { get; set; }
    }
}
