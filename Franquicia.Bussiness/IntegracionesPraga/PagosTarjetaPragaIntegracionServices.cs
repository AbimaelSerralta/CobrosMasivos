using Franquicia.DataAccess.Repository.IntegracionesPraga;
using Franquicia.Domain.Models;
using Franquicia.Domain.Models.IntegracionesPraga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesPraga
{
    public class PagosTarjetaPragaIntegracionServices
    {
        private PagosTarjetaPragaIntegracionRepository _pagosTarjetaPragaIntegracionRepository = new PagosTarjetaPragaIntegracionRepository();
        public PagosTarjetaPragaIntegracionRepository pagosTarjetaPragaIntegracionRepository
        {
            get { return _pagosTarjetaPragaIntegracionRepository; }
            set { _pagosTarjetaPragaIntegracionRepository = value; }
        }

        public bool AgregarInformacionTarjeta(PagosTarjetaPraga pagosTarjetaPraga)
        {
            return pagosTarjetaPragaIntegracionRepository.AgregarInformacionTarjeta(pagosTarjetaPraga);
        }

    }
}
