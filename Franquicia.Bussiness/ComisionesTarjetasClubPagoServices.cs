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
    public class ComisionesTarjetasClubPagoServices
    {
        private ComisionesTarjetasClubPagoRepository _comisionesTarjetasClubPagoRepository = new ComisionesTarjetasClubPagoRepository();
        public ComisionesTarjetasClubPagoRepository comisionesTarjetasClubPagoRepository
        {
            get { return _comisionesTarjetasClubPagoRepository; }
            set { _comisionesTarjetasClubPagoRepository = value; }
        }

        public List<ComisionesTarjetasClubPago> lsComisionesTarjetasClubPago = new List<ComisionesTarjetasClubPago>();

        public List<ComisionesTarjetasClubPago> CargarComisionesTarjeta(Guid UidCliente)
        {
            return lsComisionesTarjetasClubPago = comisionesTarjetasClubPagoRepository.CargarComisionesTarjeta(UidCliente);
        }
        public List<ComisionesTarjetasClubPago> CargarComisionesTarjetaTarjeta(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsComisionesTarjetasClubPago = comisionesTarjetasClubPagoRepository.CargarComisionesTarjetaTipoTarjeta(UidCliente, UidTipoTarjeta);
        }

        public bool RegistrarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidTipoTarjeta, Guid UidCliente)
        {
            bool result = false;
            if (comisionesTarjetasClubPagoRepository.RegistrarComisionesTarjeta(
                new ComisionesTarjetasClubPago
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
            if (comisionesTarjetasClubPagoRepository.ActualizarComisionesTarjeta(
                new ComisionesTarjetasClubPago
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
            if (comisionesTarjetasClubPagoRepository.EliminarComisionesTarjeta(UidCliente))
            {
                result = true;
            }
            return result;
        }
    }
}
