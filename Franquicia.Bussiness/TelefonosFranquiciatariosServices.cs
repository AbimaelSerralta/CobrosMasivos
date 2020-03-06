using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TelefonosFranquiciatariosServices
    {
        private TelefonosFranquiciatariosRepository _telefonosFranquiciatariosRepository = new TelefonosFranquiciatariosRepository();
        public TelefonosFranquiciatariosRepository telefonosFranquiciatariosRepository
        {
            get { return _telefonosFranquiciatariosRepository; }
            set { _telefonosFranquiciatariosRepository = value; }
        }


        public List<TelefonosFranquiciatarios> lsTelefonosFranquiciatarios = new List<TelefonosFranquiciatarios>();

        public TelefonosFranquiciatarios ObtenerTelefonoFranquiciatario(Guid UidFranquiciatario)
        {
            return telefonosFranquiciatariosRepository.ObtenerTelefonoFranquiciatario(UidFranquiciatario);
        }

    }
}
