using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PagosClubPagoServices
    {
        private PagosClubPagoRepository _pagosClubPagoRepository = new PagosClubPagoRepository();
        public PagosClubPagoRepository pagosClubPagoRepository
        {
            get { return _pagosClubPagoRepository; }
            set { _pagosClubPagoRepository = value; }
        }
        
        public List<PagosClubPago> lsPagosClubPago = new List<PagosClubPago>();
       
        #region Metodos Escuela

        #region Pagos
        
        #endregion

        #region ReporteLigasPadres
        public List<PagosClubPago> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            lsPagosClubPago = new List<PagosClubPago>();

            return lsPagosClubPago = pagosClubPagoRepository.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
        }
        #endregion

        #endregion
    }
}
