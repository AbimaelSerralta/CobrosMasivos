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
    public class ParametrosSendGridServices
    {
        private ParametrosSendGridRepository _parametrosSendGridRepository = new ParametrosSendGridRepository();
        public ParametrosSendGridRepository parametrosSendGridRepository
        {
            get { return _parametrosSendGridRepository; }
            set { _parametrosSendGridRepository = value; }
        }


        #region Metodos Franquicia
        public void CargarParametrosSendGrid()
        {
            parametrosSendGridRepository.CargarParametrosSendGrid();
        }

        public void ObtenerParametrosSendGrid()
        {
            parametrosSendGridRepository.ObtenerParametrosSendGrid();
        }

        public bool RegistrarParametrosSendGrid(string VchApiKey)
        {
            bool result = false;
            if (parametrosSendGridRepository.RegistrarParametrosSendGrid(
                new ParametrosSendGrid
                {
                    VchApiKey = VchApiKey
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarParametrosSendGrid(string VchApiKey)
        {
            bool result = false;
            if (parametrosSendGridRepository.ActualizarParametrosSendGrid(
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
    }
}
