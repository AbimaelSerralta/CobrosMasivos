﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class AlumnosUsuariosExcelViewModel
    {
        public string VchCorreo { get; set; }
        public Guid UidAlumno { get; set; }
    }
}
