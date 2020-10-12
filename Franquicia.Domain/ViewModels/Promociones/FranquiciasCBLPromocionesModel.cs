using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class FranquiciasCBLPromocionesModel : Promociones
    {
        public Guid UidFranquiciaPromocion { get; set; }
        public Guid UidFranquicia { get; set; }
        public Decimal DcmComicion { get; set; }
    }
}
