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
    public class SuperPromocionesPragaServices
    {
        private SuperPromocionesPragaRepository superPromocionesPragaRepository = new SuperPromocionesPragaRepository();

        public List<PromocionesPraga> lsPromocionesPraga = new List<PromocionesPraga>();
        public List<SuperPromocionesPragaDisponiblesViewModel> lsSuperPromocionesPragaDisponiblesViewModel = new List<SuperPromocionesPragaDisponiblesViewModel>();
        public List<SuperPromocionesPraga> lsCBLSuperPromocionesPraga = new List<SuperPromocionesPraga>();
        
        public void CargarPromocionesPraga()
        {
            lsPromocionesPraga = new List<PromocionesPraga>();

            lsPromocionesPraga = superPromocionesPragaRepository.CargarPromocionesPraga();
        }

        #region SuperPromocionesPraga
        public List<SuperPromocionesPragaDisponiblesViewModel> CargarSuperPromocionesPragaDisponible()
        {
            return lsSuperPromocionesPragaDisponiblesViewModel = superPromocionesPragaRepository.CargarSuperPromocionesPragaDisponible();
        }
        public List<SuperPromocionesPraga> CargarSuperPromocionesPraga()
        {
            return lsCBLSuperPromocionesPraga = superPromocionesPragaRepository.CargarSuperPromocionesPraga();
        }

        public bool RegistrarSuperPromocionesPraga(Guid UidPromocion, decimal DcmComicion, decimal DcmApartirDe, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (superPromocionesPragaRepository.RegistrarSuperPromocionesPraga(
                new SuperPromocionesPraga
                {
                    UidPromocion = UidPromocion,
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    UidTipoTarjeta = UidTipoTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarSuperPromocionesPraga()
        {
            bool result = false;
            if (superPromocionesPragaRepository.EliminarSuperPromocionesPraga())
            {
                result = true;
            }
            return result;
        }
        
        public bool RegistrarCodigoPromocionesPraga(string VchCodigo, Guid UidPromocion, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (superPromocionesPragaRepository.RegistrarCodigoPromocionesPraga(VchCodigo, UidPromocion, UidTipoTarjeta))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarCodigoPromocionesPraga()
        {
            bool result = false;
            if (superPromocionesPragaRepository.EliminarCodigoPromocionesPraga())
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
