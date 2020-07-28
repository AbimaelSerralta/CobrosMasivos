using Franquicia.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Alumnos
    {
        public Guid UidAlumno { get; set; }
        public string VchIdentificador { get; set; }
        public string VchNombres { get; set; }
        public string VchApePaterno { get; set; }
        public string VchApeMaterno { get; set; }
        public string VchMatricula { get; set; }
        public string VchCorreo { get; set; }
        public bool BitBeca { get; set; }
        public string VchTipoBeca { get; set; }
        public decimal DcmBeca { get; set; }
        public Guid UidProcesoEnsenianza { get; set; }
        public Guid UidTipoEnsenianza { get; set; }
        public Guid UidNivelEnsenianza { get; set; }
        public Guid UidEstatus { get; set; }
        public Guid UidCliente { get; set; }
    }
}
