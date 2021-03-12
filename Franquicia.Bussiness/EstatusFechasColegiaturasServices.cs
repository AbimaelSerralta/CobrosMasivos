using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class EstatusFechasColegiaturasServices
    {
        private EstatusFechasColegiaturasRepository estatusFechasColegiaturasRepository = new EstatusFechasColegiaturasRepository();

        public List<EstatusFechasColegiaturas> lsEstatusFechasColegiaturas = new List<EstatusFechasColegiaturas>();

        public void CargarEstatusFechasColegiaturas()
        {
            lsEstatusFechasColegiaturas = new List<EstatusFechasColegiaturas>();

            lsEstatusFechasColegiaturas = estatusFechasColegiaturasRepository.CargarEstatusFechasColegiaturas();
        }
    }
}
