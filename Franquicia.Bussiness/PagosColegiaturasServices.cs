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
    public class PagosColegiaturasServices
    {
        private PagosColegiaturasRepository _pagosColegiaturasRepository = new PagosColegiaturasRepository();
        public PagosColegiaturasRepository pagosColegiaturasRepository
        {
            get { return _pagosColegiaturasRepository; }
            set { _pagosColegiaturasRepository = value; }
        }

        private ValidacionesRepository _validacionesRepository = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesRepository; }
            set { _validacionesRepository = value; }
        }

        public List<PagosColegiaturas> lsPagosColegiaturas = new List<PagosColegiaturas>();
        
        public List<FechasPagosColegiaturasViewModel> lsFechasPagosColegiaturasViewModel = new List<FechasPagosColegiaturasViewModel>();
        public List<FechasPagosColegiaturasViewModel> lsPagosPendientes = new List<FechasPagosColegiaturasViewModel>();
        
        public List<ReportePadresFechasPagosColeViewModel> lsReportePadresFechasPagosColeViewModel = new List<ReportePadresFechasPagosColeViewModel>();

        public List<DetallePagosColeGridViewModel> lsDetallePagosColeGridViewModel = new List<DetallePagosColeGridViewModel>();

        #region Metodos PagosColegiatura
        //public void CargarAlumnos(Guid UidCliente)
        //{
        //    lsAlumnosGridViewModel = alumnosRepository.CargarAlumnos(UidCliente);
        //}
        //public void ObtenerAlumno(Guid UidAlumno)
        //{
        //    alumnosRepository.alumnosGridViewModel = new AlumnosGridViewModel();
        //    alumnosRepository.alumnosGridViewModel = lsAlumnosGridViewModel.Find(x => x.UidAlumno == UidAlumno);
        //}
        public int ObtenerUltimoFolio(Guid UidCliente)
        {
            return pagosColegiaturasRepository.ObtenerUltimoFolio(UidCliente);
        }
        public bool RegistrarPagoColegiatura(Guid UidPagoColegiatura, int UltimoFolio, DateTime DtFHPago, string VchPromocionDePago, string VchComisionBancaria, bool BitSubtotal, decimal DcmSubtotal, bool BitComisionBancaria, decimal DcmComisionBancaria, bool BitPromocionDePago, decimal DcmPromocionDePago, bool BitValidarImporte, decimal DcmValidarImporte, decimal DcmTotal, Guid UidUsuario, Guid UidEstatusPagoColegiatura,
                                             Guid UidFechaColegiatura, Guid UidAlumno, Guid UidFormaPago, decimal DcmImporteCole, decimal DcmImportePagado, decimal DcmImporteNuevo, Guid EstatusFechaPago)
        {
            bool result = false;
            if (pagosColegiaturasRepository.RegistrarPagoColegiatura(
                new PagosColegiaturas
                {
                    UidPagoColegiatura = UidPagoColegiatura,
                    IntFolio = UltimoFolio,
                    DtFHPago = DtFHPago,
                    VchPromocionDePago = VchPromocionDePago,
                    VchComisionBancaria = VchComisionBancaria,
                    BitSubtotal = BitSubtotal,
                    DcmSubtotal = DcmSubtotal,
                    BitComisionBancaria = BitComisionBancaria,
                    DcmComisionBancaria = DcmComisionBancaria,
                    BitPromocionDePago = BitPromocionDePago,
                    DcmPromocionDePago = DcmPromocionDePago,
                    BitValidarImporte = BitValidarImporte,
                    DcmValidarImporte = DcmValidarImporte,
                    DcmTotal = DcmTotal,
                    UidUsuario = UidUsuario,
                    UidEstatusPagoColegiatura = UidEstatusPagoColegiatura
                },
                UidFechaColegiatura,
                UidAlumno,
                UidFormaPago,
                DcmImporteCole,
                DcmImportePagado,
                DcmImporteNuevo,
                EstatusFechaPago
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarPagoColegiatura(Guid UidAlumno, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula, string Correo, bool BitBeca, string TipoBeca, decimal Beca, Guid UidEstatus, string Telefono, Guid UidPrefijo)
        {
            bool result = false;
            if (pagosColegiaturasRepository.ActualizarPagoColegiatura(
                new Alumnos
                {
                    UidAlumno = UidAlumno,
                    VchIdentificador = Identificador,
                    VchNombres = Nombre,
                    VchApePaterno = ApePaterno,
                    VchApeMaterno = ApeMaterno,
                    VchMatricula = Matricula,
                    VchCorreo = Correo,
                    BitBeca = BitBeca,
                    VchTipoBeca = TipoBeca,
                    DcmBeca = Beca,
                    UidEstatus = UidEstatus
                },
                new TelefonosAlumnos
                {
                    VchTelefono = Telefono,
                    UidPrefijo = UidPrefijo
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarPagoColegiatura(Guid UidPagoColegiatura)
        {
            bool result = false;
            if (pagosColegiaturasRepository.EliminarPagoColegiatura(UidPagoColegiatura))
            {
                result = true;
            }
            return result;
        }

        //public void BuscarAlumnos(string Identificador, string Matricula, string Correo, string Nombre, string ApePaterno, string ApeMaterno, string Celular, string Asociado, string Beca, Guid UidEstatus, string Colegiatura, Guid UidCliente)
        //{
        //    lsAlumnosGridViewModel = alumnosRepository.BuscarAlumnos(Identificador, Matricula, Correo, Nombre, ApePaterno, ApeMaterno, Celular, Asociado, Beca, UidEstatus, Colegiatura, UidCliente);
        //}
        #endregion

        #region Metodos Panel Tutor
        #region Pagos
        public List<FechasPagosColegiaturasViewModel> ObtenerPagosPadres(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            lsFechasPagosColegiaturasViewModel = new List<FechasPagosColegiaturasViewModel>();
            return lsFechasPagosColegiaturasViewModel = pagosColegiaturasRepository.ObtenerPagosPadres(UidFechaColegiatura, UidAlumno);
        }
        public List<FechasPagosColegiaturasViewModel> ObtenerPagosPendientesPadres(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            lsPagosPendientes = new List<FechasPagosColegiaturasViewModel>();
            return lsPagosPendientes = pagosColegiaturasRepository.ObtenerPagosPendientesPadres(UidFechaColegiatura, UidAlumno);
        }
        public List<ReportePadresFechasPagosColeViewModel> ObtenerPagosReportePadres(Guid UidFechaColegiatura, string VchMatricula)
        {
            lsReportePadresFechasPagosColeViewModel = new List<ReportePadresFechasPagosColeViewModel>();
            return lsReportePadresFechasPagosColeViewModel = pagosColegiaturasRepository.ObtenerPagosPadresReporte(UidFechaColegiatura, VchMatricula);
        }
        
        public decimal ObtenerImporteResta(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            return pagosColegiaturasRepository.ObtenerImporteResta(UidFechaColegiatura, UidAlumno);
        }
        public bool ActualizarImporteResta(Guid UidFechaColegiatura, Guid UidAlumno, decimal DcmImporteResta)
        {
            bool result = false;
            if (pagosColegiaturasRepository.ActualizarImporteResta(UidFechaColegiatura, UidAlumno, DcmImporteResta))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Metodos ReporteLigasPadres
        public List<ReportePadresFechasPagosColeViewModel> ObtenerPagosPadresReporte(Guid UidFechaColegiatura, string VchMatricula)
        {
            lsReportePadresFechasPagosColeViewModel = new List<ReportePadresFechasPagosColeViewModel>();
            return lsReportePadresFechasPagosColeViewModel = pagosColegiaturasRepository.ObtenerPagosPadresReporte(UidFechaColegiatura, VchMatricula);
        }
        public Tuple<List<PagosColegiaturasViewModels>, List<DetallePagosColeGridViewModel>> ObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosColegiaturasRepository.ObtenerPagoColegiatura(UidPagoColegiatura);
        }

        #region ReportViewer
        public List<PagosColegiaturasViewModels> rdlcObtenerPagoColegiatura(Guid UidPagoColegiatura)
        {
            return pagosColegiaturasRepository.rdlcObtenerPagoColegiatura(UidPagoColegiatura);
        }

        #endregion
        #endregion
        #endregion

        #region Metodos PanelEscuela
        #region ReporteLigasEscuelas
        public bool ActualizarEstatusFechaPago(Guid UidPagoColegiatura, Guid UidEstatusFechaPago)
        {
            bool result = false;
            if (pagosColegiaturasRepository.ActualizarEstatusFechaPago(UidPagoColegiatura, UidEstatusFechaPago))
            {
                result = true;
            }
            return result;
        }
        public decimal ObtenerPagosPadresRLE(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            return pagosColegiaturasRepository.ObtenerPagosPadresRLE(UidFechaColegiatura, UidAlumno);
        }
        public decimal ObtenerPendientesPadresRLE(Guid UidFechaColegiatura, Guid UidAlumno)
        {
            return pagosColegiaturasRepository.ObtenerPendientesPadresRLE(UidFechaColegiatura, UidAlumno);
        }
        #endregion
        #endregion
    }
}
