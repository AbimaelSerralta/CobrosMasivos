using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PeriodicidadesServices
    {
        private PeriodicidadesRepository periodicidadesRepository = new PeriodicidadesRepository();

        public List<Periodicidades> lsPeriodicidades = new List<Periodicidades>();

        public void CargarPeriodicidades()
        {
            lsPeriodicidades = new List<Periodicidades>();

            lsPeriodicidades = periodicidadesRepository.CargarPeriodicidades();
        }
    }
}
