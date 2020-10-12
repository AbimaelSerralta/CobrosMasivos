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
        public bool RegistrarPagoColegiatura(Guid UidPagoColegiatura, string VchAlumno, string VchMatricula, DateTime DtFHPago, string VchPromocionDePago, string VchComisionBancaria, bool BitSubtotal, decimal DcmSubtotal, bool BitComisionBancaria, decimal DcmComisionBancaria, bool BitPromocionDePago, decimal DcmPromocionDePago, decimal DcmTotal, Guid UidFechaColegiatura)
        {
            bool result = false;
            if (pagosColegiaturasRepository.RegistrarPagoColegiatura(
                new PagosColegiaturas
                {
                    UidPagoColegiatura = UidPagoColegiatura,
                    VchAlumno = VchAlumno,
                    VchMatricula = VchMatricula,
                    DtFHPago = DtFHPago,
                    VchPromocionDePago = VchPromocionDePago,
                    VchComisionBancaria = VchComisionBancaria,
                    BitSubtotal = BitSubtotal,
                    DcmSubtotal = DcmSubtotal,
                    BitComisionBancaria = BitComisionBancaria,
                    DcmComisionBancaria = DcmComisionBancaria,
                    BitPromocionDePago = BitPromocionDePago,
                    DcmPromocionDePago = DcmPromocionDePago,
                    DcmTotal = DcmTotal,
                    UidFechaColegiatura = UidFechaColegiatura
                }
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

        #region Metodos Clientes

        #endregion

        #region Metodos Colegiaturas

        #endregion
    }
}
