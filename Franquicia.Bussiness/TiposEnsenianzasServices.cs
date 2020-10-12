using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposEnsenianzasServices
    {
        private TiposEnsenianzasRepository tiposEnsenianzasRepository = new TiposEnsenianzasRepository();

        public List<TiposEnsenianzas> lsTiposEnsenianzas = new List<TiposEnsenianzas>();

        public void CargarTiposEnsenianzas(Guid UidProcesoEnsenianza)
        {
            lsTiposEnsenianzas = new List<TiposEnsenianzas>();

            lsTiposEnsenianzas = tiposEnsenianzasRepository.CargarTiposEnsenianzas(UidProcesoEnsenianza);
        }
    }
}
