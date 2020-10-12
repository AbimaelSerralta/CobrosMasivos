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
    public class SuperPromocionesServices
    {
        private SuperPromocionesRepository superPromocionesRepository = new SuperPromocionesRepository();

        public List<Promociones> lsPromociones = new List<Promociones>();
        public List<SuperPromocionesDisponiblesViewModel> lsSuperPromocionesDisponiblesViewModel = new List<SuperPromocionesDisponiblesViewModel>();
        public List<SuperPromociones> lsCBLSuperPromociones = new List<SuperPromociones>();
        
        public void CargarPromociones()
        {
            lsPromociones = new List<Promociones>();

            lsPromociones = superPromocionesRepository.CargarPromociones();
        }

        #region SuperPromociones
        public List<SuperPromocionesDisponiblesViewModel> CargarSuperPromocionesDisponible()
        {
            return lsSuperPromocionesDisponiblesViewModel = superPromocionesRepository.CargarSuperPromocionesDisponible();
        }
        public List<SuperPromociones> CargarSuperPromociones()
        {
            return lsCBLSuperPromociones = superPromocionesRepository.CargarSuperPromociones();
        }

        public bool RegistrarSuperPromociones(Guid UidPromocion, decimal DcmComicion, decimal DcmApartirDe)
        {
            bool result = false;
            if (superPromocionesRepository.RegistrarSuperPromociones(
                new SuperPromociones
                {
                    UidPromocion = UidPromocion,
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe
                }))
            {
                result = true;
            }
            return result;
        }

        public bool EliminarSuperPromociones()
        {
            bool result = false;
            if (superPromocionesRepository.EliminarSuperPromociones())
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
