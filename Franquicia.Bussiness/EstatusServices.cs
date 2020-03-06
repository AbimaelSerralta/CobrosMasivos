using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EstatusServices
    {
        private EstatusRepository estatusRepository = new EstatusRepository();

        public List<Estatus> lsEstatus = new List<Estatus>();

        public void CargarEstatus()
        {
            lsEstatus = new List<Estatus>();

            lsEstatus = estatusRepository.CargarEstatus();
        }
    }
}
