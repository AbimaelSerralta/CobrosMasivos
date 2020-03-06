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

        public void CargarModulosNivelPrincipal()
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelPrincipal();
        }
        public void CargarModulosNivelFranquicias()
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelFranquicias();
        }
        public void CargarModulosNivelClientes()
        {
            lsmodulos = new List<Modulos>();

            lsmodulos = modulosRepository.CargarModulosNivelClientes();
        }
        public void CargarModulosNivelUsuarios()
        {
            lsmodulos = new List<Modulos>();
            lsmodulos = modulosRepository.CargarModulosNivelUsuarios();
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
