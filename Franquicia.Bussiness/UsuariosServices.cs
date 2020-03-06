using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
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

        public void CargarUsuarios()
        {
            lsUsua = new List<Usuarios>();

            lsUsua = usuarioRepository.CargarUsuarios();
        }
    }
}
