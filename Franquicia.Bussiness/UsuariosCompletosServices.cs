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

        private PrefijosTelefonicosRepository _prefijosTelefonicosRepository = new PrefijosTelefonicosRepository();
        public PrefijosTelefonicosRepository prefijosTelefonicosRepository
        {
            get { return _prefijosTelefonicosRepository; }
            set { _prefijosTelefonicosRepository = value; }
        }

        public List<UsuariosCompletos> lsUsuariosCompletos = new List<UsuariosCompletos>();

        public List<UsuariosCompletos> lsActualizarUsuarios = new List<UsuariosCompletos>();

        public List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsgvUsuariosSeleccionados = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsLigasInsertar = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsLigasErrores = new List<LigasUsuariosGridViewModel>();

        public List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel = new List<LigasMultiplesUsuariosGridViewModel>();
        public List<LigasMultiplesUsuariosGridViewModel> lsgvUsuariosSeleccionadosMultiple = new List<LigasMultiplesUsuariosGridViewModel>();
        public List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple = new List<LigasMultiplesUsuariosGridViewModel>();
        public List<LigasMultiplesUsuariosGridViewModel> lsLigasErroresMultiple = new List<LigasMultiplesUsuariosGridViewModel>();

        public List<LigasUsuariosGridViewModel> lsPagoLiga = new List<LigasUsuariosGridViewModel>();
        public List<LigasUsuariosGridViewModel> lsEventoLiga = new List<LigasUsuariosGridViewModel>();
        public List<EventoUsuarioGridViewModel> lsEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();
        public List<EventoUsuarioGridViewModel> lsSelectEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

        public List<LigasUsuariosGridViewModel> lsPagoColegiaturaLiga = new List<LigasUsuariosGridViewModel>();

        public void CargarAdministradores(Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarAdministradores(UidTipoPerfil);
        }
        public void BuscarAdministradores(Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Franquicia, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarAdministradores(UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, Franquicia, UidEstatus);
        }

        public void CargarUsuariosPrincipal(Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarUsuariosPrincipal(UidTipoPerfil);
        }
        public void BuscarUsuariosPrincipal(Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Perfil, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarUsuariosPrincipal(UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, Perfil, UidEstatus);
        }

        #region Metodos Principal
        public bool RegistrarAdministradores(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                UidSegPerfilEscuela,
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
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                UidSegPerfilEscuela,
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
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                UidSegPerfilEscuela,
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

        #region MetodosFranquiciaUsuarios
        public void CargarFranquiciasUsuariosFinales(Guid UidFranquicia, Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarFranquiciasUsuariosFinales(UidFranquicia, UidTipoPerfil);
        }
        public bool RegistrarFranquiciasUsuarios(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia, string IncluirDir)
        {
            bool result = false;
            if (usuariosCompletosRepository.RegistrarFranquiciasUsuarios(
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

        public bool ActualizarFranquiciasUsuarios(
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil,
            string Telefono, Guid UidTipoTelefono, Guid UidFranquicia)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActualizarFranquiciasUsuarios(
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

        public bool RegistrarFranquiciasDireccionUsuarios(Guid UidUsuario, string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia)
        {
            bool result = false;
            if (usuariosCompletosRepository.RegistrarFranquiciasDireccionUsuarios(
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

        public bool ActualizarFranquiciasDireccionUsuarios(Guid UidUsuario, string Identificador, Guid UidPais, Guid UidEstado, Guid Municipio, Guid UidCiudad, Guid UidColonia, string Calle, string EntreCalle, string YCalle, string NumeroExterior, string NumeroInterior, string CodigoPostal, string Referencia)
        {
            bool result = false;
            if (usuariosCompletosRepository.ActualizarFranquiciasDireccionUsuarios(
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
        public void BuscarFranquiciaUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarFranquiciaUsuariosFinales(UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, UidEstatus);
        }
        public bool AsociarUsuarioFranquicia(Guid UidFranquicia, Guid UidUsuario)
        {
            return usuariosCompletosRepository.AsociarUsuarioFranquicia(UidFranquicia, UidUsuario);
        }
        public void AsociarFranquiciaUsuariosFinales(string Correo)
        {
            usuariosCompletosRepository.AsociarFranquiciaUsuariosFinales(Correo);
        }

        #endregion

        #region Metodos Franquicia
        public void CargarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarAdministradoresFranquicia(UidFranquiciatario, UidTipoPerfilFranquicia);
        }
        public void BuscarAdministradoresFranquicia(Guid UidFranquiciatario, Guid UidTipoPerfilFranquicia, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Perfil, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarAdministradoresFranquicia(UidFranquiciatario, UidTipoPerfilFranquicia, Nombre, ApePaterno, ApeMaterno, Correo, Perfil, UidEstatus);
        }
        public bool RegistrarAdministradoresFranquicia(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                UidSegPerfilEscuela,
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
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                UidSegPerfilEscuela,
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
        public void BuscarAdministradoresCliente(Guid UidFranquiciatario, Guid UidTipoPerfil, string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Cliente, Guid UidEstatus)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarAdministradoresCliente(UidFranquiciatario, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, Cliente, UidEstatus);
        }
        public bool RegistrarAdministradoresCliente(
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, bool BitEscuela, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                BitEscuela,
                UidSegPerfilEscuela,
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
            Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Correo, Guid UidEstatus, string Usuario, string Password, bool BitEscuela, Guid UidSegPerfil, Guid UidSegPerfilEscuela,
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
                BitEscuela,
                UidSegPerfilEscuela,
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

        public void CargarAdminCliente(Guid UidFranquiciatario, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsActualizarUsuarios = usuariosCompletosRepository.CargarAdminCliente(UidFranquiciatario, UidCliente, UidTipoPerfil);
        }
        public bool ActualizarAdminClientePerfilEscu(Guid UidSegUsuario, bool BitEscuela, Guid UidSegPerfilEscuela)
        {
            bool result = false;
            if (usuariosCompletosRepository.ActualizarAdminClientePerfilEscu(UidSegUsuario, BitEscuela, UidSegPerfilEscuela))
            {
                result = true;
            }
            return result;
        }

        #region Pagos
        public void SeleccionarUsuariosCliente(Guid UidUsuario)
        {
            lsPagoLiga = usuariosCompletosRepository.SeleccionarUsuariosCliente(UidUsuario);
        }
        #endregion

        #region Eventos
        public void SelectUsClienteEvento(Guid UidUsuario)
        {
            lsEventoLiga = usuariosCompletosRepository.SelectUsClienteEvento(UidUsuario);
        }

        public void AsociarUsuariosEvento(Guid UidCliente, Guid UidTipoPerfil)
        {
            lsEventoUsuarioGridViewModel = usuariosCompletosRepository.AsociarUsuariosEvento(UidCliente, UidTipoPerfil);
        }
        public List<EventoUsuarioGridViewModel> ActualizarTodoListaEventoUsuarios(List<EventoUsuarioGridViewModel> lsEventoUsuarios, bool accion)
        {
            List<EventoUsuarioGridViewModel> lsNuevoEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

            foreach (var item in lsEventoUsuarios)
            {
                lsNuevoEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                {
                    UidUsuario = item.UidUsuario,
                    StrNombre = item.StrNombre,
                    StrApePaterno = item.StrApePaterno,
                    StrApeMaterno = item.StrApeMaterno,
                    StrCorreo = item.StrCorreo,
                    UidEstatus = item.UidEstatus,
                    StrTelefono = item.StrTelefono,
                    blSeleccionado = accion
                });
            }
            return lsEventoUsuarioGridViewModel = lsNuevoEventoUsuarioGridViewModel;
        }
        public List<EventoUsuarioGridViewModel> ActualizarListaEventoUsuarios(List<EventoUsuarioGridViewModel> lsEventoUsuarios, Guid UidUsuario, bool accion)
        {
            List<EventoUsuarioGridViewModel> lsNuevoEventoUsuarioGridViewModel = new List<EventoUsuarioGridViewModel>();

            foreach (var item in lsEventoUsuarios)
            {
                if (item.UidUsuario == UidUsuario)
                {
                    lsNuevoEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = accion
                    });

                    if (accion)
                    {
                        lsSelectEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                        {
                            UidUsuario = item.UidUsuario,
                            StrNombre = item.StrNombre,
                            StrApePaterno = item.StrApePaterno,
                            StrApeMaterno = item.StrApeMaterno,
                            StrCorreo = item.StrCorreo,
                            UidEstatus = item.UidEstatus,
                            StrTelefono = item.StrTelefono,
                            blSeleccionado = accion
                        });
                    }
                    else
                    {
                        lsSelectEventoUsuarioGridViewModel.RemoveAt(lsSelectEventoUsuarioGridViewModel.FindIndex(x => x.UidUsuario == UidUsuario));
                    }
                }
                else
                {
                    lsNuevoEventoUsuarioGridViewModel.Add(new EventoUsuarioGridViewModel()
                    {
                        UidUsuario = item.UidUsuario,
                        StrNombre = item.StrNombre,
                        StrApePaterno = item.StrApePaterno,
                        StrApeMaterno = item.StrApeMaterno,
                        StrCorreo = item.StrCorreo,
                        UidEstatus = item.UidEstatus,
                        StrTelefono = item.StrTelefono,
                        blSeleccionado = item.blSeleccionado
                    });
                }
            }

            return lsEventoUsuarioGridViewModel = lsNuevoEventoUsuarioGridViewModel;
        }
        public void BuscarUsuariosEvento(List<EventoUsuarioGridViewModel> lsEventoUsuarios, Guid UidCliente, string Nombre, string ApePaterno, string ApeMaterno, string Correo)
        {
            lsEventoUsuarioGridViewModel = usuariosCompletosRepository.BuscarUsuariosEvento(lsEventoUsuarios, UidCliente, Nombre, ApePaterno, ApeMaterno, Correo);
        }
        public void ObtenerUsuariosEvento(Guid UidEvento)
        {
            lsEventoUsuarioGridViewModel = usuariosCompletosRepository.ObtenerUsuariosEvento(UidEvento);
            lsSelectEventoUsuarioGridViewModel = lsEventoUsuarioGridViewModel;
        }
        #endregion


        #endregion

        #region MetodosUsuarios
        public void CargarUsuariosFinales(Guid UidCliente, Guid UidTipoPerfil)
        {
            lsUsuariosCompletos = usuariosCompletosRepository.CargarUsuariosFinales(UidCliente, UidTipoPerfil);
        }
        public bool RegistrarUsuarios(Guid UidUsuario,
            string Nombre, string ApePaterno, string ApeMaterno, string Correo, string Usuario, string Password, Guid UidSegPerfil,
            string Telefono, Guid UidTipoTelefono, Guid UidPrefijo, Guid UidCliente)
        {
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
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono,
                    UidPrefijo = UidPrefijo
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
            string Telefono, Guid UidTipoTelefono, Guid UidPrefijo, Guid UidCliente)
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
                new TelefonosUsuarios
                {
                    VchTelefono = Telefono,
                    UidTipoTelefono = UidTipoTelefono,
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
            if (usuariosCompletosRepository.RegistrarDireccionUsuarios(
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
            if (usuariosCompletosRepository.ActualizarDireccionUsuarios(
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
            lsUsuariosCompletos = usuariosCompletosRepository.BuscarUsuariosFinales(UidCliente, UidTipoPerfil, Nombre, ApePaterno, ApeMaterno, Correo, UidEstatus);
        }
        public bool AsociarClienteUsuario(Guid UidCliente, Guid UidUsuario)
        {
            return usuariosCompletosRepository.AsociarUsuarioCliente(UidCliente, UidUsuario);
        }
        public void AsociarUsuariosFinales(string Correo)
        {
            usuariosCompletosRepository.AsociarUsuariosFinales(Correo);
        }
        #endregion

        #region Metodos Usuarios final
        public void SelectUsClienteEventoUsuarioFinal(Guid UidUsuario)
        {
            lsEventoLiga = usuariosCompletosRepository.SelectUsClienteEventoUsuarioFinal(UidUsuario);
        }
        #endregion  

        #region MetodosExel
        #region Clientes
        #region Simple
        public void CargarUsuariosFinales(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.CargarUsuariosFinales(lsLigasUsuarios, UidCliente, UidTipoPerfil);
        }
        public void ActualizarListaUsuarios(List<LigasUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuarios(lsLigasUsuarios, IdUsuario, accion);
        }
        public void EliminarItemgvUsuariosSeleccionados(int IdUsuario, int Index)
        {
            //lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.IdUsuario == IdUsuario));
            lsgvUsuariosSeleccionados.RemoveAt(Index);
        }
        public void ExcelToList(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidCliente, string URLBase)
        {
            lsgvUsuariosSeleccionados = usuariosCompletosRepository.ExcelToList(lsLigasUsuariosGridView, lsLigasInsertar, UidCliente, URLBase);
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
                    bool error = false;

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
                                    lsLigasInsertar.Add(new LigasUsuariosGridViewModel()
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
                //if (!lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == true)
                if (item.blSeleccionado == true)
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
                        IdCliente = item.IdCliente,
                        VchNombreComercial = item.VchNombreComercial
                    });
                }
                //else if (lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == false)
                //{
                //    lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.UidUsuario == item.UidUsuario));
                //}
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
        public void ExcelToListMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple, Guid UidCliente, string URLBase)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ExcelToListMultiple(lsLigasMultiplesUsuariosGridView, lsLigasInsertarMultiple, UidCliente, URLBase);
        }
        public void ActualizarListaUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuariosMultiple(lsLigasUsuarios, IdUsuario, accion);
        }
        public void ActualizarListaGvUsuariosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string Promociones, bool CBCorreo, bool CBSms, bool CBWhatsApp, int Index)
        {
            lsgvUsuariosSeleccionadosMultiple = usuariosCompletosRepository.ActualizarListaGvUsuariosMultiple(lsLigasUsuarios, IdUsuario, accion, Asunto, Concepto, Importe, Vencimiento, Promociones, CBCorreo, CBSms, CBWhatsApp, Index);
        }


        public void ValidarExcelToListMultiple(DataTable dataTable, List<CBLPromocionesModel> lsCBLPromocionesModelCliente)
        {
            lsLigasErroresMultiple.Clear();
            lsLigasInsertarMultiple.Clear();

            bool PromocionCorrecto = false;

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) &&
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) && !string.IsNullOrEmpty(item["CORREO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()) && !string.IsNullOrEmpty(item["ASUNTO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CONCEPTO"].ToString()) && !string.IsNullOrEmpty(item["IMPORTE"].ToString()) &&
                    !string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()) && !string.IsNullOrEmpty(item["EMAIL"].ToString()) &&
                    !string.IsNullOrEmpty(item["WHATS"].ToString()) && !string.IsNullOrEmpty(item["SMS"].ToString()))
                {
                    bool EMAIL = false;
                    bool WHATS = false;
                    bool SMS = false;
                    bool error = false;

                    if (item["EMAIL"].ToString().ToUpper() != "SI" && item["EMAIL"].ToString().ToUpper() != "NO" ||
                        item["WHATS"].ToString().ToUpper() != "SI" && item["WHATS"].ToString().ToUpper() != "NO" ||
                        item["SMS"].ToString().ToUpper() != "SI" && item["SMS"].ToString().ToUpper() != "NO")
                    {
                        EMAIL = true;
                        WHATS = true;
                        SMS = true;
                        error = true;
                    }
                    else
                    {
                        if (item["EMAIL"].ToString().ToUpper() == "SI")
                        {
                            EMAIL = true;
                        }
                        else if (item["EMAIL"].ToString().ToUpper() == "NO")
                        {
                            EMAIL = false;
                        }

                        if (item["WHATS"].ToString().ToUpper() == "SI")
                        {
                            WHATS = true;
                        }
                        else if (item["WHATS"].ToString().ToUpper() == "NO")
                        {
                            WHATS = false;
                        }

                        if (item["SMS"].ToString().ToUpper() == "SI")
                        {
                            SMS = true;
                        }
                        else if (item["SMS"].ToString().ToUpper() == "NO")
                        {
                            SMS = false;
                        }

                        string regexCorreo = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                        Regex reCorreo = new Regex(regexCorreo);

                        if (reCorreo.IsMatch(item["CORREO"].ToString()))
                        {
                            if (item["CELULAR"].ToString().Contains("(") && item["CELULAR"].ToString().Contains("+") && item["CELULAR"].ToString().Contains(")"))
                            {
                                string output = item["CELULAR"].ToString().Split('(', ')')[1];
                                prefijosTelefonicosRepository.ValidarPrefijoTelefonico(output);
                                string promociones = string.Empty;
                                string pro = string.Empty;

                                if (item["CELULAR"].ToString().Split('(', ')')[2].Count() == 10)
                                {
                                    if (prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != null && prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo != Guid.Empty)
                                    {
                                        decimal validar = 0;
                                        if (decimal.TryParse(item["IMPORTE"].ToString(), out validar))
                                        {
                                            string MontoMin = "50.00";
                                            string MontoMax = "15000.00";

                                            if (decimal.Parse(item["IMPORTE"].ToString()) >= decimal.Parse(MontoMin) && decimal.Parse(item["IMPORTE"].ToString()) <= decimal.Parse(MontoMax))
                                            {
                                                DateTime date = DateTime.Parse(item["VENCIMIENTO"].ToString());
                                                DateTime hoy = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                                                DateTime hoyMas = DateTime.Parse(DateTime.Now.AddDays(89).ToString("dd/MM/yyyy"));
                                                DateTime date2 = DateTime.Parse(item["VENCIMIENTO"].ToString());

                                                if (date >= hoy && date2 <= hoyMas)
                                                {
                                                    DateTime fecha;
                                                    if (DateTime.TryParse(item["VENCIMIENTO"].ToString(), out fecha))
                                                    {
                                                        if (!string.IsNullOrEmpty(item["PROMOCION(ES)"].ToString()))
                                                        {
                                                            promociones = item["PROMOCION(ES)"].ToString();

                                                            promociones = promociones.Trim().Replace(",", " MESES,");

                                                            promociones = promociones.Trim() + " MESES";

                                                            //string strRegex = @"^[0-9]+([,]*\s()[0-9]+)+([,]*\s()[0-9]+)?$";
                                                            string strRegex = @"^[0-9]+([,]()[0-9]+)+([,]()[0-9]+)?$";
                                                            Regex re = new Regex(strRegex);

                                                            string strRegexOnly = @"^[0-9]?$";
                                                            Regex reOnly = new Regex(strRegexOnly);

                                                            if (re.IsMatch(item["PROMOCION(ES)"].ToString().Trim()) || reOnly.IsMatch(item["PROMOCION(ES)"].ToString().Trim()))
                                                            {
                                                                string[] arPromo = Regex.Split(promociones, ",");

                                                                for (int i = 0; i < arPromo.Length; i++)
                                                                {
                                                                    if (!lsCBLPromocionesModelCliente.Exists(x => x.VchDescripcion == arPromo[i].Trim()))
                                                                    {

                                                                    }
                                                                    else
                                                                    {
                                                                        pro += arPromo[i] + ",";
                                                                        PromocionCorrecto = true;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                DateTime dateTime = DateTime.Now;
                                                                string Remplazo = dateTime.ToString("dd/MM/yyyy");
                                                                lsLigasErroresMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                                                                {
                                                                    StrNombre = item["NOMBRE(S)"].ToString(),
                                                                    StrApePaterno = item["APEPATERNO"].ToString(),
                                                                    StrApeMaterno = item["APEMATERNO"].ToString(),
                                                                    StrCorreo = item["CORREO"].ToString(),
                                                                    StrTelefono = item["CELULAR"].ToString(),
                                                                    StrAsunto = item["ASUNTO"].ToString(),
                                                                    StrConcepto = item["CONCEPTO"].ToString(),
                                                                    DcmImporte = item.IsNull("IMPORTE") ? 0 : decimal.Parse(item["IMPORTE"].ToString()),
                                                                    DtVencimiento = item.IsNull("VENCIMIENTO") ? DateTime.Parse(Remplazo) : DateTime.Parse(item["VENCIMIENTO"].ToString()),
                                                                    CBCorreo = EMAIL,
                                                                    CBWhatsApp = WHATS,
                                                                    CBSms = SMS,
                                                                    StrPromociones = item["PROMOCION(ES)"].ToString()
                                                                });
                                                                PromocionCorrecto = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            PromocionCorrecto = true;
                                                        }

                                                        if (PromocionCorrecto)
                                                        {
                                                            lsLigasInsertarMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                                                            {
                                                                StrNombre = item["NOMBRE(S)"].ToString(),
                                                                StrApePaterno = item["APEPATERNO"].ToString(),
                                                                StrApeMaterno = item["APEMATERNO"].ToString(),
                                                                StrCorreo = item["CORREO"].ToString(),
                                                                UidPrefijo = prefijosTelefonicosRepository.prefijosTelefonicos.UidPrefijo,
                                                                StrTelefono = item["CELULAR"].ToString().Split('(', ')')[2],
                                                                StrAsunto = item["ASUNTO"].ToString(),
                                                                StrConcepto = item["CONCEPTO"].ToString(),
                                                                DcmImporte = decimal.Parse(item["IMPORTE"].ToString()),
                                                                DtVencimiento = DateTime.Parse(item["VENCIMIENTO"].ToString()),
                                                                CBCorreo = EMAIL,
                                                                CBWhatsApp = WHATS,
                                                                CBSms = SMS,
                                                                StrPromociones = pro
                                                            });
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
                        else
                        {
                            error = true;
                        }
                    }

                    if (error)
                    {
                        DateTime Remplazo = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                        decimal Importe = 0;

                        if (!string.IsNullOrEmpty(item["IMPORTE"].ToString()))
                        {
                            decimal validar = 0;
                            string numString = item["IMPORTE"].ToString();
                            bool canConvert = decimal.TryParse(numString, out validar);

                            if (canConvert)
                            {
                                Importe = decimal.Parse(item["IMPORTE"].ToString());
                            }
                        }

                        if (!string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()))
                        {
                            DateTime fecha;

                            if (DateTime.TryParse(item["VENCIMIENTO"].ToString(), out fecha))
                            {
                                Remplazo = fecha;
                            }
                        }

                        lsLigasErroresMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            StrAsunto = item["ASUNTO"].ToString(),
                            StrConcepto = item["CONCEPTO"].ToString(),
                            DcmImporte = Importe,
                            DtVencimiento = Remplazo,
                            CBCorreo = EMAIL,
                            CBWhatsApp = WHATS,
                            CBSms = SMS,
                            StrPromociones = item["PROMOCION(ES)"].ToString()
                        });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) || !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) ||
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) || !string.IsNullOrEmpty(item["CORREO"].ToString()) ||
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()) || !string.IsNullOrEmpty(item["ASUNTO"].ToString()) ||
                    !string.IsNullOrEmpty(item["CONCEPTO"].ToString()) || !string.IsNullOrEmpty(item["IMPORTE"].ToString()) ||
                    !string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()) && !string.IsNullOrEmpty(item["EMAIL"].ToString()) &&
                    !string.IsNullOrEmpty(item["WHATS"].ToString()) && !string.IsNullOrEmpty(item["SMS"].ToString()))
                    {
                        DateTime Remplazo = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));

                        bool EMAIL = false;
                        bool WHATS = false;
                        bool SMS = false;
                        decimal Importe = 0;

                        if (item["EMAIL"].ToString() == "SI")
                        {
                            EMAIL = true;
                        }
                        else if (item["EMAIL"].ToString() == "NO")
                        {
                            EMAIL = false;
                        }

                        if (item["WHATS"].ToString() == "SI")
                        {
                            WHATS = true;
                        }
                        else if (item["WHATS"].ToString() == "NO")
                        {
                            WHATS = false;
                        }

                        if (item["SMS"].ToString() == "SI")
                        {
                            SMS = true;
                        }
                        else if (item["SMS"].ToString() == "NO")
                        {
                            SMS = false;
                        }

                        if (!string.IsNullOrEmpty(item["IMPORTE"].ToString()))
                        {
                            decimal validar = 0;
                            string numString = item["IMPORTE"].ToString();
                            bool canConvert = decimal.TryParse(numString, out validar);

                            if (canConvert)
                            {
                                Importe = decimal.Parse(item["IMPORTE"].ToString());
                            }
                        }

                        if (!string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()))
                        {
                            DateTime fecha;

                            if (DateTime.TryParse(item["VENCIMIENTO"].ToString(), out fecha))
                            {
                                Remplazo = fecha;
                            }
                        }

                        lsLigasErroresMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            StrAsunto = item["ASUNTO"].ToString(),
                            StrConcepto = item["CONCEPTO"].ToString(),
                            DcmImporte = Importe,
                            DtVencimiento = Remplazo,
                            CBCorreo = EMAIL,
                            CBWhatsApp = WHATS,
                            CBSms = SMS,
                            StrPromociones = item["PROMOCION(ES)"].ToString()
                        });
                    }
                }
            }
        }

        public void gvUsuariosSeleccionadosMultiple(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel)
        {
            Random random = new Random();

            foreach (var item in lsLigasMultiplesUsuariosGridViewModel)
            {
                DateTime dtAuxiliar = DateTime.Now.AddSeconds(3).AddMilliseconds(5);

                //if (!lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == true)
                if (item.blSeleccionado == true)
                {
                    lsgvUsuariosSeleccionadosMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
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
                        IdCliente = item.IdCliente,
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = 50,
                        DtVencimiento = item.DtVencimiento,
                        CBCorreo = item.CBCorreo,
                        CBSms = item.CBSms,
                        CBWhatsApp = item.CBWhatsApp,
                        IntAuxiliar = dtAuxiliar.ToString("ssfff") + random.Next(10000000, 100000001).ToString()
                    });
                }
                //else if (lsgvUsuariosSeleccionadosMultiple.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == false)
                //{
                //    lsgvUsuariosSeleccionadosMultiple.RemoveAt(lsgvUsuariosSeleccionadosMultiple.FindIndex(x => x.UidUsuario == item.UidUsuario));
                //}
            }
        }
        public void EliminarItemgvUsuariosSeleccionadosMultiple(int IdUsuario, int Index)
        {
            //lsgvUsuariosSeleccionadosMultiple.RemoveAt(lsgvUsuariosSeleccionadosMultiple.FindIndex(x => x.IdUsuario == IdUsuario));
            lsgvUsuariosSeleccionadosMultiple.RemoveAt(Index);
        }
        #endregion
        #endregion

        #region Franquicias
        #region Simple
        public void CargarUsuariosFinalesFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.CargarUsuariosFinalesFranquicias(lsLigasUsuarios, UidCliente, UidTipoPerfil);
        }
        public void ActualizarListaUsuariosFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuariosFranquicias(lsLigasUsuarios, IdUsuario, accion);
        }
        public void ValidarExcelToListFranquicias(DataTable dataTable)
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
        public void ExcelToListFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridView, List<LigasUsuariosGridViewModel> lsLigasInsertar, Guid UidFranquicia)
        {
            lsgvUsuariosSeleccionados = usuariosCompletosRepository.ExcelToListFranquicias(lsLigasUsuariosGridView, lsLigasInsertar, UidFranquicia);
        }
        public void gvUsuariosSeleccionadosFranquicias(List<LigasUsuariosGridViewModel> lsLigasUsuariosGridViewModel)
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial
                    });
                }
                else if (lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == false)
                {
                    lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.UidUsuario == item.UidUsuario));
                }
            }
        }
        public void EliminarItemgvUsuariosSeleccionadosFranquicia(int IdUsuario)
        {
            lsgvUsuariosSeleccionados.RemoveAt(lsgvUsuariosSeleccionados.FindIndex(x => x.IdUsuario == IdUsuario));
            //lsgvUsuariosSeleccionados.RemoveAt(Index);
        }
        #endregion
        #region Multiple
        public void CargarUsuariosFinalesMultiplesFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, Guid UidCliente, Guid UidTipoPerfil)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.CargarUsuariosFinalesMultiplesFranquicias(lsLigasUsuarios, UidCliente, UidTipoPerfil);
        }
        public void ActualizarListaUsuariosMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ActualizarListaUsuariosMultipleFranquicias(lsLigasUsuarios, IdUsuario, accion);
        }
        public void ActualizarListaGvUsuariosMultipleFranquicias(List<LigasMultiplesUsuariosGridViewModel> lsLigasUsuarios, int IdUsuario, bool accion, string Asunto, string Concepto, decimal Importe, DateTime Vencimiento, string Promociones)
        {
            lsgvUsuariosSeleccionadosMultiple = usuariosCompletosRepository.ActualizarListaGvUsuariosMultipleFranquicias(lsLigasUsuarios, IdUsuario, accion, Asunto, Concepto, Importe, Vencimiento, Promociones);
        }
        public void EliminarItemgvUsuariosSeleccionadosMultipleFranquicias(int IdUsuario)
        {
            lsgvUsuariosSeleccionadosMultiple.RemoveAt(lsgvUsuariosSeleccionadosMultiple.FindIndex(x => x.IdUsuario == IdUsuario));
        }
        public void ValidarExcelToListMultipleFranquicias(DataTable dataTable, List<FranquiciasCBLPromocionesModel> lsCBLPromocionesModelCliente)
        {
            lsLigasErroresMultiple.Clear();
            lsLigasInsertarMultiple.Clear();

            bool PromocionCorrecto = false;

            foreach (DataRow item in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) && !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) &&
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) && !string.IsNullOrEmpty(item["CORREO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()) && !string.IsNullOrEmpty(item["ASUNTO"].ToString()) &&
                    !string.IsNullOrEmpty(item["CONCEPTO"].ToString()) && !string.IsNullOrEmpty(item["IMPORTE"].ToString()) &&
                    !string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()))
                {
                    if (!string.IsNullOrEmpty(item["PROMOCION(ES)"].ToString()))
                    {
                        string[] arPromo = Regex.Split(item["PROMOCION(ES)"].ToString(), ",");

                        for (int i = 0; i < arPromo.Length; i++)
                        {
                            if (!lsCBLPromocionesModelCliente.Exists(x => x.VchDescripcion == arPromo[i].Trim()))
                            {
                                DateTime dateTime = DateTime.Now;
                                string Remplazo = dateTime.ToString("dd/MM/yyyy");
                                lsLigasErroresMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                                {
                                    StrNombre = item["NOMBRE(S)"].ToString(),
                                    StrApePaterno = item["APEPATERNO"].ToString(),
                                    StrApeMaterno = item["APEMATERNO"].ToString(),
                                    StrCorreo = item["CORREO"].ToString(),
                                    StrTelefono = item["CELULAR"].ToString(),
                                    StrAsunto = item["ASUNTO"].ToString(),
                                    StrConcepto = item["CONCEPTO"].ToString(),
                                    DcmImporte = item.IsNull("IMPORTE") ? 0 : decimal.Parse(item["IMPORTE"].ToString()),
                                    DtVencimiento = item.IsNull("VENCIMIENTO") ? DateTime.Parse(Remplazo) : DateTime.Parse(item["VENCIMIENTO"].ToString()),
                                    StrPromociones = item["PROMOCION(ES)"].ToString()
                                });
                                PromocionCorrecto = false;
                                break;
                            }
                            else
                            {
                                PromocionCorrecto = true;
                            }
                        }
                    }
                    else
                    {
                        PromocionCorrecto = true;
                    }

                    if (PromocionCorrecto)
                    {
                        lsLigasInsertarMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            StrAsunto = item["ASUNTO"].ToString(),
                            StrConcepto = item["CONCEPTO"].ToString(),
                            DcmImporte = decimal.Parse(item["IMPORTE"].ToString()),
                            DtVencimiento = DateTime.Parse(item["VENCIMIENTO"].ToString()),
                            StrPromociones = item["PROMOCION(ES)"].ToString()
                        });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(item["NOMBRE(S)"].ToString()) || !string.IsNullOrEmpty(item["APEPATERNO"].ToString()) ||
                    !string.IsNullOrEmpty(item["APEMATERNO"].ToString()) || !string.IsNullOrEmpty(item["CORREO"].ToString()) ||
                    !string.IsNullOrEmpty(item["CELULAR"].ToString()) || !string.IsNullOrEmpty(item["ASUNTO"].ToString()) ||
                    !string.IsNullOrEmpty(item["CONCEPTO"].ToString()) || !string.IsNullOrEmpty(item["IMPORTE"].ToString()) ||
                    !string.IsNullOrEmpty(item["VENCIMIENTO"].ToString()))
                    {
                        DateTime dateTime = DateTime.Now;
                        string Remplazo = dateTime.ToString("dd/MM/yyyy");
                        lsLigasErroresMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
                        {
                            StrNombre = item["NOMBRE(S)"].ToString(),
                            StrApePaterno = item["APEPATERNO"].ToString(),
                            StrApeMaterno = item["APEMATERNO"].ToString(),
                            StrCorreo = item["CORREO"].ToString(),
                            StrTelefono = item["CELULAR"].ToString(),
                            StrAsunto = item["ASUNTO"].ToString(),
                            StrConcepto = item["CONCEPTO"].ToString(),
                            DcmImporte = item.IsNull("IMPORTE") ? 0 : decimal.Parse(item["IMPORTE"].ToString()),
                            DtVencimiento = item.IsNull("VENCIMIENTO") ? DateTime.Parse(Remplazo) : DateTime.Parse(item["VENCIMIENTO"].ToString()),
                            StrPromociones = item["PROMOCION(ES)"].ToString()
                        });
                    }
                }
            }
        }
        public void ExcelToListMultipleFranquicas(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridView, List<LigasMultiplesUsuariosGridViewModel> lsLigasInsertarMultiple, Guid UidFranquicia)
        {
            lsLigasMultiplesUsuariosGridViewModel = usuariosCompletosRepository.ExcelToListMultipleFranquicias(lsLigasMultiplesUsuariosGridView, lsLigasInsertarMultiple, UidFranquicia);
        }
        public void gvUsuariosSeleccionadosMultipleFranquicia(List<LigasMultiplesUsuariosGridViewModel> lsLigasMultiplesUsuariosGridViewModel)
        {
            foreach (var item in lsLigasMultiplesUsuariosGridViewModel)
            {
                if (!lsgvUsuariosSeleccionados.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == true)
                {
                    lsgvUsuariosSeleccionadosMultiple.Add(new LigasMultiplesUsuariosGridViewModel()
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
                        IdFranquicia = item.IdFranquicia,
                        VchNombreComercial = item.VchNombreComercial,
                        StrAsunto = item.StrAsunto,
                        StrConcepto = item.StrConcepto,
                        DcmImporte = item.DcmImporte,
                        DtVencimiento = item.DtVencimiento
                    });
                }
                else if (lsgvUsuariosSeleccionadosMultiple.Exists(x => x.UidUsuario == item.UidUsuario) && item.blSeleccionado == false)
                {
                    lsgvUsuariosSeleccionadosMultiple.RemoveAt(lsgvUsuariosSeleccionadosMultiple.FindIndex(x => x.UidUsuario == item.UidUsuario));
                }
            }
        }
        #endregion
        #endregion

        public bool GenerarLigasPagos(string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidPropietario)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagos(
               UidLigaUrl, VchUrl, VchConcepto, DcmImporte, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, VchAsunto, UidLigaAsociado, UidPromocion, UidPropietario
                ))
            {
                result = true;
            }
            return result;
        }
        public bool GenerarLigasPagosEvento(string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidEvento, Guid UidPropietario)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagosEvento(
               UidLigaUrl, VchUrl, VchConcepto, DcmImporte, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, VchAsunto, UidLigaAsociado, UidPromocion, UidEvento, UidPropietario
                ))
            {
                result = true;
            }
            return result;
        }

        public bool GenerarLigasPagosTemp(Guid UidLigaUrl, string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidPropietario)
        {
            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagos(
               UidLigaUrl, VchUrl, VchConcepto, DcmImporte, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, VchAsunto, UidLigaAsociado, UidPromocion, UidPropietario
                ))
            {
                result = true;
            }
            return result;
        }

        #region Metodos Escuela
        
        #region Pagos Padres
        public void SelectUsuCliColegiatura(Guid UidCliente, Guid UidUsuario)
        {
            lsPagoColegiaturaLiga = usuariosCompletosRepository.SelectUsuCliColegiatura(UidCliente, UidUsuario);
        }
        public bool GenerarLigasPagosColegiatura(string VchUrl, string VchConcepto, decimal DcmImporte, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, string VchAsunto, Guid UidLigaAsociado, Guid UidPromocion, Guid UidFechaColegiatura, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagosColegiatura(
               UidLigaUrl, VchUrl, VchConcepto, DcmImporte, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, VchAsunto, UidLigaAsociado, UidPromocion, UidFechaColegiatura, UidPagoColegiatura, UidPropietario
                ))
            {
                result = true;
            }
            return result;
        }

        public bool GenerarLigasPagosColegiaturaPraga(string VchUrl, string VchConcepto, string IdReferencia, Guid UidUsuario, string VchIdentificador, DateTime DtRegistro, DateTime DtVencimiento, decimal DcmImporte, string VchAsunto, Guid UidLigaAsociado, Guid UidTipoTarjeta, Guid UidPromocion, Guid UidFechaColegiatura, Guid UidPagoColegiatura, Guid UidPropietario)
        {
            Guid UidLigaUrl = Guid.NewGuid();

            bool result = false;
            if (usuariosCompletosRepository.GenerarLigasPagosColegiaturaPraga(
               UidLigaUrl, VchUrl, VchConcepto, IdReferencia, UidUsuario, VchIdentificador, DtRegistro, DtVencimiento, DcmImporte, VchAsunto, UidLigaAsociado, UidTipoTarjeta, UidPromocion, UidFechaColegiatura, UidPagoColegiatura, UidPropietario
                ))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #endregion

        #endregion

        #region Metodos MasterPage
        public List<UsuarioActivarCuentaViewModel> ObtenerDatossUsuario(Guid UidUsuario)
        {
            return usuariosCompletosRepository.ObtenerDatossUsuario(UidUsuario);
        }
        public bool ActivarCuentaUsuario(Guid UidUsuario, string Nombre, string ApePaterno, string ApeMaterno, string Password, string Telefono, Guid UidPrefijo)
        {

            bool result = false;
            if (usuariosCompletosRepository.ActivarCuentaUsuario(
                new UsuariosCompletos
                {
                    UidUsuario = UidUsuario,
                    StrNombre = Nombre,
                    StrApePaterno = ApePaterno,
                    StrApeMaterno = ApeMaterno,
                    VchContrasenia = Password
                },
                new TelefonosUsuarios
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
        #endregion
    }
}
