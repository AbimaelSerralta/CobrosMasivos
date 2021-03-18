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

        private PrefijosTelefonicosRepository _prefijosTelefonicosRepository = new PrefijosTelefonicosRepository();
        public PrefijosTelefonicosRepository prefijosTelefonicosRepository
        {
            get { return _prefijosTelefonicosRepository; }
            set { _prefijosTelefonicosRepository = value; }
        }
        
        public List<AlumnosGridViewModel> lsAlumnosGridViewModel = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel = new List<AlumnosGridViewModel>();

        public List<AlumnosGridViewModel> lsExcelSeleccionar = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsExcelInsertar = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsExcelActualizar = new List<AlumnosGridViewModel>();
        public List<AlumnosGridViewModel> lsExcelErrores = new List<AlumnosGridViewModel>();

        public List<AlumnosRLEGridViewModel> lsAlumnosRLEGridViewModel = new List<AlumnosRLEGridViewModel>();
        
        public List<AlumnosFiltrosGridViewModel> lsAlumnosFiltrosGridViewModel = new List<AlumnosFiltrosGridViewModel>();

        public string ObtenerIdAlumno(Guid UidAlumno)
        {
            return alumnosRepository.ObtenerIdAlumno(UidAlumno);
        }

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
        public bool RegistrarAlumno(Guid UidAlumno, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula, string Correo, bool BitBeca, string TipoBeca, decimal Beca, string Telefono, Guid UidPrefijo, Guid UidCliente)
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
        public void BuscarAlumnos(string Identificador, string Matricula, string Correo, string Nombre, string ApePaterno, string ApeMaterno, string Celular, string Asociado, string Beca, Guid UidEstatus, string Colegiatura, Guid UidCliente)
        {
            lsAlumnosGridViewModel = alumnosRepository.BuscarAlumnos(Identificador, Matricula, Correo, Nombre, ApePaterno, ApeMaterno, Celular, Asociado, Beca, UidEstatus, Colegiatura, UidCliente);
        }

        public bool DesasociarPadreAlumno(Guid UidUsuario, Guid UidAlumno)
        {
            bool result = false;
            if (alumnosRepository.DesasociarPadreAlumno(UidUsuario, UidAlumno))
            {
                result = true;
            }
            return result;
        }
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
        public bool EliminarClienteAlumnos(Guid UidCliente, Guid UidUsuario)
        {
            bool result = false;
            if (alumnosRepository.EliminarClienteAlumnos(UidCliente, UidUsuario))
            {
                result = true;
            }
            return result;
        }
        public void ObtenerClienteAlumnos(Guid UidUidCliente, Guid UidUsuario)
        {
            lsAlumnosGridViewModel = alumnosRepository.ObtenerClienteAlumnos(UidUidCliente, UidUsuario);
            lsSelectAlumnosGridViewModel = alumnosRepository.ObtenerClienteAlumnos(UidUidCliente, UidUsuario);
        }

        public void AsignarAlumnos(List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, Guid UidUsuario, string IntCanAlum, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            lsAlumnosGridViewModel = alumnosRepository.AsignarAlumnos(lsSelectAlumnosGridViewModel, UidCliente, UidUsuario, IntCanAlum, Identificador, Nombre, ApePaterno, ApeMaterno, Matricula);
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
        public List<AlumnosGridViewModel> ActualizarLsAsignarAlumnosTodo(List<AlumnosGridViewModel> lsAlumnos, bool accion)
        {
            List<AlumnosGridViewModel> lsNuevoAlumnosGridViewModel = new List<AlumnosGridViewModel>();

            foreach (var item in lsAlumnos)
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
                    if (!item.blSeleccionado)
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
                }
                else
                {
                    List<AlumnosGridViewModel> predicate = lsSelectAlumnosGridViewModel.FindAll(x => x.UidAlumno == item.UidAlumno);

                    for (int i = 0; i < predicate.Count; i++)
                    {
                        lsSelectAlumnosGridViewModel.Remove(predicate[i]);
                    }
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
        public void AsignarColeAlumnos(List<AlumnosGridViewModel> lsExcelSelect, List<AlumnosGridViewModel> lsSelectAlumnosGridViewModel, Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            lsAlumnosGridViewModel = alumnosRepository.AsignarColeAlumnos(lsExcelSelect, lsSelectAlumnosGridViewModel, UidCliente, Identificador, Nombre, ApePaterno, ApeMaterno, Matricula);
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

        #region Alumnos
        public void ValidarAlumnosExcelToList(DataTable dataTable)
        {
            lsExcelErrores.Clear();
            lsExcelInsertar.Clear();

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["IDENTIFICADOR"].ToString()) && !string.IsNullOrEmpty(item["MATRICULA"].ToString())
                    && !string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString())
                    && !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) && !string.IsNullOrEmpty(item["ESTATUS"].ToString()))
                {
                    bool error = false;
                    Guid UidEstatus = Guid.Empty;
                    bool BitBeca = false;
                    decimal Cantidad = 0;

                    string Telefono = string.Empty;
                    Guid UidPrefijo = Guid.Parse("ABB854C4-E7ED-420F-8561-AA4B61BF5B0F");

                    string regexCorreo = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex reCorreo = new Regex(regexCorreo);

                    if (!string.IsNullOrEmpty(item["CORREO"].ToString()))
                    {
                        if (reCorreo.IsMatch(item["CORREO"].ToString()))
                        {
                        }
                        else
                        {
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(item["CELULAR"].ToString()))
                    {
                        if (item["CELULAR"].ToString().Contains("(") && item["CELULAR"].ToString().Contains("+") && item["CELULAR"].ToString().Contains(")"))
                        {
                            string output = item["CELULAR"].ToString().Split('(', ')')[1];
                            prefijosTelefonicosRepository.ValidarPrefijoTelefonico(output);

                            if (item["CELULAR"].ToString().Split('(', ')')[2].Count() == 10)
                            {
                                if (prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != null && prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != Guid.Empty)
                                {
                                    Telefono = item["CELULAR"].ToString().Split('(', ')')[2];
                                    UidPrefijo = prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo;
                                }
                                else
                                {
                                    error = true;
                                }
                            }
                            else
                            {
                                error = true;
                            }
                        }
                        else
                        {
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(item["BECA"].ToString()))
                    {
                        if (item["BECA"].ToString().ToUpper() == "SI")
                        {
                            BitBeca = true;

                            if (!string.IsNullOrEmpty(item["TIPO BECA"].ToString()))
                            {
                                if (item["TIPO BECA"].ToString().ToUpper() == "CANTIDAD")
                                {
                                    if (decimal.TryParse(item["CANTIDAD"].ToString(), out Cantidad))
                                    {
                                        Cantidad = decimal.Parse(item["CANTIDAD"].ToString());
                                    }
                                    else
                                    {
                                        error = true;
                                    }
                                }
                                else if (item["TIPO BECA"].ToString().ToUpper() == "PORCENTAJE")
                                {
                                    if (decimal.TryParse(item["CANTIDAD"].ToString(), out Cantidad))
                                    {
                                        if (decimal.Parse(item["CANTIDAD"].ToString()) <= 100)
                                        {
                                            Cantidad = decimal.Parse(item["CANTIDAD"].ToString());
                                        }
                                        else
                                        {
                                            error = true;
                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                    }
                                }
                                else
                                {
                                    error = true;
                                }
                            }
                            else
                            {
                                error = true;
                            }
                        }
                    }

                    string ResultUidEstatus = validacionesRepository.ObtenerUidEstatus(item["ESTATUS"].ToString());

                    if (!string.IsNullOrEmpty(ResultUidEstatus))
                    {
                        UidEstatus = Guid.Parse(ResultUidEstatus);
                    }
                    else
                    {
                        error = true;
                    }

                    if (error)
                    {
                        lsExcelErrores.Add(new AlumnosGridViewModel()
                        {
                            VchIdentificador = item["IDENTIFICADOR"].ToString(),
                            VchMatricula = item["MATRICULA"].ToString(),
                            VchNombres = item["NOMBRE(S)"].ToString(),
                            VchApePaterno = item["APEPATERNO"].ToString(),
                            VchApeMaterno = item["APEMATERNO"].ToString(),
                            VchCorreo = item["CORREO"].ToString(),

                            VchTelefono = item["CELULAR"].ToString(),
                            UidPrefijo = prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo,
                            BitBeca = BitBeca,
                            VchTipoBeca = item["TIPO BECA"].ToString(),
                            DcmBeca = Cantidad,
                            UidEstatus = UidEstatus,
                            VchDescripcion = item["ESTATUS"].ToString()
                        });
                    }
                    else
                    {
                        lsExcelInsertar.Add(new AlumnosGridViewModel()
                        {
                            VchIdentificador = item["IDENTIFICADOR"].ToString().ToUpper(),
                            VchMatricula = item["MATRICULA"].ToString().ToUpper(),
                            VchNombres = item["NOMBRE(S)"].ToString().ToUpper(),
                            VchApePaterno = item["APEPATERNO"].ToString().ToUpper(),
                            VchApeMaterno = item["APEMATERNO"].ToString().ToUpper(),
                            VchCorreo = item["CORREO"].ToString().ToUpper(),

                            VchTelefono = Telefono,
                            UidPrefijo = UidPrefijo,
                            BitBeca = BitBeca,
                            VchTipoBeca = item["TIPO BECA"].ToString().ToUpper(),
                            DcmBeca = Cantidad,
                            UidEstatus = UidEstatus,
                            VchDescripcion = item["ESTATUS"].ToString().ToUpper()
                        });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["IDENTIFICADOR"].ToString()) || !string.IsNullOrEmpty(item["MATRICULA"].ToString())
                        || !string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) || !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) ||
                        !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) || !string.IsNullOrEmpty(item["ESTATUS"].ToString()))
                    {
                        bool BitBeca = false;

                        if (!string.IsNullOrEmpty(item["BECA"].ToString()))
                        {
                            if (item["BECA"].ToString().ToUpper() == "SI")
                            {
                                BitBeca = true;
                            }
                        }

                        lsExcelErrores.Add(new AlumnosGridViewModel()
                        {
                            VchIdentificador = item["IDENTIFICADOR"].ToString(),
                            VchMatricula = item["MATRICULA"].ToString(),
                            VchNombres = item["NOMBRE(S)"].ToString(),
                            VchApePaterno = item["APEPATERNO"].ToString(),
                            VchApeMaterno = item["APEMATERNO"].ToString(),
                            VchCorreo = item["CORREO"].ToString(),

                            VchTelefono = item["CELULAR"].ToString(),
                            UidPrefijo = prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo,
                            BitBeca = BitBeca,
                            VchTipoBeca = item["TIPO BECA"].ToString(),
                            DcmBeca = decimal.Parse(item["CANTIDAD"].ToString()),
                            UidEstatus = Guid.Empty,
                            VchDescripcion = item["ESTATUS"].ToString()
                        });
                    }
                }
            }
        }
        public void AccionAlumnosExcelToList(List<AlumnosGridViewModel> lsAccionAlumnos, Guid UidCliente)
        {
            alumnosRepository.AccionAlumnosExcelToList(lsAccionAlumnos, UidCliente);
        }
        #endregion

        #region Padres
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

        #endregion

        #region Metodos ReporteLigasEscuelas
        public void BuscarAlumnosRLE(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Matricula)
        {
            lsAlumnosRLEGridViewModel = alumnosRepository.BuscarAlumnosRLE(UidCliente, Identificador, Nombre, ApePaterno, ApeMaterno, Matricula);
        }
        #endregion

        #region ReporteLigasPadre
        public List<AlumnosFiltrosGridViewModel> CargarFiltroAlumnosRLP(Guid UidUsuario)
        {
            lsAlumnosFiltrosGridViewModel = new List<AlumnosFiltrosGridViewModel>();
            return lsAlumnosFiltrosGridViewModel = alumnosRepository.CargarFiltroAlumnosRLP(UidUsuario);
        }
        #endregion
    }
}
