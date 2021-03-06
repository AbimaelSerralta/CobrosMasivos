﻿using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class CBLPromocionesTerminalViewModel : PromocionesTerminal
    {
        public Guid UidClientePromocionTerminal { get; set; }
        public Guid UidCliente { get; set; }
        public Decimal DcmComicion { get; set; }
        public Decimal DcmApartirDe { get; set; }
        public Guid UidTipoTarjeta { get; set; }
        
        public bool blChecked { get; set; }
    }
}
