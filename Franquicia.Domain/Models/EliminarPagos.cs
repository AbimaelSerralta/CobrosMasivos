﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.Models
{
    public class EliminarPagos
    {
        public string IdReferencia { get; set; }
        public Guid UidPagoColegiatura { get; set; }
    }
}
