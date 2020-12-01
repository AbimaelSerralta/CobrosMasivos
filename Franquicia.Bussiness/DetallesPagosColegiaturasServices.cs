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
    public class DetallesPagosColegiaturasServices
    {
        private DetallesPagosColegiaturasRepository _detallesPagosColegiaturasRepository = new DetallesPagosColegiaturasRepository();
        public DetallesPagosColegiaturasRepository detallesPagosColegiaturasRepository
        {
            get { return _detallesPagosColegiaturasRepository; }
            set { _detallesPagosColegiaturasRepository = value; }
        }

        private ValidacionesRepository _validacionesRepository = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesRepository; }
            set { _validacionesRepository = value; }
        }

        public List<DetallesPagosColegiaturas> lsDetallesPagosColegiaturas = new List<DetallesPagosColegiaturas>();
        
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
        public bool RegistrarDetallePagoColegiatura(int IntNum, string VchDescripcion, decimal DcmImporte, Guid UidPagoColegiatura)
        {
            bool result = false;
            if (detallesPagosColegiaturasRepository.RegistrarDetallePagoColegiatura(
                new DetallesPagosColegiaturas
                {
                    IntNum = IntNum,
                    VchDescripcion = VchDescripcion,
                    DcmImporte = DcmImporte,
                    UidPagoColegiatura = UidPagoColegiatura
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            bool result = false;
            if (detallesPagosColegiaturasRepository.EliminarDetallePagoColegiatura(UidPagoColegiatura))
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
        #region Alumnos

        #endregion

        #region Metodos Tutores

        #endregion

        #region Metodos Colegiaturas

        #endregion

        #region Metodos ReporteLigasPadre

        #region ReportViewer
        public List<DetallePagosColeGridViewModel> rdlcObtenerDetallePagoColegiatura(Guid UidPagoColegiatura)
        {
            lsDetallePagosColeGridViewModel = new List<DetallePagosColeGridViewModel>();
            return lsDetallePagosColeGridViewModel = detallesPagosColegiaturasRepository.rdlcObtenerDetallePagoColegiatura(UidPagoColegiatura);
        }

        #endregion
        #endregion
        #endregion
    }
}
