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
    public class ComisionesTarjetasClientesTerminalServices
    {
        private ComisionesTarjetasClientesTerminalRepository _comisionesTarjetasClientesTerminalRepository = new ComisionesTarjetasClientesTerminalRepository();
        public ComisionesTarjetasClientesTerminalRepository comisionesTarjetasClientesTerminalRepository
        {
            get { return _comisionesTarjetasClientesTerminalRepository; }
            set { _comisionesTarjetasClientesTerminalRepository = value; }
        }

        public List<ComisionesTarjetasClientesTerminal> lsComisionesTarjetasClientesTerminal = new List<ComisionesTarjetasClientesTerminal>();

        public List<ComisionesTarjetasClientesTerminal> CargarComisionesTarjetaTerminal(Guid UidCliente)
        {
            return lsComisionesTarjetasClientesTerminal = comisionesTarjetasClientesTerminalRepository.CargarComisionesTarjetaTerminal(UidCliente);
        }
        public List<ComisionesTarjetasClientesTerminal> CargarComisionesTarjetaTerminalTipoTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsComisionesTarjetasClientesTerminal = comisionesTarjetasClientesTerminalRepository.CargarComisionesTarjetaTerminalTipoTarjeta(UidCliente, UidTipoTarjeta);
        }

        public bool RegistrarComisionesTarjetaTerminal(bool BitComision, decimal DcmComision, Guid UidTipoTarjeta, Guid UidCliente)
        {
            bool result = false;
            if (comisionesTarjetasClientesTerminalRepository.RegistrarComisionesTarjetaTerminal(
                new ComisionesTarjetasClientesTerminal
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidTipoTarjeta = UidTipoTarjeta,
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
            if (comisionesTarjetasClientesTerminalRepository.ActualizarComisionesTarjeta(
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
        public bool EliminarComisionesTarjetaTerminal(Guid UidCliente)
        {
            bool result = false;
            if (comisionesTarjetasClientesTerminalRepository.EliminarComisionesTarjetaTerminal(UidCliente))
            {
                result = true;
            }
            return result;
        }
    }
}
