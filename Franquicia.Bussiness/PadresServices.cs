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
    public class PadresServices
    {
        private PadresRepository _padresRepository = new PadresRepository();
        public PadresRepository padresRepository
        {
            get { return _padresRepository; }
            set { _padresRepository = value; }
        }

        private UsuariosRepository _usuariosRepository = new UsuariosRepository();
        public UsuariosRepository usuariosRepository
        {
            get { return _usuariosRepository; }
            set { _usuariosRepository = value; }
        }

        private PrefijosTelefonicosRepository _prefijosTelefonicosRepository = new PrefijosTelefonicosRepository();
        public PrefijosTelefonicosRepository prefijosTelefonicosRepository
        {
            get { return _prefijosTelefonicosRepository; }
            set { _prefijosTelefonicosRepository = value; }
        }

        private ValidacionesRepository _validacionesRepository = new ValidacionesRepository();
        public ValidacionesRepository validacionesRepository
        {
            get { return _validacionesRepository; }
            set { _validacionesRepository = value; }
        }

        public List<Padres> lsPadres = new List<Padres>();

        public List<Padres> lsActualizarPadres = new List<Padres>();

        public List<Alumnos> lsExcelInsertarAlumnos = new List<Alumnos>();
        public List<PadresGridViewModel> lsExcelInsertar = new List<PadresGridViewModel>();
        public List<PadresGridViewModel> lsExcelErrores = new List<PadresGridViewModel>();

        #region MetodosUsuarios
        public void CargarPadres(Guid UidCliente, Guid UidTipoPerfil)
        {
            lsPadres = padresRepository.CargarPadres(UidCliente, UidTipoPerfil);
        }
        public void ObtenerPadre(Guid UidUsuario)
        {
            padresRepository.padres = new Padres();
            padresRepository.padres = lsPadres.Find(x => x.UidUsuario == UidUsuario);
        }
        public bool RegistrarPadres(Guid UidUsuario,
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
            string Telefono, Guid UidPrefijo, Guid UidCliente)
        {
            bool result = false;
            if (padresRepository.RegistrarPadres(
                new Padres
                {
                    UidUsuario = UidUsuario,
                    StrNombre = Nombre,
                    StrApePaterno = ApePaterno,
                    StrApeMaterno = ApeMaterno,
                    StrCorreo = Correo,
                    VchUsuario = Usuario,
                    VchContrasenia = Password,
                    UidSegPerfil = UidSegPerfil
                },
                UidSegPerfilEscuela,
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidPrefijo = UidPrefijo
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarPadres(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Telefono, Guid UidPrefijo, Guid UidCliente)
        {

            bool result = false;
            if (padresRepository.ActualizarPadres(
                new Padres
                {
                    UidUsuario = UidUsuario,
                    StrNombre = Nombre,
                    StrApePaterno = ApePaterno,
                    StrApeMaterno = ApeMaterno,
                    StrCorreo = Correo,
                    UidEstatus = UidEstatus,
                    VchUsuario = Usuario,
                    VchContrasenia = Password,
                    UidSegPerfil = UidSegPerfil
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidPrefijo = UidPrefijo
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }
        public bool RegistrarDireccionUsuarios(Guid UidUsuario, string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia)
        {
            bool result = false;
            if (padresRepository.RegistrarDireccionUsuarios(
                    UidUsuario,
                new DireccionesUsuarios
                {
                    Identificador = Identificador,
                    UidPais = UidPais,
                    UidEstado = UidEstado,
                    UidMunicipio = Municipio,
                    UidCiudad = UidCiudad,
                    UidColonia = UidColonia,
                    Calle = Calle,
                    EntreCalle = EntreCalle,
                    YCalle = YCalle,
                    NumeroExterior = NumeroExterior,
                    NumeroInterior = NumeroInterior,
                    CodigoPostal = CodigoPostal,
                    Referencia = Referencia
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarDireccionUsuarios(Guid UidUsuario, string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia)
        {
            bool result = false;
            if (padresRepository.ActualizarDireccionUsuarios(
                    UidUsuario,
                new DireccionesUsuarios
                {
                    Identificador = Identificador,
                    UidPais = UidPais,
                    UidEstado = UidEstado,
                    UidMunicipio = Municipio,
                    UidCiudad = UidCiudad,
                    UidColonia = UidColonia,
                    Calle = Calle,
                    EntreCalle = EntreCalle,
                    YCalle = YCalle,
                    NumeroExterior = NumeroExterior,
                    NumeroInterior = NumeroInterior,
                    CodigoPostal = CodigoPostal,
                    Referencia = Referencia
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public void BuscarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        {
            lsPadres = padresRepository.BuscarUsuariosFinales(UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, UidEstatus);
        }
        public void AsociarUsuariosFinales(string Correo)
        {
            padresRepository.AsociarUsuariosFinales(Correo);
        }
        public bool AsociarClienteUsuario(Guid UidCliente, Guid UidUsuario)
        {
            return padresRepository.AsociarUsuarioCliente(UidCliente, UidUsuario);
        }
        #endregion


        #region METODOS EXCEL

        #region Padres
        public void ValidarExcelToList(DataTable dataTable, Guid UidCliente)
        {
            lsExcelErrores.Clear();
            lsExcelInsertar.Clear();

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) &&
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) && !string.IsNullOrEmpty(item["CORREO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()))
                {
                    bool error = false;
                    bool errorMatricula = false;

                    List<string> lsErrorMatricula = new List<string>();

                    string regexCorreo = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                    Regex reCorreo = new Regex(regexCorreo);

                    if (reCorreo.IsMatch(item["CORREO"].ToString()))
                    {
                        if (item["CELULAR"].ToString().Contains("(") && item["CELULAR"].ToString().Contains("+") && item["CELULAR"].ToString().Contains(")"))
                        {
                            string output = item["CELULAR"].ToString().Split('(', ')')[1];
                            prefijosTelefonicosRepository.ValidarPrefijoTelefonico(output);

                            if (item["CELULAR"].ToString().Split('(', ')')[2].Count() == 10)
                            {
                                if (prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != null && prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != Guid.Empty)
                                {
                                    if (!string.IsNullOrEmpty(item["MATRICULA(S)"].ToString()))
                                    {
                                        string[] matriculas = Regex.Split(item["MATRICULA(S)"].ToString().Trim().ToUpper(), ",");

                                        for (int i = 0; i < matriculas.Length; i++)
                                        {
                                            var DataAlumno = validacionesRepository.ExisteAlumno(matriculas[i].Trim(), UidCliente);

                                            if (DataAlumno.Item1)
                                            {
                                                if (validacionesRepository.ExisteAlumnoAsociado(Guid.Parse(DataAlumno.Item2)))
                                                {
                                                    errorMatricula = true;
                                                }
                                                else
                                                {
                                                    lsExcelInsertarAlumnos.Add(new Alumnos
                                                    {
                                                        UidAlumno = Guid.Parse(DataAlumno.Item2)
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                lsErrorMatricula.Add(matriculas[i].Trim());
                                                errorMatricula = true;
                                            }
                                        }
                                    }

                                    lsExcelInsertar.Add(new PadresGridViewModel()
                                    {
                                        StrNombre = item["NOMBRE(S)"].ToString(),
                                        StrApePaterno = item["APEPATERNO"].ToString(),
                                        StrApeMaterno = item["APEMATERNO"].ToString(),
                                        StrCorreo = item["CORREO"].ToString(),
                                        StrTelefono = item["CELULAR"].ToString().Split('(', ')')[2],
                                        UidPrefijo = prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo
                                    });
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

                    if (error)
                    {
                        lsExcelErrores.Add(new PadresGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            VchMatricula = item["MATRICULA(S)"].ToString()
                        });
                    }

                    if (errorMatricula)
                    {
                        lsExcelErrores.Add(new PadresGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            VchMatricula = string.Join(", ", lsErrorMatricula.ToArray())
                        });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) || !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) ||
                        !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) || !string.IsNullOrEmpty(item["CORREO"].ToString()) ||
                        !string.IsNullOrEmpty(item["CELULAR"].ToString()))
                    {
                        lsExcelErrores.Add(new PadresGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            VchMatricula = item["MATRICULA(S)"].ToString()
                        });
                    }
                }
            }
        }

        public void AccionPadresExcelToList(List<Alumnos> lsAlumnos, List<PadresGridViewModel> lsAccionPadres, Guid UidSegPerfil, Guid UidSegPerfilEscuela, Guid UidCliente)
        {
            padresRepository.AccionPadresExcelToList(lsAlumnos, lsAccionPadres, UidSegPerfil, UidSegPerfilEscuela, UidCliente);
        }
        
        #endregion

        #endregion
    }
}
