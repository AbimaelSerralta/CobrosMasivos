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
    public class ComisionesTarjetasClientesServices
    {
        private ComisionesTarjetasClientesRepository _comisionesTarjetasClientesRepository = new ComisionesTarjetasClientesRepository();
        public ComisionesTarjetasClientesRepository comisionesTarjetasClientesRepository
        {
            get { return _comisionesTarjetasClientesRepository; }
            set { _comisionesTarjetasClientesRepository = value; }
        }

        public List<ComisionesTarjetasClientes> lsComisionesTarjetasClientes = new List<ComisionesTarjetasClientes>();

        public List<ComisionesTarjetasClientes> CargarComisionesTarjeta(Guid UidCliente)
        {
            return lsComisionesTarjetasClientes = comisionesTarjetasClientesRepository.CargarComisionesTarjeta(UidCliente);
        }

        public bool RegistrarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidCliente)
        {
            Guid UidFranquiciatario = Guid.NewGuid();

            bool result = false;
            if (comisionesTarjetasClientesRepository.RegistrarComisionesTarjeta(
                new ComisionesTarjetasClientes
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidCliente = UidCliente
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidComicionTarjetaCliente)
        {
            bool result = false;
            if (comisionesTarjetasClientesRepository.ActualizarComisionesTarjeta(
                new ComisionesTarjetasClientes
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidComicionTarjetaCliente = UidComicionTarjetaCliente
                }))
            {
                result = true;
            }
            return result;
        }
    }
}
