using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels.ClubPago
{
    public class GenerarRefereciaPagoIntegraciones: GenerarRefereciaPago
    {
        public string IntegrationID { get; set; }
        public string SchoolID { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string UidTipoPagoIntegracion { get; set; }
    }
}
