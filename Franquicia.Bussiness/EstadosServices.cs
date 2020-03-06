using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EstadosServices
    {
        private EstadosRepository estadosRepository = new EstadosRepository();

        public List<Estados> lsEstados = new List<Estados>();

        public List<Estados> CargarEstados(string UidPais)
        {
            lsEstados = new List<Estados>();

            return lsEstados = estadosRepository.CargarEstados(UidPais);
        }
    }
}
