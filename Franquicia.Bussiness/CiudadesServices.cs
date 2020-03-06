using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class CiudadesServices
    {
        private CiudadesRepository ciudadesRepository = new CiudadesRepository();

        public List<Ciudades> lsCiudades = new List<Ciudades>();

        public List<Ciudades> CargarCiudades(string UidMunicipio)
        {
            lsCiudades = new List<Ciudades>();

            return lsCiudades = ciudadesRepository.CargarCiudades(UidMunicipio);
        }
    }
}
