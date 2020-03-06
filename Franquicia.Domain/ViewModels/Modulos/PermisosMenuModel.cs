using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PermisosMenuModel : Models.Modulos
    {
        public bool Lectura { get; set; }
        public bool Agregar { get; set; }
        public bool Actualizar { get; set; }
    }
}
