using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class FormasPagosServices
    {
        private FormasPagosRepository formasPagosRepository = new FormasPagosRepository();

        public List<FormasPagos> lsFormasPagos = new List<FormasPagos>();

        public void CargarEstatus()
        {
            lsFormasPagos = new List<FormasPagos>();

            lsFormasPagos = formasPagosRepository.CargarFormasPagos();
        }
    }
}
