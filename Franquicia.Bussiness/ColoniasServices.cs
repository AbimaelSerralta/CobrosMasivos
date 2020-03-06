using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class ColoniasServices
    {
        private ColoniasRepository coloniasRepository = new ColoniasRepository();

        public List<Colonias> lsColonias = new List<Colonias>();

        public List<Colonias> CargarColonias(string UidCiudad)
        {
            lsColonias = new List<Colonias>();

            return lsColonias = coloniasRepository.CargarColonias(UidCiudad);
        }
    }
}
