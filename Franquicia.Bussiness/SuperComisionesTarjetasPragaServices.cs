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
    public class SuperComisionesTarjetasPragaServices
    {
        private SuperComisionesTarjetasPragaRepository _superComisionesTarjetasPragaRepository = new SuperComisionesTarjetasPragaRepository();
        public SuperComisionesTarjetasPragaRepository superComisionesTarjetasPragaRepository
        {
            get { return _superComisionesTarjetasPragaRepository; }
            set { _superComisionesTarjetasPragaRepository = value; }
        }

        public List<SuperComisionesTarjetasPraga> lsSuperComisionesTarjetasPraga = new List<SuperComisionesTarjetasPraga>();

        public List<SuperComisionesTarjetasPraga> CargarComisionesTarjeta()
        {
            return lsSuperComisionesTarjetasPraga = superComisionesTarjetasPragaRepository.CargarComisionesTarjeta();
        }
        public List<SuperComisionesTarjetasPraga> CargarComisionesTarjetaTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsSuperComisionesTarjetasPraga = superComisionesTarjetasPragaRepository.CargarComisionesTarjetaTipoTarjeta(UidCliente, UidTipoTarjeta);
        }

        public bool RegistrarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (superComisionesTarjetasPragaRepository.RegistrarComisionesTarjeta(
                new SuperComisionesTarjetasPraga
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidTipoTarjeta = UidTipoTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidSuperComicionTarjeta)
        {
            bool result = false;
            if (superComisionesTarjetasPragaRepository.ActualizarComisionesTarjeta(
                new SuperComisionesTarjetasPraga
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidSuperComicionTarjeta = UidSuperComicionTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarComisionesTarjeta(Guid UidCliente)
        {
            bool result = false;
            if (superComisionesTarjetasPragaRepository.EliminarComisionesTarjeta(UidCliente))
            {
                result = true;
            }
            return result;
        }
    }
}
