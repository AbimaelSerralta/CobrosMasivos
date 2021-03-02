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
    public class PagosTarjetaPragaServices
    {
        private PagosTarjetaPragaRepository _pagosTarjetaPragaRepository = new PagosTarjetaPragaRepository();
        public PagosTarjetaPragaRepository pagosTarjetaPragaRepository
        {
            get { return _pagosTarjetaPragaRepository; }
            set { _pagosTarjetaPragaRepository = value; }
        }

        public List<LigasUrlsPayCardModel> lsLigasUrlsPayCardModel = new List<LigasUrlsPayCardModel>();
        public List<LigasEventoPayCardModel> lsLigasEventoPayCardModel = new List<LigasEventoPayCardModel>();
        
        public List<PagosTarjetaColeDetalleGridViewModel> lsPagosTarjetaColeDetalleGridViewModel = new List<PagosTarjetaColeDetalleGridViewModel>();
        
        
        public List<PagosColegiaturas> lsPagosColegiaturas = new List<PagosColegiaturas>();
        
        public bool AgregarInformacionTarjeta(PagosTarjetaPraga pagosTarjetaPraga)
        {
            return pagosTarjetaPragaRepository.AgregarInformacionTarjeta(pagosTarjetaPraga);
        }

        //public List<LigasUrlsPayCardModel> ConsultarPromocionLiga(string IdReferencia)
        //{
        //    lsLigasUrlsPayCardModel = new List<LigasUrlsPayCardModel>();
        //    return lsLigasUrlsPayCardModel = pagosRepository.ConsultarPromocionLiga(IdReferencia);
        //}

        //public List<LigasEventoPayCardModel> ConsultarPagoEventoLiga(string IdReferencia)
        //{
        //    lsLigasEventoPayCardModel = new List<LigasEventoPayCardModel>();
        //    return lsLigasEventoPayCardModel = pagosRepository.ConsultarPagoEventoLiga(IdReferencia);
        //}

        #region Metodos Escuela

        #region Pagos
        public Tuple<string, string, string> ConsultarPagoColegiatura(string IdReferencia)
        {
            return pagosTarjetaPragaRepository.ConsultarPagoColegiatura(IdReferencia);
        }
        public Tuple<List<PagosColegiaturasViewModels>, List<DetallesPagosColegiaturas>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosTarjetaPragaRepository.ObtenerPagoColegiatura(UidPagoColegiatura);
        }
        public bool ActualizarPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosTarjetaPragaRepository.ActualizarPagoColegiatura(UidPagoColegiatura);
        }

        public Tuple<string, string, string, string> ConsultarDatosValidarPago(Guid UidPagoColegiatura)
        {
            return pagosTarjetaPragaRepository.ConsultarDatosValidarPago(UidPagoColegiatura);
        }

        public bool ValidarPagoPadre(string IdReferencia)
        {
            return pagosTarjetaPragaRepository.ValidarPagoPadre(IdReferencia);
        }
        #endregion

        #region ReporteLigasPadres
        //public List<PagosTarjetaColeDetalleGridViewModel> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        //{
        //    lsPagosTarjetaColeDetalleGridViewModel = new List<PagosTarjetaColeDetalleGridViewModel>();

        //    return lsPagosTarjetaColeDetalleGridViewModel = pagosRepository.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
        //}
        #endregion

        #endregion
    }
}
