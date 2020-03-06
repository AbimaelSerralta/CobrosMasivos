using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class DireccionesUsuariosServices
    {
        private DireccionesUsuariosRepository _direccionesUsuariosRepository = new DireccionesUsuariosRepository();
        public DireccionesUsuariosRepository direccionesUsuariosRepository
        {
            get { return _direccionesUsuariosRepository; }
            set { _direccionesUsuariosRepository = value; }
        }


        public List<DireccionesUsuariosRepository> lsDireccionesFranquiciatariosRepository = new List<DireccionesUsuariosRepository>();

        public DireccionesUsuarios ObtenerDireccionesUsuarios(Guid UidUsuario)
        {
            return direccionesUsuariosRepository.ObtenerDireccionesUsuarios(UidUsuario);
        }
    }
}
