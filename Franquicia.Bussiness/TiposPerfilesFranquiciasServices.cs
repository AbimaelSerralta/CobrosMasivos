using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class TiposPerfilesFranquiciasServices
    {
        TiposPerfilesFranquiciasRepository tiposPerfilesFranquiciasRepository = new TiposPerfilesFranquiciasRepository();

        public List<TiposPerfilesFranquicia> lsTiposPerfilesFranquicia = new List<TiposPerfilesFranquicia>();

        public List<TiposPerfilesFranquicia> CargarTipoPerfilFranquicia()
        {
            lsTiposPerfilesFranquicia = new List<TiposPerfilesFranquicia>();

            lsTiposPerfilesFranquicia = tiposPerfilesFranquiciasRepository.CargarTipoPerfilFranquicia();

            return lsTiposPerfilesFranquicia;
        }
    }
}
