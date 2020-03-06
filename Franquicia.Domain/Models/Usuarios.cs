using Franquicia.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class Usuarios
    {
        public Guid UidUsuario { get; set; }
        public string StrNombre { get; set; }
        public string StrApePaterno { get; set; }
        public string StrApeMaterno { get; set; }
        public string StrCorreo { get; set; }
        public Guid UidEstatus { get; set; }
        public int IdUsuario { get; set; }
    }
}
