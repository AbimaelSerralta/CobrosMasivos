using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposTarjetasServices
    {
        private TiposTarjetasRepository tiposTarjetasRepository = new TiposTarjetasRepository();

        public List<TiposTarjetas> lsTiposTarjetas = new List<TiposTarjetas>();

        public void CargarTiposTarjetas()
        {
            lsTiposTarjetas = new List<TiposTarjetas>();

            lsTiposTarjetas = tiposTarjetasRepository.CargarTiposTarjetas();
        }

        public void CargarTiposTarjetasCliente(Guid UidCliente)
        {
            lsTiposTarjetas = new List<TiposTarjetas>();

            lsTiposTarjetas = tiposTarjetasRepository.CargarTiposTarjetasCliente(UidCliente);
        }
    }
}
