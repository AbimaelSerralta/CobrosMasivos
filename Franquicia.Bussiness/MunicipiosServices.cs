using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class MunicipiosServices
    {
        private MunicipiosRepository municipiosRepository = new MunicipiosRepository();

        public List<Municipios> lsMunicipios = new List<Municipios>();

        public List<Municipios> CargarMunicipios(string UidEstado)
        {
            lsMunicipios = new List<Municipios>();

            return lsMunicipios = municipiosRepository.CargarMunicipios(UidEstado);
        }
    }
}
