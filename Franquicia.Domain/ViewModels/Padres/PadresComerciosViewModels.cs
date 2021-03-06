﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PadresComerciosViewModels: PagosPadres
    {
        public Guid UidCliente { get; set; }
        public string VchNombreComercial { get; set; }
        public byte[] Imagen { get; set; }
        
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Ciudad { get; set; }
        public string Calle { get; set; }
        public string EntreCalle { get; set; }
        public string YCalle { get; set; }
        public string VchColonia { get; set; }
        public string CodigoPostal { get; set; }
        //public string Direccion { get { return Calle + " " + EntreCalle + " " + YCalle + " " + VchColonia; } }
        public string Direccion { get { return Ciudad + " " + Municipio + " " + Estado + ", " + Pais; } }


        public string VchTelefono { get; set; }


    }
}
