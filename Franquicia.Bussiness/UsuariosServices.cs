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
    public class UsuariosServices
    {
        private UsuariosRepository usuarioRepository = new UsuariosRepository();

        public List<Usuarios> lsUsua = new List<Usuarios>();
        
        public List<UsuarioGridViewModel> lsUsuarioGridViewModel = new List<UsuarioGridViewModel>();

        public void CargarUsuarios()
        {
            lsUsua = new List<Usuarios>();

            lsUsua = usuarioRepository.CargarUsuarios();
        }

        #region PanelTutor
        #region ReporteLigasEscuela
        public void CargarTutoresAlumnos(Guid UidAlumno)
        {
            lsUsuarioGridViewModel = new List<UsuarioGridViewModel>();

            lsUsuarioGridViewModel = usuarioRepository.CargarTutoresAlumnos(UidAlumno);
        }
        #endregion
        #endregion
    }
}
