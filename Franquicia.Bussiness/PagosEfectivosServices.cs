using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PagosEfectivosServices
    {
        private PagosEfectivosRepository _pagosEfectivosRepository = new PagosEfectivosRepository();
        public PagosEfectivosRepository pagosEfectivosRepository
        {
            get { return _pagosEfectivosRepository; }
            set { _pagosEfectivosRepository = value; }
        }

        public List<PagosEfectivos> lsPagosEfectivos = new List<PagosEfectivos>();
        
        public List<rdlcPagosEfectivosViewModels> lsrdlcPagosEfectivosViewModels = new List<rdlcPagosEfectivosViewModels>();

        #region PanelEscuela

        #region Metodos ReporteLigasEscuela
        public bool RegistrarPagoEfectivo(Guid UidPagoColegiatura, DateTime DtFHPago, decimal DcmImporte, bool BitTipoTarjeta, Guid UidTipoTarjeta, bool BitPromocionTT, Guid UidPromocionTerminal)
        {
            bool result = false;
            if (pagosEfectivosRepository.RegistrarPagoEfectivo(
                new PagosEfectivos()
                {
                    UidPagoColegiatura = UidPagoColegiatura,
                    DtFHPago = DtFHPago,
                    DcmImporte = DcmImporte,
                    BitTipoTarjeta = BitTipoTarjeta,
                    UidTipoTarjeta = UidTipoTarjeta,
                    BitPromocionTT = BitPromocionTT,
                    UidPromocionTerminal = UidPromocionTerminal
                }
                ))
            {
                result = true;
            }
            return result;
        }

        public List<rdlcPagosEfectivosViewModels> ObtenerPagoEfectivoRLE(Guid UidPagoColegiatura)
        {
            lsrdlcPagosEfectivosViewModels = new List<rdlcPagosEfectivosViewModels>();
            return lsrdlcPagosEfectivosViewModels = pagosEfectivosRepository.ObtenerPagoEfectivoRLE(UidPagoColegiatura);
        }
        #endregion
        #endregion
    }
}
