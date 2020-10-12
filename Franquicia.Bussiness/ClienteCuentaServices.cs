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
    public class ClienteCuentaServices
    {
        private ClienteCuentaRepository _clienteCuentaRepository = new ClienteCuentaRepository();
        public ClienteCuentaRepository clienteCuentaRepository
        {
            get { return _clienteCuentaRepository; }
            set { _clienteCuentaRepository = value; }
        }
        
        
        #region Metodos Franquicia
        
        #endregion

        #region Metodos Cliente
        public void ObtenerDineroCuentaCliente(Guid UidCliente)
        {
            clienteCuentaRepository.ObtenerDineroCuentaCliente(UidCliente);
        }
        public bool RegistrarDineroCuentaCliente(decimal DineroCuenta, Guid UidCliente, string IdReferencia)
        {
            bool result = false;
            if (clienteCuentaRepository.RegistrarDineroCuentaCliente(
                new ClienteCuenta
                {
                    UidClienteCuenta = Guid.NewGuid(),
                    DcmDineroCuenta = DineroCuenta,
                    UidCliente = UidCliente
                }, IdReferencia
                ))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarDineroCuentaCliente(decimal DineroCuenta, Guid UidCliente, string IdReferencia)
        {
            bool result = false;
            if (clienteCuentaRepository.ActualizarDineroCuentaCliente(
                new ClienteCuenta
                {
                    DcmDineroCuenta = DineroCuenta,
                    UidCliente = UidCliente
                }, IdReferencia
                ))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
