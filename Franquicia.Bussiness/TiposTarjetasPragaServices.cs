using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposTarjetasPragaServices
    {
        private TiposTarjetasPragaRepository tiposTarjetasPragaRepository = new TiposTarjetasPragaRepository();

        public List<TiposTarjetasPraga> lsTiposTarjetasPraga = new List<TiposTarjetasPraga>();

        public void CargarTiposTarjetas()
        {
            lsTiposTarjetasPraga = new List<TiposTarjetasPraga>();

            lsTiposTarjetasPraga = tiposTarjetasPragaRepository.CargarTiposTarjetas();
        }

        public void CargarTiposTarjetasCliente(Guid UidCliente)
        {
            lsTiposTarjetasPraga = new List<TiposTarjetasPraga>();

            lsTiposTarjetasPraga = tiposTarjetasPragaRepository.CargarTiposTarjetasCliente(UidCliente);
        }
    }
}
