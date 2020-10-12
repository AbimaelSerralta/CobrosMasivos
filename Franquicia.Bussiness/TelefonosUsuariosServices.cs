using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TelefonosUsuariosServices
    {
        private TelefonosUsuariosRepository _telefonosUsuariosRepository = new TelefonosUsuariosRepository();
        public TelefonosUsuariosRepository telefonosUsuariosRepository
        {
            get { return _telefonosUsuariosRepository; }
            set { _telefonosUsuariosRepository = value; }
        }


        public List<TelefonosUsuarios> lsTelefonosFranquiciatarios = new List<TelefonosUsuarios>();

        public TelefonosUsuarios ObtenerTelefonoUsuario(Guid UidUsuario)
        {
            return telefonosUsuariosRepository.ObtenerTelefonoUsuario(UidUsuario);
        }

        public TelefonosUsuarios ObtenerTelefonoUsuarioSinPrefijo(Guid UidUsuario)
        {
            return telefonosUsuariosRepository.ObtenerTelefonoUsuarioSinPrefijo(UidUsuario);
        }

        #region Twilio
        public bool ActualizarEstatusWhats(Guid UidTelefono, Guid UidPermisoWhats)
        {
            return telefonosUsuariosRepository.ActualizarEstatusWhats(UidTelefono, UidPermisoWhats);
        }
        public string ObtenerIdTelefono(string Telefono)
        {
            return telefonosUsuariosRepository.ObtenerIdTelefono(Telefono);
        }
        #endregion

    }
}
