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
    public class PromocionesPragaServices
    {
        private PromocionesPragaRepository promocionesPragaRepository = new PromocionesPragaRepository();

        public List<PromocionesPraga> lsPromocionesPraga = new List<PromocionesPraga>();
        public List<CBLPromocionesPragaViewModel> lsCBLPromocionesPragaViewModel = new List<CBLPromocionesPragaViewModel>();
        
        public List<CBLSuperPromocionesPragaViewModel> lsCBLSuperPromocionesPragaViewModel = new List<CBLSuperPromocionesPragaViewModel>();

        public List<PromocionesColegiaturaModel> lsPromocionesColegiaturaModel = new List<PromocionesColegiaturaModel>();
        
        public void CargarPromociones()
        {
            lsPromocionesPraga = new List<PromocionesPraga>();

            lsPromocionesPraga = promocionesPragaRepository.CargarPromociones();
        }
        public string ObtenerIdPromocion(Guid UidTipoTarjeta, Guid UidPromocion)
        {
            return promocionesPragaRepository.ObtenerIdPromocion(UidTipoTarjeta, UidPromocion);
        }

        #region Metodos Panel Administrativo

        #region Empresas
        public List<CBLPromocionesPragaViewModel> CargarPromocionesCliente(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsCBLPromocionesPragaViewModel = promocionesPragaRepository.CargarPromocionesCliente(UidCliente, UidTipoTarjeta);
        }
        public bool RegistrarPromocionesCliente(Guid UidCliente, Guid UidPromocion, decimal DcmComicion, decimal DcmApartirDe, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (promocionesPragaRepository.RegistrarPromocionesCliente(
                new CBLPromocionesPragaViewModel
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
            if (promocionesPragaRepository.EliminarPromocionesCliente(UidCliente))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Tarifas
        public List<CBLSuperPromocionesPragaViewModel> CargarSuperPromociones(Guid UidTipoTarjeta)
        {
            return lsCBLSuperPromocionesPragaViewModel = promocionesPragaRepository.CargarSuperPromociones(UidTipoTarjeta);
        }
        #endregion

        #endregion

        #region Metodos Escuela

        #region Colegiatura
        public List<PromocionesColegiaturaModel> ObtenerPromocionesColegiatura(Guid UidCliente)
        {
            return lsPromocionesColegiaturaModel = promocionesPragaRepository.ObtenerPromocionesColegiatura(UidCliente);
        }
        #endregion

        #region Pagos
        public void CargarPromocionesPagosImporte(Guid UidCliente, Guid UidTipoTarjeta, decimal Importe)
        {
            lsCBLPromocionesPragaViewModel = new List<CBLPromocionesPragaViewModel>();

            lsCBLPromocionesPragaViewModel = promocionesPragaRepository.CargarPromocionesPagosImporte(UidCliente, UidTipoTarjeta, Importe);
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
