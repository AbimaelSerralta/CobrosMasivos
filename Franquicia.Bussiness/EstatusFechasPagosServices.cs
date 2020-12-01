using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EstatusFechasPagosServices
    {
        private EstatusFechasPagosRepository estatusFechasPagosRepository = new EstatusFechasPagosRepository();

        public List<EstatusFechasPagos> lsEstatusFechasPagos = new List<EstatusFechasPagos>();

        public void CargarEstatusFechasPagos()
        {
            lsEstatusFechasPagos = new List<EstatusFechasPagos>();

            lsEstatusFechasPagos = estatusFechasPagosRepository.CargarEstatusFechasPagos();
        }
        public void CargarEstatusFechasPagosApRe()
        {
            lsEstatusFechasPagos = new List<EstatusFechasPagos>();

            lsEstatusFechasPagos = estatusFechasPagosRepository.CargarEstatusFechasPagosApRe();
        }

        #region PanelEscuela
        #region ReporteLigasEscuela
        public void CargarEstatusFechasPagosBusquedaRLE()
        {
            lsEstatusFechasPagos = new List<EstatusFechasPagos>();
            lsEstatusFechasPagos = estatusFechasPagosRepository.CargarEstatusFechasPagosBusquedaRLE();
        }
        #endregion
        #endregion
    }
}
