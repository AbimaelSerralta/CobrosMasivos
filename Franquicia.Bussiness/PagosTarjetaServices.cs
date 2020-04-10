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
    public class PagosTarjetaServices
    {
        private PagosTarjetaRepository _pagosTarjetaRepository = new PagosTarjetaRepository();
        public PagosTarjetaRepository pagosTarjetaRepository
        {
            get { return _pagosTarjetaRepository; }
            set { _pagosTarjetaRepository = value; }
        }
        
        public List<PagosTarjeta> lsPagosTarjeta = new List<PagosTarjeta>();
        public List<PagosTarjetaDetalleGridViewModel> lsPagosTarjetaDetalleGridViewModel = new List<PagosTarjetaDetalleGridViewModel>();

        public void ObtenerEstatusLiga(string Liga)
        {
            lsPagosTarjeta = new List<PagosTarjeta>();

            lsPagosTarjeta = pagosTarjetaRepository.ObtenerEstatusLiga(Liga);
        }
        public void DetalleLiga(string IdReferencia)
        {
            lsPagosTarjetaDetalleGridViewModel = new List<PagosTarjetaDetalleGridViewModel>();

            lsPagosTarjetaDetalleGridViewModel = pagosTarjetaRepository.DetalleLiga(IdReferencia);
        }
        public void DetalleLigaPromocion(Guid UidLigaAsociado)
        {
            lsPagosTarjetaDetalleGridViewModel = new List<PagosTarjetaDetalleGridViewModel>();

            lsPagosTarjetaDetalleGridViewModel = pagosTarjetaRepository.DetalleLigaPromocion(UidLigaAsociado);
        }
    }
}
