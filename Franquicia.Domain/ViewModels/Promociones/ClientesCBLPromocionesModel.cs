﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class CBLPromocionesModel : Promociones
    {
        public Guid UidClientePromocion { get; set; }
        public Guid UidCliente { get; set; }
        public Decimal DcmComicion { get; set; }
    }
}
