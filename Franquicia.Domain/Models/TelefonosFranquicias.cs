﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class TelefonosFranquicias
    {
        public Guid UidTelefonoFranquicia { get; set; }
        public string VchTelefono { get; set; }
        public Guid UidFranquicia { get; set; }
        public Guid UidTipoTelefono { get; set; }
    }
}
