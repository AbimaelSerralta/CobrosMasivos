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
    public class PagosPadresServices
    {
        private PagosPadresRepository _pagosPadresRepository = new PagosPadresRepository();
        public PagosPadresRepository pagosPadresRepository
        {
            get { return _pagosPadresRepository; }
            set { _pagosPadresRepository = value; }
        }

        public List<PadresComerciosViewModels> lsPadresComerciosViewModels = new List<PadresComerciosViewModels>();

        #region Metodos Franquicia

        #endregion

        #region Metodos Cliente

        #endregion

        #region Metodos Padres
        public List<PadresComerciosViewModels> CargarComercios(Guid UidUsuario)
        {
            return lsPadresComerciosViewModels = pagosPadresRepository.CargarComercios(UidUsuario);
        }
        
        #endregion
    }
}
