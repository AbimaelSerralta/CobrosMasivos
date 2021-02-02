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
    public class PromocionesClubPagoServices
    {
        private PromocionesClubPagoRepository promocionesClubPagoRepository = new PromocionesClubPagoRepository();

        public List<PromocionesClubPago> lsPromocionesClubPago = new List<PromocionesClubPago>();
        public List<CBLPromocionesClubPagoViewModel> lsCBLPromocionesClubPagoViewModel = new List<CBLPromocionesClubPagoViewModel>();

        public List<PromocionesColegiaturaModel> lsPromocionesColegiaturaModel = new List<PromocionesColegiaturaModel>();
        
        public void CargarPromociones()
        {
            lsPromocionesClubPago = new List<PromocionesClubPago>();

            lsPromocionesClubPago = promocionesClubPagoRepository.CargarPromociones();
        }


        #region Empresas
        public List<CBLPromocionesClubPagoViewModel> CargarPromocionesCliente(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsCBLPromocionesClubPagoViewModel = promocionesClubPagoRepository.CargarPromocionesCliente(UidCliente, UidTipoTarjeta);
        }
        public bool RegistrarPromocionesCliente(Guid UidCliente, Guid UidPromocion, decimal DcmComicion, decimal DcmApartirDe, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (promocionesClubPagoRepository.RegistrarPromocionesCliente(
                new CBLPromocionesClubPagoViewModel
                {
                    UidCliente = UidCliente,
                    UidPromocion = UidPromocion,
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    UidTipoTarjeta = UidTipoTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarPromocionesCliente(Guid UidCliente)
        {
            bool result = false;
            if (promocionesClubPagoRepository.EliminarPromocionesCliente(UidCliente))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Metodos Escuela

        #region Colegiatura
        public List<PromocionesColegiaturaModel> ObtenerPromocionesColegiatura(Guid UidCliente)
        {
            return lsPromocionesColegiaturaModel = promocionesClubPagoRepository.ObtenerPromocionesColegiatura(UidCliente);
        }
        #endregion

        #region Pagos
        public void CargarPromocionesPagosImporte(Guid UidCliente, Guid UidTipoTarjeta, decimal Importe)
        {
            lsCBLPromocionesClubPagoViewModel = new List<CBLPromocionesClubPagoViewModel>();

            lsCBLPromocionesClubPagoViewModel = promocionesClubPagoRepository.CargarPromocionesPagosImporte(UidCliente, UidTipoTarjeta, Importe);
        }
        //public void PromocionesPagosDisponible(List<SuperPromociones> lsSuperPromociones, List<PromocionesColegiaturaModel> lsPromocionesColegiaturaModels)
        //{
        //    List<PromocionesColegiaturaModel> lsTemp = new List<PromocionesColegiaturaModel>();
        //    lsTemp = lsPromocionesColegiaturaModels;

        //    foreach (var item in lsSuperPromociones)
        //    {
        //        foreach (var it in lsTemp)
        //        {
        //            lsPromocionesColegiaturaModels.RemoveAt(lsPromocionesColegiaturaModels.FindIndex(x=>x.UidPromocion = ));
        //        }
        //    }
        //}
        #endregion

        #endregion
    }
}
