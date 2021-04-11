using Franquicia.DataAccess.Repository;
using Franquicia.Domain.Models;
using Franquicia.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franquicia.Bussiness
{
    public class ParametrosClubPagoServices
    {
        private ParametrosClubPagoRepository _parametrosClubPagoRepository = new ParametrosClubPagoRepository();
        public ParametrosClubPagoRepository parametrosClubPagoRepository
        {
            get { return _parametrosClubPagoRepository; }
            set { _parametrosClubPagoRepository = value; }
        }


        #region Metodos Franquicia
        public void ObtenerParametrosClubPago()
        {
            parametrosClubPagoRepository.ObtenerParametrosClubPago();
        }

        public bool RegistrarParametrosClubPago(string VchApiKey)
        {
            bool result = false;
            if (parametrosClubPagoRepository.RegistrarParametrosClubPago(
                new ParametrosSendGrid
                {
                    VchApiKey = VchApiKey
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarParametrosClubPago(string VchApiKey)
        {
            bool result = false;
            if (parametrosClubPagoRepository.ActualizarParametrosClubPago(
                new ParametrosSendGrid
                {
                    VchApiKey = VchApiKey
                }))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Metodos Integraciones

        #endregion
    }
}
