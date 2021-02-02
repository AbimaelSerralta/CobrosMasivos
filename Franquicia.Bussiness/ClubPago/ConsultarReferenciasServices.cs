using Franquicia.DataAccess.Repository.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class ConsultarReferenciasServices
    {
        private ConsultarReferenciasRepository _consultarReferenciasRepository = new ConsultarReferenciasRepository();
        public ConsultarReferenciasRepository consultarReferenciasRepository
        {
            get { return _consultarReferenciasRepository; }
            set { _consultarReferenciasRepository = value; }
        }

        public ConsultarReferencia ConsultarReferenciaClubPago(string IdReferencia, DateTime day)
        {
            return consultarReferenciasRepository.ConsultarReferenciaClubPago(IdReferencia, day);
        }
    }
}
