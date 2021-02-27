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
    public class ComisionesTarjetasPragaServices
    {
        private ComisionesTarjetasPragaRepository _comisionesTarjetasPragaRepository = new ComisionesTarjetasPragaRepository();
        public ComisionesTarjetasPragaRepository comisionesTarjetasPragaRepository
        {
            get { return _comisionesTarjetasPragaRepository; }
            set { _comisionesTarjetasPragaRepository = value; }
        }

        public List<ComisionesTarjetasPraga> lsComisionesTarjetasPraga = new List<ComisionesTarjetasPraga>();

        public List<ComisionesTarjetasPraga> CargarComisionesTarjeta(Guid UidCliente)
        {
            return lsComisionesTarjetasPraga = comisionesTarjetasPragaRepository.CargarComisionesTarjeta(UidCliente);
        }
        public List<ComisionesTarjetasPraga> CargarComisionesTarjetaTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsComisionesTarjetasPraga = comisionesTarjetasPragaRepository.CargarComisionesTarjetaTipoTarjeta(UidCliente, UidTipoTarjeta);
        }

        public bool RegistrarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidTipoTarjeta, Guid UidCliente)
        {
            bool result = false;
            if (comisionesTarjetasPragaRepository.RegistrarComisionesTarjeta(
                new ComisionesTarjetasPraga
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
            if (comisionesTarjetasPragaRepository.ActualizarComisionesTarjeta(
                new ComisionesTarjetasPraga
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidComicionTarjeta = UidComicionTarjetaCliente
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarComisionesTarjeta(Guid UidCliente)
        {
            bool result = false;
            if (comisionesTarjetasPragaRepository.EliminarComisionesTarjeta(UidCliente))
            {
                result = true;
            }
            return result;
        }
    }
}
