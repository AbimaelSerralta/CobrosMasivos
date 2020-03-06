using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class DireccionesFranquiciatariosServices
    {
        private DireccionesFranquiciatariosRepository _direccionesFranquiciatariosRepository = new DireccionesFranquiciatariosRepository();
        public DireccionesFranquiciatariosRepository direccionesFranquiciatariosRepository
        {
            get { return _direccionesFranquiciatariosRepository; }
            set { _direccionesFranquiciatariosRepository = value; }
        }


        public List<DireccionesFranquiciatariosRepository> lsDireccionesFranquiciatariosRepository = new List<DireccionesFranquiciatariosRepository>();

        public DireccionesFranquiciatarios ObtenerDireccionesFranquiciatarios(Guid UidFranquiciatario)
        {
            return direccionesFranquiciatariosRepository.ObtenerTelefonoFranquiciatario(UidFranquiciatario);
        }
    }
}
