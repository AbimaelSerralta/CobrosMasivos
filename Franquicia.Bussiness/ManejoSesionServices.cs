using Franquicia.Domain.Models;
using Franquicia.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Franquicia.Domain.ViewModels;

namespace Franquicia.Bussiness
{
    public class ManejoSesionServices
    {
        AccesosRepository _accesosRepository = new AccesosRepository();
        public AccesosRepository accesosRepository
        {
            get { return _accesosRepository; }
            set { _accesosRepository = value; }
        }
        
        ModulosRepository _modulosRepository = new ModulosRepository();
        public ModulosRepository modulosRepository
        {
            get { return _modulosRepository; }
            set { _modulosRepository = value; }
        }

        UsuariosCompletosRepository _usuarioCompletoRepository = new UsuariosCompletosRepository();
        public UsuariosCompletosRepository usuarioCompletoRepository
        {
            get { return _usuarioCompletoRepository; }
            set { _usuarioCompletoRepository = value; }
        }

        PerfilesRepository _perfilesRepository = new PerfilesRepository();
        public PerfilesRepository perfilesRepository
        {
            get { return _perfilesRepository; }
            set { _perfilesRepository = value; }
        }

        private ClienteCuentaRepository _clienteCuentaRepository = new ClienteCuentaRepository();
        public ClienteCuentaRepository clienteCuentaRepository
        {
            get { return _clienteCuentaRepository; }
            set { _clienteCuentaRepository = value; }
        }

        private ClientesRepository _clientesRepository = new ClientesRepository();
        public ClientesRepository clientesRepository
        {
            get { return _clientesRepository; }
            set { _clientesRepository = value; }
        }

        private bool _BolStatusSesion;
        public bool BolStatusSesion
        {
            get { return _BolStatusSesion; }
            set { _BolStatusSesion = value; }
        }

        public List<Usuarios> ListaUsarioCompleto = new List<Usuarios>();

        public List<PermisosMenuModel> lsmodulos = new List<PermisosMenuModel>();
        public List<PermisosMenuModel> lsAccesosPermitidos = new List<PermisosMenuModel>();

        public List<UsuariosCompletos> lsRecoveryPassword = new List<UsuariosCompletos>();

        #region Metodos
        public void IniciarSesion(string Usuario, string Password)
        {
            BolStatusSesion = false;

            //UsuarioCompleto usuarios = new UsuarioCompleto()
            //{
            //    VchUsuario = Usuario,
            //    VchContrasenia = Password
            //};

            usuarioCompletoRepository.LoginUsuario(Usuario, Password);

            if (usuarioCompletoRepository.usuarioCompleto.UidAppWeb == new Guid("514433C7-4439-42F5-ABE4-6BF1C330F0CA"))
            {
                if (usuarioCompletoRepository.ObtenerDatosUsuario(usuarioCompletoRepository.usuarioCompleto.UidUsuario))
                {
                    BolStatusSesion = true;
                }
            }
            else
            {
                if (usuarioCompletoRepository.usuarioCompleto.UidUsuario != Guid.Empty)
                {
                    usuarioCompletoRepository.ObtenerEstatusdeEmpresaUsuario(usuarioCompletoRepository.usuarioCompleto.UidUsuario);

                    if (usuarioCompletoRepository.usuarioCompleto.UidUsuario != Guid.Empty)
                    {
                        if (usuarioCompletoRepository.usuarioCompleto.UidEstatusEmpresa.ToString() == "65e46bc9-1864-4145-ad1a-70f5b5f69739")
                        {
                            if (usuarioCompletoRepository.usuarioCompleto.UidEstatus.ToString() == "65e46bc9-1864-4145-ad1a-70f5b5f69739")
                            {
                                BolStatusSesion = true;
                            }
                            else
                            {
                                BolStatusSesion = false;
                            }
                        }
                        else
                        {
                            BolStatusSesion = false;
                        }
                    }
                    else
                    {
                        //limpiarPropiedadesUsuarioEmpresas();
                        BolStatusSesion = true;

                    }
                }
                else
                {
                    //limpiarPropiedadesUsuarioEmpresas();
                    BolStatusSesion = false;
                }
            }
        }

        public String ObtenerHome()
        {
            string Home = string.Empty;

            if (usuarioCompletoRepository.usuarioCompleto.UidUltimoModulo != Guid.Empty)
            {
                modulosRepository.ObtenerModulo(usuarioCompletoRepository.usuarioCompleto.UidUltimoModulo);
                Home = modulosRepository.modulos.VchUrl;
            }

            else if (usuarioCompletoRepository.usuarioCompleto.UidModuloInicial != Guid.Empty)
            {
                modulosRepository.ObtenerModulo(usuarioCompletoRepository.usuarioCompleto.UidModuloInicial);
                Home = modulosRepository.modulos.VchUrl;
            }
            else if (usuarioCompletoRepository.usuarioCompleto.UidSegPerfil != Guid.Empty)
            {
                perfilesRepository.ObtenerPerfil(usuarioCompletoRepository.usuarioCompleto.UidSegPerfil);
                modulosRepository.ObtenerModulo(perfilesRepository.perfiles.UidModuloInicial);
                Home = modulosRepository.modulos.VchUrl;
            }
            else if (perfilesRepository.perfiles.UidAppWeb != Guid.Empty)
            {
                perfilesRepository.ObtenerPerfil(usuarioCompletoRepository.usuarioCompleto.UidSegPerfil);
                perfilesRepository.appWebRepository.ObtenerAppWeb(perfilesRepository.perfiles.UidAppWeb);
                if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 1)
                {
                    Home = "MenuBackSite.aspx";
                }
                else if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 2)
                {
                    Home = "MenuBackEnd.aspx";
                }
                else if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 3)
                {
                    Home = "MenuFrontEnd.aspx";
                }

            }
            else
            {
                Home = "MenuBackSite.aspx";

            }

            return Home;
        }

        public bool ValidarAccesoAModulo(string vchUrl)
        {
            bool resultado = false;
            try
            {
                if (usuarioCompletoRepository.usuarioCompleto.UidSegPerfil != Guid.Empty)
                {
                    if (accesosRepository.validarModuloPerfil(usuarioCompletoRepository.usuarioCompleto.UidSegPerfil, vchUrl) == true)
                    {
                        if (usuarioCompletoRepository.usuarioCompleto.UidSegPerfil != Guid.Empty)
                        {
                            //LimpiarPropiedadesModeloAcceso();
                            return resultado = true;

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return resultado;
        }

        public string ObtenerAppWeb()
        {
            string home = string.Empty;
            perfilesRepository.ObtenerPerfil(usuarioCompletoRepository.usuarioCompleto.UidSegPerfil);
            perfilesRepository.appWebRepository.ObtenerAppWeb(perfilesRepository.perfiles.UidAppWeb);
            if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 1)
            {
                home = "Empresas.aspx";
            }
            else if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 2)
            {
                home = "MenuBackEnd.aspx";
            }
            else if (perfilesRepository.appWebRepository.appWeb.IntGerarquia == 3)
            {
                home = "MenuFrontEnd.aspx";
            }
            return home;
        }

        public void CargarMenu(Guid UidAppWeb)
        {
            lsmodulos = new List<PermisosMenuModel>();

            lsmodulos = modulosRepository.CargarMenu(UidAppWeb);
        }
        public void CargarAccesosPermitidos(Guid UidSegPerfil)
        {
            lsAccesosPermitidos = new List<PermisosMenuModel>();

            lsAccesosPermitidos = modulosRepository.CargarAccesosPermitidos(UidSegPerfil);
        }

        public void ObtenerFranquiciaUsuario()
        {
            usuarioCompletoRepository.ObtenerFranquiciaUsuario(usuarioCompletoRepository.usuarioCompleto.UidUsuario);
        }
        public string ObtenerIDFranquiciaUsuario()
        {
            return usuarioCompletoRepository.ObtenerIDFranquiciaUsuario(usuarioCompletoRepository.usuarioCompleto.UidUsuario);
        }
        public void ObtenerFranquiciaClienteUsuario()
        {
            usuarioCompletoRepository.ObtenerFranquiciaClienteUsuario(usuarioCompletoRepository.usuarioCompleto.UidUsuario);
        }

        public void ObtenerDineroCuentaCliente(Guid UidCliente)
        {
            clienteCuentaRepository.ObtenerDineroCuentaCliente(UidCliente);
        }

        public List<UsuariosCompletos> RecoveryPassword(string Parametro, string Dato)
        {
            return lsRecoveryPassword = usuarioCompletoRepository.RecoveryPassword(Parametro, Dato);
        }
        #endregion

        #region MetodosEscuela
        public void IniciarSesionEscuela(string Usuario, string Password)
        {
            BolStatusSesion = false;

            usuarioCompletoRepository.LoginUsuarioEscuela(Usuario, Password);

            if (usuarioCompletoRepository.usuarioCompleto.UidAppWeb == new Guid("514433C7-4439-42F5-ABE4-6BF1C330F0CA"))
            {
                if (usuarioCompletoRepository.ObtenerDatosUsuarioEscuela(usuarioCompletoRepository.usuarioCompleto.UidUsuario))
                {
                    BolStatusSesion = true;
                }
            }
            else
            {
                if (usuarioCompletoRepository.usuarioCompleto.UidUsuario != Guid.Empty)
                {
                    usuarioCompletoRepository.ObtenerEstatusdeEmpresaUsuarioEscuela(usuarioCompletoRepository.usuarioCompleto.UidUsuario);

                    if (usuarioCompletoRepository.usuarioCompleto.UidUsuario != Guid.Empty)
                    {
                        if (usuarioCompletoRepository.usuarioCompleto.UidEstatusEmpresa.ToString() == "65e46bc9-1864-4145-ad1a-70f5b5f69739")
                        {
                            if (usuarioCompletoRepository.usuarioCompleto.UidEstatus.ToString() == "65e46bc9-1864-4145-ad1a-70f5b5f69739")
                            {
                                BolStatusSesion = true;
                            }
                            else
                            {
                                BolStatusSesion = false;
                            }
                        }
                        else
                        {
                            BolStatusSesion = false;
                        }
                    }
                    else
                    {
                        //limpiarPropiedadesUsuarioEmpresas();
                        BolStatusSesion = true;

                    }
                }
                else
                {
                    //limpiarPropiedadesUsuarioEmpresas();
                    BolStatusSesion = false;
                }
            }
        }

        public List<UsuariosCompletos> RecoveryPasswordEscuela(string Parametro, string Dato)
        {            
            return lsRecoveryPassword = usuarioCompletoRepository.RecoveryPasswordEscuela(Parametro, Dato);
        }

        #region Metodos Cliente
        public bool RegistrarLogo(Guid UidCliente, byte[] Imagen)
        {
            return clientesRepository.RegistrarLogo(UidCliente, Imagen);
        }
        public bool ActualizarLogo(Guid UidCliente, byte[] Imagen)
        {
            return clientesRepository.ActualizarLogo(UidCliente, Imagen);
        }
        #endregion
        #endregion
    }
}
