using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposTelefonosServices
    {
        TiposTelefonosRepository tiposTelefonosRepository = new TiposTelefonosRepository();

        public List<TiposTelefonos> lsTiposTelefonos = new List<TiposTelefonos>();

        public List<TiposTelefonos> CargarTiposTelefonos()
        {
            lsTiposTelefonos = new List<TiposTelefonos>();

            return lsTiposTelefonos = tiposTelefonosRepository.CargarTiposTelefonos();
        }
    }
}
