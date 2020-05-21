using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Clientes
    {
        public Guid UidCliente { get; set; }
        public string VchRFC  { get; set; }
        public string VchRazonSocial  { get; set; }
        public string VchNombreComercial  { get; set; }
        public DateTime DtFechaAlta { get; set; }
        public string VchCorreoElectronico { get; set; }
        public Guid UidEstatus { get; set; }
        public Guid UidFranquiciatario { get; set; }
        public int IdCliente { get; set; }
        public string VchIdWAySMS { get; set; }

    }
}
