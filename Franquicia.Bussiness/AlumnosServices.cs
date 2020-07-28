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
    public class AlumnosServices
    {
        private AlumnosRepository _alumnosRepository = new AlumnosRepository();
        public AlumnosRepository alumnosRepository
        {
            get { return _alumnosRepository; }
            set { _alumnosRepository = value; }
        }

        private ValidacionesRepository _validacionesRepository = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesRepository; }
            set { _validacionesRepository = value; }
        }

        public List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel = new List<AlumnosGridViewModel>();

        public List<AlumnosGridViewModel> lsExcelSeleccionar = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsExcelErrores = new List<AlumnosGridViewModel>();

        #region Metodos Alumnos
        public void CargarAlumnos(Guid UidCliente)
        {
            lsAlumnosGridViewModel = alumnosRepository.CargarAlumnos(UidCliente);
        }
        public void ObtenerAlumno(Guid UidAlumno)
        {
            alumnosRepository.alumnosGridViewModel = new AlumnosGridViewModel();
            alumnosRepository.alumnosGridViewModel = lsAlumnosGridViewModel.Find(x => x.UidAlumno == UidAlumno);
        }
        public bool RegistrarUsuarios(Guid UidAlumno, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula, string Correo, bool BitBeca, string TipoBeca, decimal Beca, string Telefono, Guid UidPrefijo, Guid UidCliente)
        {
            bool result = false;
            if (alumnosRepository.RegistrarAlumno(
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
                    UidCliente = UidCliente
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

        public bool ActualizarAlumno(Guid UidAlumno, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula, string Correo, bool BitBeca, string TipoBeca, decimal Beca, Guid UidEstatus, string Telefono, Guid UidPrefijo)
        {
            bool result = false;
            if (alumnosRepository.ActualizarAlumno(
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
        //public void BuscarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        //{
        //    lsUsuariosCompletos = usuariosCompletosRepository.BuscarUsuariosFinales(UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, UidEstatus);
        //}
        #endregion

        #region Metodos Clientes
        public bool RegistrarClienteAlumnos(Guid UidUsuario, Guid UidAlumno)
        {
            bool result = false;
            if (alumnosRepository.RegistrarClienteAlumnos(UidUsuario, UidAlumno))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarClienteAlumnos(Guid UidUsuario)
        {
            bool result = false;
            if (alumnosRepository.EliminarClienteAlumnos(UidUsuario))
            {
                result = true;
            }
            return result;
        }
        public void ObtenerClienteAlumnos(Guid UidUsuario)
        {
            lsAlumnosGridViewModel = alumnosRepository.ObtenerClienteAlumnos(UidUsuario);
            lsSelectAlumnosGridViewModel = alumnosRepository.ObtenerClienteAlumnos(UidUsuario);
        }

        public void AsignarAlumnos(List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            lsAlumnosGridViewModel = alumnosRepository.AsignarAlumnos(lsSelectAlumnosGridViewModel, UidCliente, UidUsuario, Nombre, ApePaterno, ApeMaterno, Matricula);
        }
        public List<AlumnosGridViewModel> ActualizarLsAsignarAlumnos(List<AlumnosGridViewModel> lsAlumnos, Guid UidAlumno, bool accion)
        {
            List<AlumnosGridViewModel> lsNuevoAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            foreach (var item in lsAlumnos)
            {
                if (item.UidAlumno == UidAlumno)
                {
                    lsNuevoAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = item.UidAlumno,
                        VchNombres = item.VchNombres,
                        VchApePaterno = item.VchApePaterno,
                        VchApeMaterno = item.VchApeMaterno,
                        VchIdentificador = item.VchIdentificador,
                        VchMatricula = item.VchMatricula,
                        UidEstatus = item.UidEstatus,
                        blSeleccionado = accion
                    });

                    if (accion)
                    {
                        lsSelectAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                        {
                            UidAlumno = item.UidAlumno,
                            VchNombres = item.VchNombres,
                            VchApePaterno = item.VchApePaterno,
                            VchApeMaterno = item.VchApeMaterno,
                            VchIdentificador = item.VchIdentificador,
                            VchMatricula = item.VchMatricula,
                            UidEstatus = item.UidEstatus,
                            blSeleccionado = accion
                        });
                    }
                    else
                    {
                        lsSelectAlumnosGridViewModel.RemoveAt(lsSelectAlumnosGridViewModel.FindIndex(x => x.UidAlumno == UidAlumno));
                    }
                }
                else
                {
                    lsNuevoAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = item.UidAlumno,
                        VchNombres = item.VchNombres,
                        VchApePaterno = item.VchApePaterno,
                        VchApeMaterno = item.VchApeMaterno,
                        VchIdentificador = item.VchIdentificador,
                        VchMatricula = item.VchMatricula,
                        UidEstatus = item.UidEstatus,
                        blSeleccionado = item.blSeleccionado
                    });
                }
            }

            return lsAlumnosGridViewModel = lsNuevoAlumnosGridViewModel;
        }
        #endregion

        #region Metodos Colegiaturas
        public bool RegistrarColeAlumnos(Guid UidColegiatura, Guid UidAlumno)
        {
            bool result = false;
            if (alumnosRepository.RegistrarColeAlumnos(UidColegiatura, UidAlumno))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarColeAlumnos(Guid UidColegiatura)
        {
            bool result = false;
            if (alumnosRepository.EliminarColeAlumnos(UidColegiatura))
            {
                result = true;
            }
            return result;
        }

        public void ObtenerColeAlumnos(Guid UidColegiatura)
        {
            lsAlumnosGridViewModel = alumnosRepository.ObtenerColeAlumnos(UidColegiatura);
            lsSelectAlumnosGridViewModel = alumnosRepository.ObtenerColeAlumnos(UidColegiatura);
        }
        public void AsignarColeAlumnos(List<AlumnosGridViewModel> lsExcelSelect, List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            lsAlumnosGridViewModel = alumnosRepository.AsignarColeAlumnos(lsExcelSelect, lsSelectAlumnosGridViewModel, UidCliente, Nombre, ApePaterno, ApeMaterno, Matricula);
        }
        public List<AlumnosGridViewModel> ActualizarLsAsignarColeAlumnos(List<AlumnosGridViewModel> lsAlumnos, Guid UidAlumno, bool accion)
        {
            List<AlumnosGridViewModel> lsNuevoAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            foreach (var item in lsAlumnos)
            {
                if (item.UidAlumno == UidAlumno)
                {
                    lsNuevoAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = item.UidAlumno,
                        VchNombres = item.VchNombres,
                        VchApePaterno = item.VchApePaterno,
                        VchApeMaterno = item.VchApeMaterno,
                        VchMatricula = item.VchMatricula,
                        UidEstatus = item.UidEstatus,
                        blSeleccionado = accion
                    });

                    if (accion)
                    {
                        lsSelectAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                        {
                            UidAlumno = item.UidAlumno,
                            VchNombres = item.VchNombres,
                            VchApePaterno = item.VchApePaterno,
                            VchApeMaterno = item.VchApeMaterno,
                            VchMatricula = item.VchMatricula,
                            UidEstatus = item.UidEstatus,
                            blSeleccionado = accion
                        });
                    }
                    else
                    {
                        lsSelectAlumnosGridViewModel.RemoveAt(lsSelectAlumnosGridViewModel.FindIndex(x => x.UidAlumno == UidAlumno));
                    }
                }
                else
                {
                    lsNuevoAlumnosGridViewModel.Add(new AlumnosGridViewModel()
                    {
                        UidAlumno = item.UidAlumno,
                        VchNombres = item.VchNombres,
                        VchApePaterno = item.VchApePaterno,
                        VchApeMaterno = item.VchApeMaterno,
                        VchMatricula = item.VchMatricula,
                        UidEstatus = item.UidEstatus,
                        blSeleccionado = item.blSeleccionado
                    });
                }
            }

            return lsAlumnosGridViewModel = lsNuevoAlumnosGridViewModel;
        }

        #region Metodos de Excel
        public void ValidarExcelToList(DataTable dataTable, Guid UidCliente)
        {
            lsExcelErrores.Clear();
            lsExcelSeleccionar.Clear();

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["MATRICULA"].ToString()))
                {
                    if (validacionesRepository.ExisteMatriculaAlumno(item["MATRICULA"].ToString(), UidCliente))
                    {
                        lsExcelSeleccionar.Add(new AlumnosGridViewModel()
                        {
                            VchMatricula = item["MATRICULA"].ToString()
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(item["MATRICULA"].ToString()))
                        {
                            bool beca = false;
                            decimal Cantidad = 0;

                            if (item["BECA"].ToString().ToUpper() == "SI")
                            {
                                beca = true;
                            }

                            if (item["BECA"].ToString().ToUpper() == "NO")
                            {
                                beca = false;
                            }

                            if (!string.IsNullOrEmpty(item["CANTIDAD"].ToString()))
                            {
                                Cantidad = decimal.Parse(item["CANTIDAD"].ToString());
                            }

                            lsExcelErrores.Add(new AlumnosGridViewModel()
                            {
                                //Regresa todo lo que trae el excel
                                VchMatricula = item["MATRICULA"].ToString(),
                                VchIdentificador = item["IDENTIFICADOR"].ToString(),
                                VchNombres = item["NOMBRE(S)"].ToString(),
                                VchApePaterno = item["APEPATERNO"].ToString(),
                                VchApeMaterno = item["APEMATERNO"].ToString(),
                                VchCorreo = item["CORREO"].ToString(),
                                VchTelefono = item["CELULAR"].ToString(),
                                BitBeca = beca,
                                VchTipoBeca = item["TIPO BECA"].ToString(),
                                DcmBeca = Cantidad,
                                VchDescripcion = item["ESTATUS"].ToString()
                            });
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["MATRICULA"].ToString()))
                    {
                        bool beca = false;
                        decimal Cantidad = 0;

                        if (item["BECA"].ToString().ToUpper() == "SI")
                        {
                            beca = true;
                        }

                        if (item["BECA"].ToString().ToUpper() == "NO")
                        {
                            beca = false;
                        }

                        if (!string.IsNullOrEmpty(item["CANTIDAD"].ToString()))
                        {
                            Cantidad = decimal.Parse(item["CANTIDAD"].ToString());
                        }

                        lsExcelErrores.Add(new AlumnosGridViewModel()
                        {
                            //Regresa todo lo que trae el excel
                            VchMatricula = item["MATRICULA"].ToString(),
                            VchIdentificador = item["IDENTIFICADOR"].ToString(),
                            VchNombres = item["NOMBRE(S)"].ToString(),
                            VchApePaterno = item["APEPATERNO"].ToString(),
                            VchApeMaterno = item["APEMATERNO"].ToString(),
                            VchCorreo = item["CORREO"].ToString(),
                            VchTelefono = item["CELULAR"].ToString(),
                            BitBeca = beca,
                            VchTipoBeca = item["TIPO BECA"].ToString(),
                            DcmBeca = Cantidad,
                            VchDescripcion = item["ESTATUS"].ToString()
                        });
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}
