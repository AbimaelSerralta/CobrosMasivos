using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class BancosServices
    {
        private BancosRepository estatusRepository = new BancosRepository();

        public List<Bancos> lsBancos = new List<Bancos>();

        public void CargarBancos()
        {
            lsBancos = new List<Bancos>();

            lsBancos = estatusRepository.CargarBancos();
        }
    }
}
