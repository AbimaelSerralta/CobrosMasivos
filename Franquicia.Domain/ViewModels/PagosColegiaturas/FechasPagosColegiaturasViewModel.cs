using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class FechasPagosColegiaturasViewModel : PagosColegiaturas
    {
        public Guid UidEstatusFechaPago { get; set; }        
        public string VchEstatus { get; set; }        
        public decimal DcmImportePagado { get; set; }        
        
        public string Comentario { get; set; }        
    }
}
