using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Integraciones
    {
        public Guid UidIntegracion {get; set;}
        public int IdIntegracion { get; set;}
        public string VchIdentificador { get; set;}
        public Guid UidEstatus { get; set;}
    }
}
