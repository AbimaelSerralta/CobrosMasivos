using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class DireccionesFranquiciatarios
    {
        public Guid UidDireccionFranquiciatario { get; set; }
        public string Identificador { get; set; }
        public Guid UidPais { get; set; }
        public Guid UidEstado { get; set; }
        public Guid UidMunicipio { get; set; }
        public Guid UidCiudad { get; set; }
        public Guid UidColonia { get; set; }
        public string Calle { get; set; }
        public string EntreCalle { get; set; }
        public string YCalle { get; set; }
        public string NumeroExterior { get; set; }
        public string NumeroInterior { get; set; }
        public string CodigoPostal { get; set; }
        public string Referencia { get; set; }
        public Guid UidFranquiciatario { get; set; }
    }
}
