using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposPerfilesServices
    {
        TiposPerfilesRepository tipoPerfilRepository = new TiposPerfilesRepository();

        public List<TiposPerfiles> lsTipoPerfil = new List<TiposPerfiles>();

        public List<TiposPerfiles> CargarTipoPerfil(Guid UidAppWeb)
        {
            lsTipoPerfil = new List<TiposPerfiles>();

            lsTipoPerfil = tipoPerfilRepository.CargarTipoPerfil(UidAppWeb);

            return lsTipoPerfil;
        }
    }
}
