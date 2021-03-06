﻿using Franquicia.DataAccess.Repository.IntegracionesPraga;
using Franquicia.Domain.Models.IntegracionesPraga;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.IntegracionesPraga
{
    public class PagosIntegracionServices
    {
        private PagosIntegracionRepository _pagosIntegracion = new PagosIntegracionRepository();
        public PagosIntegracionRepository pagosIntegracion
        {
            get { return _pagosIntegracion; }
            set { _pagosIntegracion = value; }
        }

        public List<ReporteGeneradosGridViewModel> lsReporteGeneradosGridViewModel = new List<ReporteGeneradosGridViewModel>();

        public bool RegistrarPagosIntegracion(Guid UidPagoIntegracion, int IdEscuela, decimal DcmImporte, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidFormaPago, Guid UidEstatusFechaPago, Guid UidTipoPagoIntegracion)
        {
            bool result = false;
            if (pagosIntegracion.RegistrarPagosIntegracion(UidPagoIntegracion, IdEscuela, DcmImporte, DcmImportePagado, DcmImporteNuevo, UidFormaPago, UidEstatusFechaPago, UidTipoPagoIntegracion))
            {
                result = true;
            }
            return result;
        }

        #region Metodos Integraciones
        #region Praga 
        public List<PagosIntegracion> ObtenerPagoIntegracion(string IdReferencia)
        {
            return pagosIntegracion.ObtenerPagoIntegracion(IdReferencia);
        }
        public bool ActualizarPagoIntegracion(Guid UidPagoIntegracion, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidEstatusFechaPago)
        {
            bool result = false;
            if (pagosIntegracion.ActualizarPagoIntegracion(UidPagoIntegracion, DcmImportePagado, DcmImporteNuevo, UidEstatusFechaPago))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region ClubPago
        public List<PagosIntegracion> ObtenerPagoClubPagoIntegracion(string IdReferencia)
        {
            return pagosIntegracion.ObtenerPagoClubPagoIntegracion(IdReferencia);
        }
        public bool ActualizarPagoClubPagoIntegracion(Guid UidPagoIntegracion, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid UidEstatusFechaPago)
        {
            bool result = false;
            if (pagosIntegracion.ActualizarPagoClubPagoIntegracion(UidPagoIntegracion, DcmImportePagado, DcmImporteNuevo, UidEstatusFechaPago))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Metodos web
        #region ReportGenerated
        public List<ReporteGeneradosGridViewModel> CargarReporteGenerado(Guid UidCredencial)
        {
            lsReporteGeneradosGridViewModel = new List<ReporteGeneradosGridViewModel>();
            return lsReporteGeneradosGridViewModel = pagosIntegracion.CargarReporteGenerado(UidCredencial);
        }
        public List<ReporteGeneradosGridViewModel> BuscarReporteGenerado(string FHDesde, string FHHasta, string IdReferencia, Guid UidCredencial)
        {
            lsReporteGeneradosGridViewModel = new List<ReporteGeneradosGridViewModel>();
            return lsReporteGeneradosGridViewModel = pagosIntegracion.BuscarReporteGenerado(FHDesde, FHHasta, IdReferencia, UidCredencial);
        }
        #endregion
        #endregion

        #endregion
    }
}
