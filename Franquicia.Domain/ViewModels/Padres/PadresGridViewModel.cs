using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PadresGridViewModel : Usuarios
    {
        public string VchMatricula { get; set; }
        public string NombreCompleto { get { return StrNombre + " " + StrApePaterno + " " + StrApeMaterno; } }
        public Guid UidPrefijo { get; set; }
        public string StrTelefono { get; set; }
    }
}
