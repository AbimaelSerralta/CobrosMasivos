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
    public class PromocionesTerminalServices
    {
        private PromocionesTerminalRepository promocionesTerminalRepository = new PromocionesTerminalRepository();

        //public List<Promociones> lsPromociones = new List<Promociones>();
        public List<PromocionesTerminal> lsPromocionesTerminal = new List<PromocionesTerminal>();

        public List<CBLPromocionesTerminalViewModel> lsCBLPromocionesTerminalViewModel = new List<CBLPromocionesTerminalViewModel>();
        public List<CBLPromocionesModel> lsCBLPromocionesModelCliente = new List<CBLPromocionesModel>();

        public List<LigasUrlsPromocionesModel> lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();
        public List<LigasMultiplePromocionesModel> lsLigasMultiplePromocionesModel = new List<LigasMultiplePromocionesModel>();

        public List<FranquiciasCBLPromocionesModel> lsFranquiciasCBLPromocionesModel = new List<FranquiciasCBLPromocionesModel>();

        public List<EventosGenerarLigasModel> lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();
        public List<EventosPromocionesModel> lsEventosPromocionesModel = new List<EventosPromocionesModel>();


        public List<SelectPagoLigaModel> lsSelectPagoLigaModel = new List<SelectPagoLigaModel>();

        public List<PromocionesColegiaturaModel> lsPromocionesColegiaturaModel = new List<PromocionesColegiaturaModel>();
        
        public void CargarPromocionesTerminal()
        {
            lsPromocionesTerminal = new List<PromocionesTerminal>();

            lsPromocionesTerminal = promocionesTerminalRepository.CargarPromocionesTerminal();
        }

        #region Franquicias
        public List<FranquiciasCBLPromocionesModel> CargarPromocionesFranquicia(Guid UidFraqnuicia)
        {
            return lsFranquiciasCBLPromocionesModel = promocionesTerminalRepository.CargarPromocionesFranquicia(UidFraqnuicia);
        }
        public bool EliminarPromocionesFranquicias(Guid UidCliente)
        {
            bool result = false;
            if (promocionesTerminalRepository.EliminarPromocionesFranquicias(
                new FranquiciasCBLPromocionesModel
                {
                    UidFranquicia = UidCliente
                }))
            {
                result = true;
            }
            return result;
        }
        public bool RegistrarPromocionesFranquicias(Guid UidCliente, Guid UidPromocion, decimal DcmComicion)
        {
            bool result = false;
            if (promocionesTerminalRepository.RegistrarPromocionesFranquicias(
                new FranquiciasCBLPromocionesModel
                {
                    UidFranquicia = UidCliente,
                    UidPromocion = UidPromocion,
                    DcmComicion = DcmComicion
                }))
            {
                result = true;
            }
            return result;
        }
        public List<FranquiciasCBLPromocionesModel> CargarPromocionesFranquicias(Guid UidFraqnuicia)
        {
            return lsFranquiciasCBLPromocionesModel = promocionesTerminalRepository.CargarPromocionesFranquicias(UidFraqnuicia);
        }
        #endregion

        #region Empresas
        public List<CBLPromocionesTerminalViewModel> CargarPromocionesTerminalCliente(Guid UidCliente, Guid UidTipoTarjeta)
        {
            return lsCBLPromocionesTerminalViewModel = promocionesTerminalRepository.CargarPromocionesTerminalCliente(UidCliente, UidTipoTarjeta);
        }
        public bool RegistrarPromocionesTerminalCliente(Guid UidCliente, Guid UidPromocionTerminal, decimal DcmComicion, decimal DcmApartirDe, Guid UidTipoTarjeta)
        {
            bool result = false;
            if (promocionesTerminalRepository.RegistrarPromocionesTerminalCliente(
                new CBLPromocionesTerminalViewModel
                {
                    UidCliente = UidCliente,
                    UidPromocionTerminal = UidPromocionTerminal,
                    DcmComicion = DcmComicion,
                    DcmApartirDe = DcmApartirDe,
                    UidTipoTarjeta = UidTipoTarjeta
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarPromocionesTerminalCliente(Guid UidCliente)
        {
            bool result = false;
            if (promocionesTerminalRepository.EliminarPromocionesTerminalCliente(UidCliente))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Clientes
        public List<CBLPromocionesModel> CargarPromocionesClientes(Guid UidCliente)
        {
            return lsCBLPromocionesModelCliente = promocionesTerminalRepository.CargarPromocionesClientes(UidCliente);
        }

        #region Eventos
        public void CargarPromocionesEventoImporte(Guid UidCliente, Guid UidEvento, string Importe)
        {
            lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();

            lsEventosGenerarLigasModel = promocionesTerminalRepository.CargarPromocionesEventoImporte(UidCliente, UidEvento, Importe);
        }
        public void CargarPromocionesEvento(Guid UidCliente, Guid UidEvento)
        {
            lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();

            lsEventosGenerarLigasModel = promocionesTerminalRepository.CargarPromocionesEvento(UidCliente, UidEvento);
        }
        public List<EventosPromocionesModel> ObtenerPromocionesEvento(Guid UidCliente)
        {
            return lsEventosPromocionesModel = promocionesTerminalRepository.ObtenerPromocionesEvento(UidCliente);
        }
        #endregion
        #endregion

        #region Metodos Usuarios final
        public void CargarPromocionesEventoUsuarioFinal(Guid UidCliente, Guid UidEvento)
        {
            lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();

            lsEventosGenerarLigasModel = promocionesTerminalRepository.CargarPromocionesEventoUsuarioFinal(UidCliente, UidEvento);
        }
        public void CargarPromoPagoLigaUsuarioFinal(Guid UidLigaAsociado)
        {
            lsSelectPagoLigaModel = new List<SelectPagoLigaModel>();

            lsSelectPagoLigaModel = promocionesTerminalRepository.CargarPromoPagoLigaUsuarioFinal(UidLigaAsociado);
        }
        #endregion  

        #region PromocionesValidas
        public void CargarPromocionesValidas(Guid UidLigaAsociado)
        {
            lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            lsLigasUrlsPromocionesModel = promocionesTerminalRepository.CargarPromocionesValidas(UidLigaAsociado);
        }
        public void ValidarPromociones(Guid UidLigaAsociado)
        {
            lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            lsLigasUrlsPromocionesModel = promocionesTerminalRepository.ValidarPromociones(UidLigaAsociado);
        }
        #endregion

        #region Ligas multiples
        public List<LigasMultiplePromocionesModel> PromocionesMultiples(string Value, int IdUsuario, string VchDescripcion)
        {
            lsLigasMultiplePromocionesModel.Add(new LigasMultiplePromocionesModel { UidPromocion = Guid.Parse(Value), IdUsuario = IdUsuario, VchDescripcion = VchDescripcion });

            return lsLigasMultiplePromocionesModel;
        }
        public List<LigasMultiplePromocionesModel> PromocionesMultiplesTemporal(string Value, int IdUsuario, string VchDescripcion, string IntAuxiliar)
        {
            lsLigasMultiplePromocionesModel.Add(new LigasMultiplePromocionesModel { UidPromocion = Guid.Parse(Value), IdUsuario = IdUsuario, VchDescripcion = VchDescripcion, IntAuxiliar = IntAuxiliar });

            return lsLigasMultiplePromocionesModel;
        }
        public void EliminarPromocionesMultiplesTemporal(int IdUsuario, string IntAuxiliar)
        {
            List<LigasMultiplePromocionesModel> lsLigasMultiplePromocionesModelCopia = lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == IdUsuario && x.IntAuxiliar == IntAuxiliar).ToList();

            foreach (var item in lsLigasMultiplePromocionesModelCopia)
            {
                lsLigasMultiplePromocionesModel.RemoveAt(lsLigasMultiplePromocionesModel.FindIndex(x => x.IdUsuario == item.IdUsuario && x.IntAuxiliar == IntAuxiliar));
            }
        }
        public void EliminarPromocionesMultiples(int IdUsuario)
        {
            List<LigasMultiplePromocionesModel> lsLigasMultiplePromocionesModelCopia = lsLigasMultiplePromocionesModel.Where(x => x.IdUsuario == IdUsuario).ToList();

            foreach (var item in lsLigasMultiplePromocionesModelCopia)
            {
                lsLigasMultiplePromocionesModel.RemoveAt(lsLigasMultiplePromocionesModel.FindIndex(x => x.IdUsuario == item.IdUsuario));
            }
        }
        #endregion




        #region Metodos Escuela

        #region Colegiatura
        public List<PromocionesColegiaturaModel> ObtenerPromocionesColegiatura(Guid UidCliente)
        {
            return lsPromocionesColegiaturaModel = promocionesTerminalRepository.ObtenerPromocionesColegiatura(UidCliente);
        }
        #endregion

        #region Pagos
        public void CargarPromocionesPagosImporte(Guid UidCliente, Guid UidTipoTarjeta, decimal Importe)
        {
            lsCBLPromocionesTerminalViewModel = new List<CBLPromocionesTerminalViewModel>();

            lsCBLPromocionesTerminalViewModel = promocionesTerminalRepository.CargarPromocionesPagosImporte(UidCliente, UidTipoTarjeta, Importe);
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
