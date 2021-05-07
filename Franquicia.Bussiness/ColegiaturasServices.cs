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
    public class ColegiaturasServices
    {
        private ColegiaturasRepository _colegiaturasRepository = new ColegiaturasRepository();
        public ColegiaturasRepository colegiaturasRepository
        {
            get { return _colegiaturasRepository; }
            set { _colegiaturasRepository = value; }
        }

        public List<ColegiaturasGridViewModel> lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();

        public List<PagosColegiaturasViewModel> lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();

        public List<DesglosePagosGridViewModel> lsDesglosePagosGridViewModel = new List<DesglosePagosGridViewModel>();

        public List<PagosReporteLigaEscuelaViewModels> lsPagosReporteLigaEscuelaViewModels = new List<PagosReporteLigaEscuelaViewModels>();

        public List<PagosReporteLigaPadreViewModels> lsPagosReporteLigaPadreViewModels = new List<PagosReporteLigaPadreViewModels>();

        public List<ColegiaturasFechasGridViewModel> lsFechasColegiaturas = new List<ColegiaturasFechasGridViewModel>();


        public List<FCAATMViewModel> lsFCAATMViewModel = new List<FCAATMViewModel>();

        #region Metodos Cliente
        public List<ColegiaturasGridViewModel> CargarColegiaturas(Guid UidCliente)
        {
            lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();
            return lsColegiaturasGridViewModel = colegiaturasRepository.CargarColegiaturas(UidCliente);
        }
        public void ObtenerColegiatura(Guid UidColegiatura)
        {
            colegiaturasRepository.colegiaturasGridViewModel = new ColegiaturasGridViewModel();
            colegiaturasRepository.colegiaturasGridViewModel = lsColegiaturasGridViewModel.Find(x => x.UidColegiatura == UidColegiatura);
        }
        //public void ObtenerEvento(Guid UidEvento)
        //{
        //    eventosRepository.eventosGridViewModel = new EventosGridViewModel();
        //    eventosRepository.eventosGridViewModel = lsEventosGridViewModel.Find(x => x.UidEvento == UidEvento);
        //}
        public bool RegistrarColegiatura(Guid UidColegiatura, string VchIdentificador, decimal DcmImporte, int IntCantPagos, Guid UidPeriodicidad, DateTime DtFHInicio, bool BitFHLimite, DateTime DtFHLimite, bool BitFHVencimiento, DateTime DtFHVencimiento, bool BitRecargo, string VchTipoRecargo, decimal DcmRecargo, bool BitRecargoPeriodo, string VchTipoRecargoPeriodo, decimal DcmRecargoPeriodo, Guid UidCliente)
        {
            bool result = false;
            if (colegiaturasRepository.RegistrarColegiatura(new ColegiaturasGridViewModel
            {
                UidColegiatura = UidColegiatura,
                VchIdentificador = VchIdentificador,
                DcmImporte = DcmImporte,
                IntCantPagos = IntCantPagos,
                UidPeriodicidad = UidPeriodicidad,
                DtFHInicio = DtFHInicio,
                BitFHLimite = BitFHLimite,
                DtFHLimite = DtFHLimite,
                BitFHVencimiento = BitFHVencimiento,
                DtFHVencimiento = DtFHVencimiento,
                BitRecargo = BitRecargo,
                VchTipoRecargo = VchTipoRecargo,
                DcmRecargo = DcmRecargo,
                BitRecargoPeriodo = BitRecargoPeriodo,
                VchTipoRecargoPeriodo = VchTipoRecargoPeriodo,
                DcmRecargoPeriodo = DcmRecargoPeriodo,
                UidCliente = UidCliente
            }))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarColegiatura(Guid UidColegiatura, string VchIdentificador, decimal DcmImporte, int IntCantPagos, Guid UidPeriodicidad, DateTime DtFHInicio, bool BitFHLimite, DateTime DtFHLimite, bool BitFHVencimiento, DateTime DtFHVencimiento, bool BitRecargo, string VchTipoRecargo, decimal DcmRecargo, bool BitRecargoPeriodo, string VchTipoRecargoPeriodo, decimal DcmRecargoPeriodo)
        {
            bool result = false;
            if (colegiaturasRepository.ActualizarColegiatura(new ColegiaturasGridViewModel
            {
                UidColegiatura = UidColegiatura,
                VchIdentificador = VchIdentificador,
                DcmImporte = DcmImporte,
                IntCantPagos = IntCantPagos,
                UidPeriodicidad = UidPeriodicidad,
                DtFHInicio = DtFHInicio,
                BitFHLimite = BitFHLimite,
                DtFHLimite = DtFHLimite,
                BitFHVencimiento = BitFHVencimiento,
                DtFHVencimiento = DtFHVencimiento,
                BitRecargo = BitRecargo,
                VchTipoRecargo = VchTipoRecargo,
                DcmRecargo = DcmRecargo,
                BitRecargoPeriodo = BitRecargoPeriodo,
                VchTipoRecargoPeriodo = VchTipoRecargoPeriodo,
                DcmRecargoPeriodo = DcmRecargoPeriodo
            }))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarEstatusColegiatura(Guid UidColegiatura, Guid UidEstatus)
        {
            return colegiaturasRepository.ActualizarEstatusColegiatura(UidColegiatura, UidEstatus);
        }
        public void BuscarColegiatura(string Identificador, decimal ImporteMayor, decimal ImporteMenor, string CantPagos, Guid UidPeriodicidad, string FHInicioDesde, string FHInicioHasta, string FechaLimite, string FechaVencimineto, string RecargoLimite, string RecargoPeriodo, Guid UidEstatus, Guid UidCliente)
        {
            lsColegiaturasGridViewModel = colegiaturasRepository.BuscarColegiatura(Identificador, ImporteMayor, ImporteMenor, CantPagos, UidPeriodicidad, FHInicioDesde, FHInicioHasta, FechaLimite, FechaVencimineto, RecargoLimite, RecargoPeriodo, UidEstatus, UidCliente);
        }

        public bool RegistrarColegiaturaFechas(Guid UidColegiatura, int IntNumero, DateTime DtFHInicio, DateTime DtFHLimite, DateTime DtFHVencimiento, DateTime DtFHFinPeriodo)
        {
            bool result = false;

            if (colegiaturasRepository.RegistrarColegiaturaFechas(UidColegiatura, IntNumero, DtFHInicio, DtFHLimite, DtFHVencimiento, DtFHFinPeriodo))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarColegiaturaFechas(Guid UidColegiatura)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarColegiaturaFechas(UidColegiatura))
            {
                result = true;
            }
            return result;
        }
        public List<ColegiaturasFechasGridViewModel> ObtenerFechaColegiatura(Guid UidColegiatura)
        {
            return colegiaturasRepository.ObtenerFechaColegiatura(UidColegiatura);
        }

        public List<ColegiaturasFechasGridViewModel> ObtenerFechasColegiaturasVicular(Guid UidColegiatura)
        {
            lsFechasColegiaturas = new List<ColegiaturasFechasGridViewModel>();
            return lsFechasColegiaturas = colegiaturasRepository.ObtenerFechasColegiaturasVicular(UidColegiatura);
        }
        public bool RegistrarFechasColegiaturasAlumnos(Guid UidFechaColegiatura, Guid UidAlumno, decimal DcmImporteResta, Guid UidEstatusFechaColegiatura)
        {
            bool result = false;

            if (colegiaturasRepository.RegistrarFechasColegiaturasAlumnos(UidFechaColegiatura, UidAlumno, DcmImporteResta, UidEstatusFechaColegiatura))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarFechasColegiaturasAlumnos(Guid UidFechaColegiatura)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarFechasColegiaturasAlumnos(UidFechaColegiatura))
            {
                result = true;
            }
            return result;
        }

        public bool RegistrarPromocionesColegiatura(Guid UidColegiatura, Guid UidPromociones)
        {
            bool result = false;
            if (colegiaturasRepository.RegistrarPromocionesColegiatura(UidColegiatura, UidPromociones))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarPromocionesColegiatura(Guid UidColegiatura)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarPromocionesColegiatura(UidColegiatura))
            {
                result = true;
            }
            return result;
        }

        //public string ObtenerUrlLiga(string IdReferencia)
        //{
        //    return eventosRepository.ObtenerUrlLiga(IdReferencia);
        //}
        //public string ObtenerUidAdminCliente(Guid UidCliente)
        //{
        //    return eventosRepository.ObtenerUidAdminCliente(UidCliente);
        //}

        //public bool ValidarPagoEvento(string IdReferencia)
        //{
        //    return eventosRepository.ValidarPagoEvento(IdReferencia);
        //}
        //public bool EliminarLigaEvento(string IdReferencia)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarLigaEvento(IdReferencia))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool InsertCorreoLigaEvento(string Correo, string IdReferencia)
        //{
        //    bool result = false;
        //    if (eventosRepository.InsertCorreoLigaEvento(Correo, IdReferencia))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        //public void BuscarEventos(Guid UidPropietario, string VchNombreEvento, string DtFHInicioDesde, string DtFHInicioHasta, string DtFHFinDesde, string DtFHFinHasta, decimal DcmImporteMayor, decimal DcmImporteMenor, Guid UidEstatus)
        //{
        //    lsEventosGridViewModel = eventosRepository.BuscarEventos(UidPropietario, VchNombreEvento, DtFHInicioDesde, DtFHInicioHasta, DtFHFinDesde, DtFHFinHasta, DcmImporteMayor, DcmImporteMenor, UidEstatus);
        //}

        //public bool RegistrarUsuariosEvento(Guid UidEvento, Guid UidUsuario)
        //{
        //    bool result = false;
        //    if (eventosRepository.RegistrarUsuariosEvento(UidEvento, UidUsuario))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool EliminarUsuariosEvento(Guid UidEvento)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarUsuariosEvento(UidEvento))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        #endregion

        #region Metodos Padres
        public List<PagosColegiaturasViewModel> CargarPagosColegiaturas(Guid UidCliente, Guid UidUsuario, DateTime FechaInicio)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.CargarPagosColegiaturas(UidCliente, UidUsuario, FechaInicio);
        }
        public void ObtenerPagosColegiaturas(Guid UidCliente, Guid UidUsuario, Guid UidFechaColegiatura, string VchMatricula)
        {
            colegiaturasRepository.ObtenerPagoColegiatura(UidCliente, UidUsuario, UidFechaColegiatura, VchMatricula);
        }
        //public void ObtenerPagosColegiaturas(Guid UidFechaColegiatura)
        //{
        //    colegiaturasRepository.pagosColegiaturasViewModel = new PagosColegiaturasViewModel();
        //    colegiaturasRepository.pagosColegiaturasViewModel = lsPagosColegiaturasViewModel.Find(x => x.UidFechaColegiatura == UidFechaColegiatura);
        //}
        public bool EliminarLigaColegiatura(string IdReferencia)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarLigaColegiatura(IdReferencia))
            {
                result = true;
            }
            return result;
        }

        public bool EliminarLigaPragaColegiatura(string IdReferencia)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarLigaPragaColegiatura(IdReferencia))
            {
                result = true;
            }
            return result;
        }

        public List<DesglosePagosGridViewModel> FormarDesgloseCole(int IntNum, string VchConcepto, decimal DcmImporte, string VchCoResta = "")
        {
            lsDesglosePagosGridViewModel.Add(new DesglosePagosGridViewModel
            {
                IntNum = IntNum,
                VchConcepto = VchConcepto,
                DcmImporte = DcmImporte,
                VchCoResta = VchCoResta
            });

            return lsDesglosePagosGridViewModel;
        }
        public bool ActualizarEstatusColegiaturaAlumno(Guid UidFechaColegiatura, Guid UidAlumno, DateTime DtFechaPago, Guid UidEstatus)
        {
            return colegiaturasRepository.ActualizarEstatusColegiaturaAlumno(UidFechaColegiatura, UidAlumno, DtFechaPago, UidEstatus);
        }

        public List<PagosColegiaturasViewModel> BuscarColegiaturaPadre(Guid UidCliente, Guid UidUsuario, DateTime FechaInicio, string UidAlumno, string Colegiatura, string NumPago, string EstatusCole, string EstatusPago)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.BuscarColegiaturaPadre(UidCliente, UidUsuario, FechaInicio, UidAlumno, Colegiatura, NumPago, EstatusCole, EstatusPago);
        }

        #endregion

        #region Metodos ReporteLigasPadres
        public List<PagosColegiaturasViewModel> CargarPagosColegiaturasReporte(Guid UidUsuario)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.CargarPagosColegiaturasReporte(UidUsuario);
        }
        public List<PagosReporteLigaPadreViewModels> CargarPagosColeReportePadre(Guid UidUsuario)
        {
            lsPagosReporteLigaPadreViewModels = new List<PagosReporteLigaPadreViewModels>();
            return lsPagosReporteLigaPadreViewModels = colegiaturasRepository.CargarPagosColeReportePadre(UidUsuario);
        }
        public List<PagosReporteLigaPadreViewModels> BuscarPagosColeReportePadre(Guid UidUsuario, string UidAlumno, string Colegiatura, string NumPago, string Folio, string Cuenta, string Banco, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string FormaPago, string Estatus)
        {
            lsPagosReporteLigaPadreViewModels = new List<PagosReporteLigaPadreViewModels>();
            return lsPagosReporteLigaPadreViewModels = colegiaturasRepository.BuscarPagosColeReportePadre(UidUsuario, UidAlumno, Colegiatura, NumPago, Folio, Cuenta, Banco, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, FormaPago, Estatus);
        }
        public void ObtenerPagosColegiaturasRLP(Guid UidPagoColegiatura, string VchMatricula)
        {
            colegiaturasRepository.ObtenerPagosColegiaturasRLP(UidPagoColegiatura, VchMatricula);
        }
        public List<DesglosePagosGridViewModel> FormarDesgloseColeRLP(List<DetallePagosColeGridViewModel> lsDetallePagos, List<DesglosePagosGridViewModel> lsDesglosePagos)
        {
            List<DesglosePagosGridViewModel> lsNewDesglosePagos = new List<DesglosePagosGridViewModel>();

            int num = 0;

            foreach (var item in lsDetallePagos)
            {
                if (lsDesglosePagos.Exists(x=>x.VchConcepto == item.VchDescripcion))
                {
                    lsDesglosePagos.RemoveAt(lsDesglosePagos.FindIndex(x => x.VchConcepto == item.VchDescripcion));
                }
            }

            foreach (var item2 in lsDesglosePagos.OrderBy(x=>x.IntNum))
            {
                lsNewDesglosePagos.Add(new DesglosePagosGridViewModel()
                {
                    IntNum = num + 1,
                    VchConcepto = item2.VchConcepto,
                    DcmImporte = item2.DcmImporte,
                    VchCoResta = item2.VchCoResta
                });
            }

            return lsNewDesglosePagos;
        }
        #endregion

        #region Metodos ReporteLigasEscuelas
        public List<PagosReporteLigaEscuelaViewModels> CargarPagosColeReporteEscuela(Guid UidCliente)
        {
            lsPagosReporteLigaEscuelaViewModels = new List<PagosReporteLigaEscuelaViewModels>();
            return lsPagosReporteLigaEscuelaViewModels = colegiaturasRepository.CargarPagosColeReporteEscuela(UidCliente);
        }
        public decimal ObtenerDatosFechaColegiatura(Guid UidCliente, Guid UidUsuario, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            return colegiaturasRepository.ObtenerDatosFechaColegiatura(UidCliente, UidUsuario, UidFechaColegiatura, UidAlumno);
        }
        public bool ActualizarEstatusFeColegiaturaAlumno(Guid UidFechaColegiatura, Guid UidAlumno, Guid UidEstatus, Guid UidEstatusColeAlumnos, bool BitUsarFecha)
        {
            return colegiaturasRepository.ActualizarEstatusFeColegiaturaAlumno(UidFechaColegiatura, UidAlumno, UidEstatus, UidEstatusColeAlumnos, BitUsarFecha);
        }

        public string ObtenerEstatusColegiaturasRLE(DateTime hoy, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            return colegiaturasRepository.ObtenerEstatusColegiaturasRLE(hoy, UidFechaColegiatura, UidAlumno);
        }

        public List<PagosReporteLigaEscuelaViewModels> BuscarPagosEscuelas(Guid UidCliente, string Colegiatura, string NumPago, string Matricula, string AlNombre, string AlApePaterno, string AlApeMaterno, string TuNombre, string TuApePaterno, string TuApeMaterno, string Folio, string Cuenta, string Banco, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, Guid FormaPago, Guid Estatus)
        {
            lsPagosReporteLigaEscuelaViewModels = new List<PagosReporteLigaEscuelaViewModels>();
            return lsPagosReporteLigaEscuelaViewModels = colegiaturasRepository.BuscarPagosEscuelas(UidCliente, Colegiatura, NumPago, Matricula, AlNombre, AlApePaterno, AlApeMaterno, TuNombre, TuApePaterno, TuApeMaterno, Folio, Cuenta, Banco, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, FormaPago, Estatus);
        }

        public List<PagosColegiaturasViewModel> CargarPagosColegiaturasRLE(Guid UidCliente, Guid UidAlumno)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.CargarPagosColegiaturasRLE(UidCliente, UidAlumno);
        }
        public void ObtenerPagosColegiaturasRLE(Guid UidCliente, Guid UidFechaColegiatura, Guid UidAlumno)
        {
            colegiaturasRepository.ObtenerPagosColegiaturasRLE(UidCliente, UidFechaColegiatura, UidAlumno);
        }
        public List<DesglosePagosGridViewModel> FormarDesgloseColeRLE(List<DetallePagosColeGridViewModel> lsDetallePagos, List<DesglosePagosGridViewModel> lsDesglosePagos)
        {
            List<DesglosePagosGridViewModel> lsNewDesglosePagos = new List<DesglosePagosGridViewModel>();

            int num = 0;

            foreach (var item in lsDetallePagos)
            {
                if (lsDesglosePagos.Exists(x => x.VchConcepto == item.VchDescripcion))
                {
                    lsDesglosePagos.RemoveAt(lsDesglosePagos.FindIndex(x => x.VchConcepto == item.VchDescripcion));
                }
            }

            foreach (var item2 in lsDesglosePagos.OrderBy(x => x.IntNum))
            {
                lsNewDesglosePagos.Add(new DesglosePagosGridViewModel()
                {
                    IntNum = num + 1,
                    VchConcepto = item2.VchConcepto,
                    DcmImporte = item2.DcmImporte,
                    VchCoResta = item2.VchCoResta
                });
            }

            return lsNewDesglosePagos;
        }
        #endregion

        #region Metodos ReporteColegiaturas
        public List<PagosColegiaturasViewModel> CargarReporteColegiaturas(Guid UidCliente)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.CargarReporteColegiaturas(UidCliente);
        }
        public List<PagosColegiaturasViewModel> BuscarReporteColegiaturas(Guid UidCliente, string Colegiatura, string NumPago, string EstatusCole, string EstatusPago, string Matricula, string AlNombre, string AlApePaterno, string AlApeMaterno, string TuNombre, string TuApePaterno, string TuApeMaterno)
        {
            lsPagosColegiaturasViewModel = new List<PagosColegiaturasViewModel>();
            return lsPagosColegiaturasViewModel = colegiaturasRepository.BuscarReporteColegiaturas(UidCliente, Colegiatura, NumPago, EstatusCole, EstatusPago, Matricula, AlNombre, AlApePaterno, AlApeMaterno, TuNombre, TuApePaterno, TuApeMaterno);
        }
        #endregion

        #region Procesos Automaticos
        public List<FCAATMViewModel> ObtenerFechaColegiaturasATM(DateTime Fecha)
        {
            return lsFCAATMViewModel = colegiaturasRepository.ObtenerFechaColegiaturasATM(Fecha);
        }
        public bool ActualizarEstatusFechasPagosATM(Guid UidFechaColegiaturaAlumno, Guid UidEstatusFechaColegiatura)
        {
            return colegiaturasRepository.ActualizarEstatusFechasPagosATM(UidFechaColegiaturaAlumno, UidEstatusFechaColegiatura);
        }
        #endregion

        #region ValidarEstatusColegiatura
        public bool ObtenerEstatusColegiatura(string IdReferencia)
        {
            return colegiaturasRepository.ObtenerEstatusColegiatura(IdReferencia);
        }
        public bool ObtenerEstatusColegiatura2(Guid UidFechaColegiatura)
        {
            return colegiaturasRepository.ObtenerEstatusColegiatura2(UidFechaColegiatura);
        }
        #endregion
    }
}
