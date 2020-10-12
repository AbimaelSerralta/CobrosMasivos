using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class ProcesosEnsenianzasServices
    {
        private ProcesosEnsenianzasRepository procesosEnsenianzasRepository = new ProcesosEnsenianzasRepository();

        public List<ProcesosEnsenianzas> lsProcesosEnsenianzas = new List<ProcesosEnsenianzas>();

        public void CargarProcesosEnsenianzas()
        {
            lsProcesosEnsenianzas = new List<ProcesosEnsenianzas>();

            lsProcesosEnsenianzas = procesosEnsenianzasRepository.CargarProcesosEnsenianzas();
        }
    }
}
