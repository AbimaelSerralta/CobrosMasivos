﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasUsuariosGridViewModel : Usuarios
    {
        public string NombreCompleto { get { return StrNombre + " " + StrApePaterno + " " + StrApeMaterno; } }
        public Guid UidPrefijo { get; set; }
        public string StrTelefono { get; set; }
        public bool blSeleccionado { get; set; }
        public int IdCliente { get; set; }
        public int IdFranquicia { get; set; }
        public string VchNombreComercial { get; set; }
    }
}
