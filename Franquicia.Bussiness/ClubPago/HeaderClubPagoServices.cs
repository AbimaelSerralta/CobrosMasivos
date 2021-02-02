using Franquicia.DataAccess.Repository.ClubPago;
using Franquicia.Domain.Models.ClubPago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness.ClubPago
{
    public class HeaderClubPagoServices
    {
        private HeaderClubPagoRepository _headerClubPagoRepository = new HeaderClubPagoRepository();
        public HeaderClubPagoRepository headerClubPagoRepository
        {
            get { return _headerClubPagoRepository; }
            set { _headerClubPagoRepository = value; }
        }

        public HeaderClubPago ObtenerHeaderClubPago()
        {
            return headerClubPagoRepository.ObtenerHeaderClubPago();
        }
    }
}
