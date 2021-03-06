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
    public class ParametrosPragaServices
    {
        private ParametrosPragaRepository _parametrosPragaRepository = new ParametrosPragaRepository();
        public ParametrosPragaRepository parametrosPragaRepository
        {
            get { return _parametrosPragaRepository; }
            set { _parametrosPragaRepository = value; }
        }


        #region Metodos Franquicia
        public bool ObtenerParametrosPragaBl(Guid UidPropietario)
        {
            return parametrosPragaRepository.ObtenerParametrosPragaBl(UidPropietario);
        }
        public void ObtenerParametrosPraga(Guid UidPropietario)
        {
            parametrosPragaRepository.ObtenerParametrosPraga(UidPropietario);
        }

        public bool RegistrarParametrosPraga(string BusinessId, string VchUrl, string UserCode, string WSEncryptionKey, string APIKey, string Currency, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosPragaRepository.RegistrarParametrosPraga(
                new ParametrosPraga
                {
                    UidParametro = Guid.NewGuid(),
                    BusinessId = BusinessId,
                    VchUrl = VchUrl,
                    UserCode = UserCode,
                    WSEncryptionKey = WSEncryptionKey,
                    APIKey = APIKey,
                    Currency = Currency,
                    UidPropietario = UidPropietario
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarParametrosPraga(string BusinessId, string VchUrl, string UserCode, string WSEncryptionKey, string APIKey, string Currency, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosPragaRepository.ActualizarParametrosPraga(
                new ParametrosPraga
                {
                    BusinessId = BusinessId,
                    VchUrl = VchUrl,
                    UserCode = UserCode,
                    WSEncryptionKey = WSEncryptionKey,
                    APIKey = APIKey,
                    Currency = Currency,
                    UidPropietario = UidPropietario
                }))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
