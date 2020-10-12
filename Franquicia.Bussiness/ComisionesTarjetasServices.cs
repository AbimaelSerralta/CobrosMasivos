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
    public class ComisionesTarjetasServices
    {
        private ComisionesTarjetasRepository _comisionesTarjetasRepository = new ComisionesTarjetasRepository();
        public ComisionesTarjetasRepository comisionesTarjetasRepository
        {
            get { return _comisionesTarjetasRepository; }
            set { _comisionesTarjetasRepository = value; }
        }

        public List<ComisionesTarjetas> lsComisionesTarjetas = new List<ComisionesTarjetas>();

        public List<ComisionesTarjetas> CargarComisionesTarjeta()
        {
            return lsComisionesTarjetas = comisionesTarjetasRepository.CargarComisionesTarjeta();
        }

        public bool RegistrarComisionesTarjeta(bool BitComision, decimal DcmComision)
        {
            Guid UidFranquiciatario = Guid.NewGuid();

            bool result = false;
            if (comisionesTarjetasRepository.RegistrarComisionesTarjeta(
                new ComisionesTarjetas
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision
                }))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarComisionesTarjeta(bool BitComision, decimal DcmComision, Guid UidComicionTarjeta)
        {
            bool result = false;
            if (comisionesTarjetasRepository.ActualizarComisionesTarjeta(
                new ComisionesTarjetas
                {
                    BitComision = BitComision,
                    DcmComision = DcmComision,
                    UidComicionTarjeta = UidComicionTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
    }
}
