using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Domain.ViewModels
{
    public class PagTarjDetalUsFinalGridViewModel : PagosTarjeta
    {
        public int IntNum { get; set; }
        public string VchColor { get; set; }
    }
}
