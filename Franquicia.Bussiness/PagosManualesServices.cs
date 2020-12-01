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
    public class PagosManualesServices
    {
        private PagosManualesRepository _pagosManualesRepository = new PagosManualesRepository();
        public PagosManualesRepository pagosManualesRepository
        {
            get { return _pagosManualesRepository; }
            set { _pagosManualesRepository = value; }
        }

        public List<PagosManuales> lsPagosManuales = new List<PagosManuales>();
        
        public List<PagosManualesReporteEscuelaViewModel> lsPagosManualesReporteEscuelaViewModel = new List<PagosManualesReporteEscuelaViewModel>();

        #region Metodos PagosColegiatura
        public bool RegistrarPagoManual(Guid UidBanco, string VchCuenta, DateTime DtFHPago, decimal DcmImporte, string VchFolio, Guid UidPagoColegiatura)
        {
            bool result = false;
            if (pagosManualesRepository.RegistrarPagoManual(
                new PagosManuales
                {
                    UidBanco = UidBanco,
                    VchCuenta = VchCuenta,
                    DtFHPago = DtFHPago,
                    DcmImporte = DcmImporte,
                    VchFolio = VchFolio,
                    UidPagoColegiatura = UidPagoColegiatura
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarPagoManual(Guid UidAlumno, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula, string Correo, bool BitBeca, string TipoBeca, decimal Beca, Guid UidEstatus, string Telefono, Guid UidPrefijo)
        {
            bool result = false;
            if (pagosManualesRepository.ActualizarPagoManual(
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
        public bool EliminarPagoManual(Guid UidPagoColegiatura)
        {
            bool result = false;
            if (pagosManualesRepository.EliminarPagoManual(UidPagoColegiatura))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Metodos Clientes
        #region Pagos

        #endregion

        #region Metodos ReporteLigasPadres
        public List<PagosManualesReporteEscuelaViewModel> ObtenerPagoManual(Guid UidPagoColegiatura)
        {
            lsPagosManualesReporteEscuelaViewModel = new List<PagosManualesReporteEscuelaViewModel>();

            return lsPagosManualesReporteEscuelaViewModel = pagosManualesRepository.ObtenerPagoManual(UidPagoColegiatura);
        }
        public List<PagosManualesReporteEscuelaViewModel> ConsultarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            lsPagosManualesReporteEscuelaViewModel = new List<PagosManualesReporteEscuelaViewModel>();

            return lsPagosManualesReporteEscuelaViewModel = pagosManualesRepository.ConsultarDetallePagoColegiatura(UidPagoColegiatura);
        }
        #endregion
        #endregion

        #region Metodos Colegiaturas

        #endregion
    }
}
