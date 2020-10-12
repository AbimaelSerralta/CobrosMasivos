using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposEventosServices
    {
        private TiposEventosRepository tiposEventosRepository = new TiposEventosRepository();

        public List<TiposEventos> lsTiposEventos = new List<TiposEventos>();

        public void CargarTiposEventos()
        {
            lsTiposEventos = new List<TiposEventos>();

            lsTiposEventos = tiposEventosRepository.CargarTiposEventos();
        }
    }
}
