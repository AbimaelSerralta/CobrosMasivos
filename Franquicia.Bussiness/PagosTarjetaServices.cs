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
        
        public List<PagTarjDetalUsFinalGridViewModel> lsPagTarjDetalUsFinalGridViewModel = new List<PagTarjDetalUsFinalGridViewModel>();


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

        #region UsuariosFinales
        public bool ValidarPagoUsuarioFinal(string IdReferencia)
        {
            return pagosTarjetaRepository.ValidarPagoUsuarioFinal(IdReferencia);
        }
        public void DetalleLigaUsuarioFinal(Guid UidLigaUrl)
        {
            lsPagTarjDetalUsFinalGridViewModel = new List<PagTarjDetalUsFinalGridViewModel>();

            lsPagTarjDetalUsFinalGridViewModel = pagosTarjetaRepository.DetalleLigaUsuarioFinal(UidLigaUrl);
        }
        public void DetalleLigaPromocionUsuarioFinal(Guid UidLigaAsociado)
        {
            lsPagTarjDetalUsFinalGridViewModel = new List<PagTarjDetalUsFinalGridViewModel>();

            lsPagTarjDetalUsFinalGridViewModel = pagosTarjetaRepository.DetalleLigaPromocionUsuarioFinal(UidLigaAsociado);
        }
        #endregion
    }
}
