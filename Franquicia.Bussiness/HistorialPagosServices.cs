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
    public class HistorialPagosServices
    {
        private HistorialPagosRepository _historialPagosRepository = new HistorialPagosRepository();
        public HistorialPagosRepository historialPagosRepository
        {
            get { return _historialPagosRepository; }
            set { _historialPagosRepository = value; }
        }

        public List<HistorialPagosGridViewModel> lsHistorialPagosGridViewModel = new List<HistorialPagosGridViewModel>();

        #region Metodos Franquicia

        #endregion

        #region Metodos Cliente
        public List<HistorialPagosGridViewModel> CargarMovimientos(Guid UidCliente)
        {
            return lsHistorialPagosGridViewModel = historialPagosRepository.CargarMovimientos(UidCliente);
        }
        public void ObtenerHistorialPago(Guid UidCliente)
        {
            historialPagosRepository.ObtenerHistorialPago(UidCliente);
        }
        public bool RegistrarHistorialPago(decimal DcmSaldo, decimal DcmOperacion, decimal DcmNuevoSaldo, string IdReferencia)
        {
            bool result = false;
            if (historialPagosRepository.RegistrarHistorialPago(
                new HistorialPagos
                {
                    UidHistorialPago = Guid.NewGuid(),
                    DcmSaldo = DcmSaldo,
                    DcmOperacion = DcmOperacion,
                    DcmNuevoSaldo = DcmNuevoSaldo,
                    IdReferencia = IdReferencia
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool RegistrarHistorialPagoTicket(Guid UidHistorialPago, decimal DcmSaldo, decimal DcmOperacion, decimal DcmNuevoSaldo, string IdReferencia)
        {
            bool result = false;
            if (historialPagosRepository.RegistrarHistorialPago(
                new HistorialPagos
                {
                    UidHistorialPago = UidHistorialPago,
                    DcmSaldo = DcmSaldo,
                    DcmOperacion = DcmOperacion,
                    DcmNuevoSaldo = DcmNuevoSaldo,
                    IdReferencia = IdReferencia
                }
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarParametrosEntradaCliente(string IdCompany, string IdBranch, string VchModena, string VchUsuario, string VchPassword, string VchCanal, string VchData0, string VchUrl, string VchSemillaAES, Guid UidPropietario)
        {
            bool result = false;
            if (historialPagosRepository.ActualizarParametrosEntradaCliente(
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
        public bool EliminarHistorialPagoLigas(string IdReferencia)
        {
            bool result = false;
            if (historialPagosRepository.EliminarHistorialPagoLigas(IdReferencia))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
