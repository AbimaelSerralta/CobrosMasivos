using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposTarjetasClubPagoServices
    {
        private TiposTarjetasClubPagoRepository tiposTarjetasClubPagoRepository = new TiposTarjetasClubPagoRepository();

        public List<TiposTarjetasClubPago> lsTiposTarjetasClubPago = new List<TiposTarjetasClubPago>();

        public void CargarTiposTarjetas()
        {
            lsTiposTarjetasClubPago = new List<TiposTarjetasClubPago>();

            lsTiposTarjetasClubPago = tiposTarjetasClubPagoRepository.CargarTiposTarjetas();
        }

        public void CargarTiposTarjetasCliente(Guid UidCliente)
        {
            lsTiposTarjetasClubPago = new List<TiposTarjetasClubPago>();

            lsTiposTarjetasClubPago = tiposTarjetasClubPagoRepository.CargarTiposTarjetasCliente(UidCliente);
        }
    }
}
