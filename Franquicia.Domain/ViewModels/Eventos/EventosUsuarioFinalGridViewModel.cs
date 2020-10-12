using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class EventosUsuarioFinalGridViewModel : Eventos
    {
        public string VchEstatus { get; set; }
        public string VchIcono { get; set; }
        public string VchNombreComercial { get; set; }
        public string VchTelefono { get; set; }
        public string VchCorreo { get; set; }
        public string VchFHFin { get; set; }
    }
}
