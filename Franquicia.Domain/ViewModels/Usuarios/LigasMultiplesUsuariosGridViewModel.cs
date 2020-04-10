﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class LigasMultiplesUsuariosGridViewModel : Usuarios
    {
        public string NombreCompleto { get { return StrNombre + " " + StrApePaterno + " " + StrApeMaterno; } }
        public string StrTelefono { get; set; }
        public bool blSeleccionado { get; set; }
        public int IdCliente { get; set; }
        public string StrAsunto { get; set; }
        public string StrConcepto { get; set; }
        public decimal DcmImporte { get; set; }
        public DateTime DtVencimiento { get; set; }
        public string StrPromociones { get; set; }
        public int IdFranquicia { get; set; }
        public string VchNombreComercial { get; set; }
    }
}
