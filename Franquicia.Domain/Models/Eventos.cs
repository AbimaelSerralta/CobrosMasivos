using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Eventos
    {
        public Guid UidEvento { get; set; }
        public string VchNombreEvento { get; set; }
        public string VchDescripcion { get; set; }
        public DateTime DtRegistro { get; set; }
        public DateTime DtFHInicio { get; set; }
        public DateTime DtFHFin { get; set; }
        public bool BitTipoImporte { get; set; }
        public decimal DcmImporte { get; set; }
        public string VchConcepto { get; set; }
        public bool BitDatosUsuario { get; set; }
        public string VchUrlEvento { get; set; }
        public Guid UidPropietario { get; set; }
        public Guid UidEstatus { get; set; }
        public bool BitFHFin { get; set; }
        public Guid UidTipoEvento { get; set; }
        public bool BitDatosBeneficiario { get; set; }
    }
}
