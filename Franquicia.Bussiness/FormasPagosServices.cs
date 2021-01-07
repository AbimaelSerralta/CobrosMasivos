using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class FormasPagosServices
    {
        private FormasPagosRepository formasPagosRepository = new FormasPagosRepository();

        public List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

        public void CargarFormasPagos()
        {
            lsFormasPagos = new List<FormasPagos>();

            lsFormasPagos = formasPagosRepository.CargarFormasPagos();
        }

        #region Metodos PanelTutor
        #region Pagos
        public void CargarFormasPagosPadres()
        {
            lsFormasPagos = new List<FormasPagos>();

            lsFormasPagos = formasPagosRepository.CargarFormasPagosPadres();
        }
        #endregion

        #region ReportePagosPadres
        public void CargarFormasPagosReporteLigasPadres()
        {
            lsFormasPagos = new List<FormasPagos>();

            lsFormasPagos = formasPagosRepository.CargarFormasPagosReporteLigasPadres();
        }
        #endregion
        
        #region ReporteLigasEscuelas
        public void CargarFormasPagosReporteLigasEscuelas(Guid UidCliente)
        {
            lsFormasPagos = new List<FormasPagos>();

            lsFormasPagos = formasPagosRepository.CargarFormasPagosReporteLigasEscuelas(UidCliente);
        }
        #endregion
        #endregion
    }
}
