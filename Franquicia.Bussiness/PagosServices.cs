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
    public class PagosServices
    {
        private PagosRepository _pagosRepository = new PagosRepository();
        public PagosRepository pagosRepository
        {
            get { return _pagosRepository; }
            set { _pagosRepository = value; }
        }

        public List<LigasUrlsPayCardModel> lsLigasUrlsPayCardModel = new List<LigasUrlsPayCardModel>();
        public List<LigasEventoPayCardModel> lsLigasEventoPayCardModel = new List<LigasEventoPayCardModel>();
        
        public List<PagosTarjetaColeDetalleGridViewModel> lsPagosTarjetaColeDetalleGridViewModel = new List<PagosTarjetaColeDetalleGridViewModel>();
        
        public List<PagosTarjetaPragaColeDetalleGridViewModel> lsPagosTarjetaPragaColeDetalleGridViewModel = new List<PagosTarjetaPragaColeDetalleGridViewModel>();
        
        
        public List<PagosColegiaturas> lsPagosColegiaturas = new List<PagosColegiaturas>();
        
        public bool AgregarInformacionTarjeta(string Autorizacion, string reference, DateTime HoraTransaccion, string response, string cc_type, string tp_operation, string nb_company, string nb_merchant, string id_url, string cd_error, string nb_error, string cc_number, string cc_mask, string FolioPago, decimal Monto, DateTime DtFechaOperacion)
        {
            return pagosRepository.AgregarInformacionTarjeta(Autorizacion, reference, HoraTransaccion, response, cc_type, tp_operation, nb_company, nb_merchant, id_url, cd_error, nb_error, cc_number, cc_mask, FolioPago, Monto, DtFechaOperacion);
        }

        public List<LigasUrlsPayCardModel> ConsultarPromocionLiga(string IdReferencia)
        {
            lsLigasUrlsPayCardModel = new List<LigasUrlsPayCardModel>();
            return lsLigasUrlsPayCardModel = pagosRepository.ConsultarPromocionLiga(IdReferencia);
        }

        public List<LigasEventoPayCardModel> ConsultarPagoEventoLiga(string IdReferencia)
        {
            lsLigasEventoPayCardModel = new List<LigasEventoPayCardModel>();
            return lsLigasEventoPayCardModel = pagosRepository.ConsultarPagoEventoLiga(IdReferencia);
        }

        public string ObtenerCorreoAuxiliar(string IdReferencia)
        {
            return pagosRepository.ObtenerCorreoAuxiliar(IdReferencia);
        }

        //public bool ActualizarEstatusLigas(string Estatus)
        //{
        //    //return pagosRepository.ActualizarEstatusLigas(Estatus);
        //}

        #region Metodos Escuela

        #region Pagos
        public Tuple<string, string, string> ConsultarPagoColegiatura(string IdReferencia)
        {
            return pagosRepository.ConsultarPagoColegiatura(IdReferencia);
        }
        public Tuple<List<PagosColegiaturasViewModels>, List<DetallesPagosColegiaturas>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosRepository.ObtenerPagoColegiatura(UidPagoColegiatura);
        }
        public bool ActualizarPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosRepository.ActualizarPagoColegiatura(UidPagoColegiatura);
        }
        public bool ActualizarPagoColegiaturaPLE(Guid UidPagoColegiatura, Guid UidEstatusFechaPago)
        {
            return pagosRepository.ActualizarPagoColegiaturaPLE(UidPagoColegiatura, UidEstatusFechaPago);
        }

        public Tuple<string, string, string, string> ConsultarDatosValidarPago(Guid UidPagoColegiatura)
        {
            return pagosRepository.ConsultarDatosValidarPago(UidPagoColegiatura);
        }
        #endregion

        #region ReporteLigasPadres
        public List<PagosTarjetaColeDetalleGridViewModel> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            lsPagosTarjetaColeDetalleGridViewModel = new List<PagosTarjetaColeDetalleGridViewModel>();

            return lsPagosTarjetaColeDetalleGridViewModel = pagosRepository.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
        }
        public List<PagosTarjetaPragaColeDetalleGridViewModel> ConsultarDetallePagoColegiaturaPraga(Guid UidPagoColegiatura)
        {
            lsPagosTarjetaPragaColeDetalleGridViewModel = new List<PagosTarjetaPragaColeDetalleGridViewModel>();

            return lsPagosTarjetaPragaColeDetalleGridViewModel = pagosRepository.ConsultarDetallePagoColegiaturaPraga(UidPagoColegiatura);
        }
        #endregion

        #endregion
    }
}
