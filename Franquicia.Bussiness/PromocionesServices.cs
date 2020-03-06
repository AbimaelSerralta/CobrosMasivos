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
    public class PromocionesServices
    {
        private PromocionesRepository promocionesRepository = new PromocionesRepository();

        public List<Promociones> lsPromociones = new List<Promociones>();
        public List<CBLPromocionesModel> lsCBLPromocionesModel = new List<CBLPromocionesModel>();

        public void CargarPromociones()
        {
            lsPromociones = new List<Promociones>();

            lsPromociones = promocionesRepository.CargarPromociones();
        }

        #region Empresas
        public List<CBLPromocionesModel> CargarPromociones(Guid UidCliente)
        {
            return lsCBLPromocionesModel = promocionesRepository.CargarPromociones(UidCliente);
        }
        #endregion
    }
}
