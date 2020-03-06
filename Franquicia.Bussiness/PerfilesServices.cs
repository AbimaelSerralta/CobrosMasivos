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
    public class PerfilesServices
    {
        private PerfilesRepository _perfilesRepository = new PerfilesRepository();
        public PerfilesRepository perfilesRepository
        {
            get { return _perfilesRepository; }
            set { _perfilesRepository = value; }
        }

        public List<PerfilesGridViewModel> lsperfilesGridViewModel = new List<PerfilesGridViewModel>();
        public List<PerfilesDropDownListModel> lsPerfilesDropDownListModel = new List<PerfilesDropDownListModel>();

        public List<PerfilesGridViewModel> CargarPerfilesGridViewModel()
        {
            lsperfilesGridViewModel = new List<PerfilesGridViewModel>();

            return lsperfilesGridViewModel = perfilesRepository.CargarPerfilesGridViewModel();
        }
        public List<PerfilesDropDownListModel> CargarPerfilesDropDownListModel(Guid UidSegPerfil)
        {
            return lsPerfilesDropDownListModel = perfilesRepository.CargarPerfilesDropDownListModel(UidSegPerfil);
        }
        
        public void ObtenerPerfil(Guid UidSegPerfil)
        {
            perfilesRepository.perfilesGridViewModel = new PerfilesGridViewModel();
            perfilesRepository.perfilesGridViewModel = lsperfilesGridViewModel.Find(x => x.UidSegPerfil == UidSegPerfil);
        }

        public bool RegistrarPerfiles(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid ModuloInicial, Guid TipoPerfil)
        {
            bool result = false;
            if (perfilesRepository.RegistrarPerfiles(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarPerfiles(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid Estatus, Guid ModuloInicial, Guid TipoPerfil)
        {
            bool result = false;
            if (perfilesRepository.ActualizarPerfiles(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidEstatus = Estatus,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil
                }))
            {
                result = true;
            }
            return result;
        }

        #region Metodos Franquicia
        public List<PerfilesGridViewModel> CargarPerfilesFranquiciaGridViewModel(Guid UidFranquiciatario)
        {
            lsperfilesGridViewModel = new List<PerfilesGridViewModel>();

            return lsperfilesGridViewModel = perfilesRepository.CargarPerfilesFranquiciaGridViewModel(UidFranquiciatario);
        }

        public List<PerfilesDropDownListModel> CargarPerfilesFranquiciaDropDownListModel(Guid UidFranquiciatario, Guid UidSegPerfil)
        {
            return lsPerfilesDropDownListModel = perfilesRepository.CargarPerfilesFranquiciaDropDownListModel(UidFranquiciatario, UidSegPerfil);
        }

        public void ObtenerPerfilFranquicia(Guid UidSegPerfil)
        {
            perfilesRepository.perfilesGridViewModel = new PerfilesGridViewModel();
            perfilesRepository.perfilesGridViewModel = lsperfilesGridViewModel.Find(x => x.UidSegPerfil == UidSegPerfil);
        }

        public bool RegistrarPerfilesFranquicia(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid UidFranquiciatario, Guid ModuloInicial, Guid TipoPerfil)
        {
            bool result = false;
            if (perfilesRepository.RegistrarPerfilesFranquicia(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidFranquiciatario = UidFranquiciatario,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarPerfilesFranquicia(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid Estatus, Guid ModuloInicial, Guid TipoPerfil)
        {
            bool result = false;
            if (perfilesRepository.ActualizarPerfilesFranquicia(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidEstatus = Estatus,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil
                }))
            {
                result = true;
            }
            return result;
        }

        #endregion

        #region Metodos Cliente
        public List<PerfilesGridViewModel> CargarPerfilesClienteGridViewModel(Guid UidCliente)
        {
            lsperfilesGridViewModel = new List<PerfilesGridViewModel>();

            return lsperfilesGridViewModel = perfilesRepository.CargarPerfilesClienteGridViewModel(UidCliente);
        }

        public List<PerfilesDropDownListModel> CargarPerfilesClienteDropDownListModel(Guid UidCliente, Guid UidSegPerfil)
        {
            return lsPerfilesDropDownListModel = perfilesRepository.CargarPerfilesClienteDropDownListModel(UidCliente, UidSegPerfil);
        }

        public void ObtenerPerfilCliente(Guid UidSegPerfil)
        {
            perfilesRepository.perfilesGridViewModel = new PerfilesGridViewModel();
            perfilesRepository.perfilesGridViewModel = lsperfilesGridViewModel.Find(x => x.UidSegPerfil == UidSegPerfil);
        }

        public bool RegistrarPerfilesCliente(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid ModuloInicial, Guid TipoPerfil, Guid UidCliente)
        {
            bool result = false;
            if (perfilesRepository.RegistrarPerfilesCliente(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil,
                    UidCliente = UidCliente,
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarPerfilesCliente(
            Guid UidSegPerfil, string Nombre, Guid AppWeb, Guid Estatus, Guid ModuloInicial, Guid TipoPerfil)
        {
            bool result = false;
            if (perfilesRepository.ActualizarPerfilesCliente(
                new Perfiles
                {
                    UidSegPerfil = UidSegPerfil,
                    VchNombre = Nombre,
                    UidAppWeb = AppWeb,
                    UidEstatus = Estatus,
                    UidModuloInicial = ModuloInicial,
                    UidTipoPerfil = TipoPerfil
                }))
            {
                result = true;
            }
            return result;
        }

        #endregion
    }
}
