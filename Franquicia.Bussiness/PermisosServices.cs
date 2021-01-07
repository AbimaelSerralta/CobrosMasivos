using Franquicia.DataAccess.Repository;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PermisosServices
    {
        private PermisosRepository _permisosRepository = new PermisosRepository();
        public PermisosRepository permisosRepository
        {
            get { return _permisosRepository; }
            set { _permisosRepository = value; }
        }

        public List<PermisosCheckBoxListModel> lsModulosCheckBoxListModel = new List<PermisosCheckBoxListModel>();
        public List<PermisosCheckBoxListModel> lsModulosPermisos = new List<PermisosCheckBoxListModel>();
        public List<PermisosCheckBoxListModel> lsAccesosModulosPermisos = new List<PermisosCheckBoxListModel>();

        public List<PermisosCheckBoxListModel> lsModulosPermisosInsertar = new List<PermisosCheckBoxListModel>();

        public List<PermisosCheckBoxListModel> CargarModulosPermisos(Guid UidSegPerfil)
        {
            lsModulosPermisos = new List<PermisosCheckBoxListModel>();
            return lsModulosPermisos = permisosRepository.CargarModulosPermisos(UidSegPerfil);
        }
        
        public List<PermisosCheckBoxListModel> CargarAccesosModulosPermisos(Guid UidSegPerfil)
        {
            lsAccesosModulosPermisos = new List<PermisosCheckBoxListModel>();
            return lsAccesosModulosPermisos = permisosRepository.CargarAccesosModulosPermisos(UidSegPerfil);
        }
        public List<PermisosCheckBoxListModel> ObtenerModulosPermisos(Guid UidModulo)
        {
            return lsModulosCheckBoxListModel = permisosRepository.ObtenerModulosPermisos(UidModulo);
        }

        public void PrepararLsPermisos(List<string> lsUidPermisos, List<PermisosCheckBoxListModel> lsChckPermisos)
        {
            foreach (var item in lsUidPermisos)
            {
                foreach (var it in lsChckPermisos)
                {
                    //permisosRepository.permisosCheckBoxListModel

                    //lsModulosPermisosInsertar.Add(new PermisosCheckBoxListModel{it.UidPermiso, it.UidSegModulo, it.UidSegPerfil});
                }
            }
        }

        public bool RegistrarModulosPermisos(Guid UidSegPerfil, List<string> lsUidPermisos, List<PermisosCheckBoxListModel> lsChckPermisos)
        {
            bool result = false;

            for (int i = 0; i < lsUidPermisos.Count; i++)
            {
                foreach (var it in lsChckPermisos)
                {
                    if (lsUidPermisos[i] == it.UidPermiso.ToString())
                    {
                        if (permisosRepository.RegistrarModulosPermisos(UidSegPerfil, it.UidSegModulo, it.UidPermiso, true))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public bool ActualizarModulosPermisos(Guid UidSegPerfil, List<string> lsUidDenegarPermisos, List<PermisosCheckBoxListModel> lsChckPermisos, List<string> lsUidPermisos)
        {
            bool result = false;

            for (int i = 0; i < lsUidDenegarPermisos.Count; i++)
            {
                permisosRepository.EliminarModulosPermisos(new Guid(lsUidDenegarPermisos[i]), UidSegPerfil);
            }


            for (int i = 0; i < lsUidPermisos.Count; i++)
            {
                if (!permisosRepository.ValidarModulosPermisosExiste(new Guid(lsUidPermisos[i]), UidSegPerfil))
                {
                    foreach (var it in lsChckPermisos)
                    {
                        if (lsUidPermisos[i] == it.UidPermiso.ToString())
                        {
                            if (permisosRepository.RegistrarModulosPermisos(UidSegPerfil, it.UidSegModulo, it.UidPermiso, true))
                            {
                                result = true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        #region Metodos Franquicias
        public List<PermisosCheckBoxListModel> CargarModulosPermisosFranquicias(Guid UidSegPerfil)
        {
            lsModulosPermisos = new List<PermisosCheckBoxListModel>();
            return lsModulosPermisos = permisosRepository.CargarModulosPermisosFranquicias(UidSegPerfil);
        }
        #endregion
    }
}
