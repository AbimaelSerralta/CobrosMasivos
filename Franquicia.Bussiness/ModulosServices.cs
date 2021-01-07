using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class ModulosServices
    {
        private ModulosRepository _modulosRepository = new ModulosRepository();
        public ModulosRepository modulosRepository
        {
            get { return _modulosRepository; }
            set { _modulosRepository = value; }
        }


        public List<Modulos> lsmodulos = new List<Modulos>();

        public void CargarModulosNivel()
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivel();
        }

        public void CargarModulosNivelPrincipal(Guid UidSegPerfil)
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelPrincipal(UidSegPerfil);
        }
        public void CargarModulosNivelFranquicias(Guid UidSegPerfil)
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelFranquicias(UidSegPerfil);
        }
        public void CargarModulosNivelClientes(Guid UidSegPerfil)
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelClientes(UidSegPerfil);
        }
        public void CargarModulosNivelUsuarios(Guid UidSegPerfil)
        {
            lsmodulos = new List<Modulos>();
            lsmodulos = modulosRepository.CargarModulosNivelUsuarios(UidSegPerfil);
        }
        public void CargarMenu(Guid UidAppWeb)
        {
            //lsmodulos = new List<Modulos>();

            //lsmodulos = modulosRepository.CargarMenu(UidAppWeb);
        }

        public List<PermisosMenuModel> CargarAccesosPermitidos(Guid UidSegPerfil)
        {
            return modulosRepository.CargarAccesosPermitidos(UidSegPerfil);
        }
    }
}
