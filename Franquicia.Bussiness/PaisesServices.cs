using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class PaisesServices
    {
        private PaisesRepository paisesRepository = new PaisesRepository();

        public List<Paises> lsPaises = new List<Paises>();

        public List<Paises> CargarPaises()
        {
            lsPaises = new List<Paises>();

            return lsPaises = paisesRepository.CargarPaises();
        }
    }
}
