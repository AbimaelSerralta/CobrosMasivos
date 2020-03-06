using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class UsuariosCompletosServices
    {
        private UsuariosCompletosRepository _usuariosCompletosRepository = new UsuariosCompletosRepository();
        public UsuariosCompletosRepository usuariosCompletosRepository
        {
            get { return _usuariosCompletosRepository; }
            set { _usuariosCompletosRepository = value; }
        }

        private UsuariosRepository _usuariosRepository = new UsuariosRepository();
        public UsuariosRepository usuariosRepository
        {
            get { return _usuariosRepository; }
            set { _usuariosRepository = value; }
        }


        public List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

        public List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();

        public List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsgvUsuariosSeleccionados = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsLigasInsertar = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsLigasErrores = new List<LigasUsuariosGridViewModel>();


        public void CargarAdministradores(Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarAdministradores(UidTipoPerfil);
        }

        #region Metodos Principal
        public bool RegistrarAdministradores(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia)
        {
            Guid UidUsuario = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.RegistrarAdministradores(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidFranquicia
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarAdministradores(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarAdministradores(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidFranquicia
                ))
            {
                result = true;
            }
            return result;
        }

        public void ObtenerAdministrador(Guid UidAdministrador)
        {
            usuariosCompletosRepository.usuarioCompleto = new UsuariosCompletos();
            usuariosCompletosRepository.usuarioCompleto = lsUsuariosCompletos.Find(x => x.UidUsuario == UidAdministrador);
        }

        public bool RegistrarAdministradoresPrincipal(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono)
        {
            Guid UidUsuario = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.RegistrarAdministradoresPrincipal(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                }
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarAdministradoresPrincipal(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarAdministradoresPrincipal(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                }
                ))
            {
                result = true;
            }
            return result;
        }

        #endregion

        #region Metodos Franquicia
        public void CargarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarAdministradoresFranquicia(UidFranquiciatario, UidTipoPerfilFranquicia);
        }
        public bool RegistrarAdministradoresFranquicia(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia)
        {
            Guid UidUsuario = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.RegistrarAdministradoresFranquicia(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidFranquicia
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarAdministradoresFranquicia(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarAdministradoresFranquicia(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidFranquicia
                ))
            {
                result = true;
            }
            return result;
        }


        #endregion

        #region MetodosClientes
        public void CargarAdministradoresCliente(Guid UidFranquiciatario, Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarAdministradoresCliente(UidFranquiciatario, UidTipoPerfil);
        }
        public bool RegistrarAdministradoresCliente(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidCliente)
        {
            Guid UidUsuario = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.RegistrarAdministradoresCliente(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarAdministradoresCliente(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidCliente)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarAdministradoresCliente(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region MetodosUsuarios
        public void CargarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarUsuariosFinales(UidCliente, UidTipoPerfil);
        }
        public bool RegistrarUsuarios(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidCliente)
        {
            Guid UidUsuario = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.RegistrarUsuarios(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarUsuarios(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia,
            string Telefono, Guid UidTipoTelefono, Guid UidCliente)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarUsuarios(
                new UsuariosCompletos
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
                },
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono
                },
                UidCliente
                ))
            {
                result = true;
            }
            return result;
        }

        public void BuscarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarUsuariosFinales(UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, UidEstatus);
        }

        #endregion


        #region MetodosExel
        #region Simple
        public void CargarUsuariosFinales(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.CargarUsuariosFinales(lsLigasUsuarios, UidCliente, UidTipoPerfil);
        }
        public void ActualizarListaUsuarios(List<LigasUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuarios(lsLigasUsuarios, IdUsuario, accion);
        }
        public void EliminarItemgvUsuariosSeleccionados(int IdUsuario)
        {
            lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.IdUsuario == IdUsuario));
        }
        public void ExcelToList(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidCliente)
        {
            lsgvUsuariosSeleccionados = usuariosCompletosRepository.ExcelToList(lsLigasUsuariosGridView, lsLigasInsertar, UidCliente);
        }
        public void ValidarExcelToList(DataTable dataTable)
        {
            lsLigasErrores.Clear();
            lsLigasInsertar.Clear();

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) &&
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) && !string.IsNullOrEmpty(item["CORREO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()))
                {
                    lsLigasInsertar.Add(new LigasUsuariosGridViewModel()
                    {
                        StrNombre = item["NOMBRE(S)"].ToString(),
                        StrApePaterno = item["APEPATERNO"].ToString(),
                        StrApeMaterno = item["APEMATERNO"].ToString(),
                        StrCorreo = item["CORREO"].ToString(),
                        StrTelefono = item["CELULAR"].ToString()
                    });
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) || !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) ||
                        !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) || !string.IsNullOrEmpty(item["CORREO"].ToString()) ||
                        !string.IsNullOrEmpty(item["CELULAR"].ToString()))
                    {
                        lsLigasErrores.Add(new LigasUsuariosGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString()
                        });
                    }
                }
            }
        }
        public void gvUsuariosSeleccionados(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel)
        {
            foreach (var item in lsLigasUsuariosGridViewModel)
            {
                if (!lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == true)
                {
                    lsgvUsuariosSeleccionados.Add(new LigasUsuariosGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        IdUsuario = item.IdUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = item.blSeleccionado,
                        IdCliente = item.IdCliente
                    });
                }
                else if (lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == false)
                {
                    lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.UidUsuario == item.UidUsuario));
                }
            }
        }
        #endregion

        #region Multiple       
        public void CargarUsuariosFinalesMultiples(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.CargarUsuariosFinalesMultiples(lsLigasUsuarios, UidCliente, UidTipoPerfil);
        }
        public void BuscarUsuarios(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Telefono)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.BuscarUsuarios(lsLigasUsuarios, UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, Telefono);
        }
        public void ExcelToListMultiple(DataTable dataTable, Guid UidCliente)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ExcelToListMultiple(dataTable, UidCliente);
        }
        public void ActualizarListaUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuariosMultiple(lsLigasUsuarios, IdUsuario, accion);
        }
        public void ActualizarListaGvUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaGvUsuariosMultiple(lsLigasUsuarios, IdUsuario, accion, Asunto, Concepto, Importe, Vencimiento);
        }
        #endregion

        public bool GenerarLigasPagos(string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagos(
               UidLigaUrl, VchUrl, VchConcepto, DcmImporte, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, VchAsunto
                ))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
