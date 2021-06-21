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
    public class ParametrosEntradaServices
    {
        private ParametrosEntradaRepository _parametrosEntradaRepository = new ParametrosEntradaRepository();
        public ParametrosEntradaRepository parametrosEntradaRepository
        {
            get { return _parametrosEntradaRepository; }
            set { _parametrosEntradaRepository = value; }
        }


        #region Metodos Franquicia
        public void ObtenerParametrosEntradaFraquicia(Guid UidFranquiciatario)
        {
            parametrosEntradaRepository.ObtenerParametrosEntrada(UidFranquiciatario);
        }

        public bool RegistrarParametrosEntradaFraquicia(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosEntradaRepository.RegistrarParametrosEntrada(
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
            if (parametrosEntradaRepository.ActualizarParametrosEntrada(
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

        #region CobrosMasivos
        #region Metodos Cliente
        public void ObtenerParametrosEntradaClienteCM(Guid UidCliente)
        {
            parametrosEntradaRepository.ObtenerParametrosEntradaClienteCM(UidCliente);
        }

        public bool RegistrarParametrosEntradaClienteCM(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario, bool BitImporteLiga, decimal DcmImporteMin, decimal DcmImporteMax)
        {
            bool result = false;
            if (parametrosEntradaRepository.RegistrarParametrosEntradaClienteCM(
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
                    UidPropietario = UidPropietario,
                    BitImporteLiga = BitImporteLiga,
                    DcmImporteMin = DcmImporteMin,
                    DcmImporteMax = DcmImporteMax
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarParametrosEntradaClienteCM(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario, bool BitImporteLiga, decimal DcmImporteMin, decimal DcmImporteMax)
        {
            bool result = false;
            if (parametrosEntradaRepository.ActualizarParametrosEntradaClienteCM(
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
                    UidPropietario = UidPropietario,
                    BitImporteLiga = BitImporteLiga,
                    DcmImporteMin = DcmImporteMin,
                    DcmImporteMax = DcmImporteMax
                }))
            {
                result = true;
            }
            return result;
        }

        #endregion
        #endregion

        #region PagaLaEscuela
        #region Metodos Cliente
        public void ObtenerParametrosEntradaCliente(Guid UidCliente)
        {
            parametrosEntradaRepository.ObtenerParametrosEntradaCliente(UidCliente);
        }

        public bool RegistrarParametrosEntradaCliente(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosEntradaRepository.RegistrarParametrosEntradaCliente(
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

        public bool ActualizarParametrosEntradaCliente(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (parametrosEntradaRepository.ActualizarParametrosEntradaCliente(
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
        #endregion
    }
}
