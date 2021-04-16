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
    public class ManejoSesionSandboxServices
    {
        ManejoSesionSandboxRepository _manejoSesionSandboxRepository = new ManejoSesionSandboxRepository();
        public ManejoSesionSandboxRepository manejoSesionSandboxRepository
        {
            get { return _manejoSesionSandboxRepository; }
            set { _manejoSesionSandboxRepository = value; }
        }

        private bool _BolStatusSesion;
        public bool BolStatusSesion
        {
            get { return _BolStatusSesion; }
            set { _BolStatusSesion = value; }
        }

        public List<ModulosSandbox> lsAccesosPermitidos = new List<ModulosSandbox>();

        #region Metodos
        public void IniciarSesion(string Usuario, string Password)
        {
            BolStatusSesion = false;

            manejoSesionSandboxRepository.LoginUsuario(Usuario, Password);

            if (manejoSesionSandboxRepository.integracionesCompleto.UidIntegracion != Guid.Empty)
            {
                BolStatusSesion = true;
            }
            else
            {
                BolStatusSesion = false;
            }
        }
        public String ObtenerHome(Guid UidCredencial)
        {
            string Home = string.Empty;

            Home = manejoSesionSandboxRepository.ObtenerModuloInicial(UidCredencial);

            return Home;
        }
        public void CargarAccesosPermitidos(Guid UidCredencial)
        {
            lsAccesosPermitidos = new List<ModulosSandbox>();

            lsAccesosPermitidos = manejoSesionSandboxRepository.CargarAccesosPermitidos(UidCredencial);
        }
        #endregion
    }
}
