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

        public List<Padres> lsPadres = new List<Padres>();
        
        public List<Padres> lsActualizarPadres = new List<Padres>();
                
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
            if (padresRepository.RegistrarUsuarios(
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

    }
}
