using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PadresSelectAlumnosViewModel : Usuarios
    {
        public string NombreCompleto { get { return StrNombre + " " + StrApePaterno + " " + StrApeMaterno; } }
        public Guid UidPrefijo { get; set; }
        public string StrTelefono { get; set; }
        public bool blSeleccionadoTodo { get; set; }
        public bool blSeleccionado { get; set; }
    }
}
