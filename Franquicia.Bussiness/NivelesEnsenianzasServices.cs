using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class NivelesEnsenianzasServices
    {
        private NivelesEnsenianzasRepository nivelesEnsenianzasRepository = new NivelesEnsenianzasRepository();

        public List<NivelesEnsenianzas> lsNivelesEnsenianzas = new List<NivelesEnsenianzas>();

        public void CargarNivelesEnsenianzas(Guid UidTipoEnsenianza)
        {
            lsNivelesEnsenianzas = new List<NivelesEnsenianzas>();

            lsNivelesEnsenianzas = nivelesEnsenianzasRepository.CargarNivelesEnsenianzas(UidTipoEnsenianza);
        }
    }
}
