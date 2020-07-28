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
    public class ParametrosTwiServices
    {
        private ParametrosTwiRepository _parametrosTwiRepository = new ParametrosTwiRepository();
        public ParametrosTwiRepository parametrosTwiRepository
        {
            get { return _parametrosTwiRepository; }
            set { _parametrosTwiRepository = value; }
        }


        #region Metodos Franquicia
        public void CargarParametrosTwi()
        {
            parametrosTwiRepository.CargarParametrosTwi();
        }

        public void ObtenerParametrosEntradaFraquicia()
        {
            parametrosTwiRepository.ObtenerParametrosTwi();
        }

        public bool RegistrarParametrosEntradaFraquicia(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosTwiRepository.RegistrarParametrosTwi(
                new ParametrosEntrada
                {
                    UidParametroEntrada = Guid.NewGuid(),
                    IdCompany = IdCompany,
                    IdBranch = IdBranch,
                    VchModena = VchModena,
                    VchUsuario = VchUsuario,
                    VchPassword = VchPassword,
                    VchCanal = VchCanal,
                    VchData0 = VchData0,
                    VchUrl = VchUrl,
                    VchSemillaAES = VchSemillaAES,
                    UidPropietario = UidPropietario
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarParametrosEntradaFraquicia(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosTwiRepository.ActualizarParametrosTwi(
                new ParametrosEntrada
                {
                    IdCompany = IdCompany,
                    IdBranch = IdBranch,
                    VchModena = VchModena,
                    VchUsuario = VchUsuario,
                    VchPassword = VchPassword,
                    VchCanal = VchCanal,
                    VchData0 = VchData0,
                    VchUrl = VchUrl,
                    VchSemillaAES = VchSemillaAES,
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
