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
    public class PromocionesServices
    {
        private PromocionesRepository promocionesRepository = new PromocionesRepository();

        public List<Promociones> lsPromociones = new List<Promociones>();
        public List<CBLPromocionesModel> lsCBLPromocionesModel = new List<CBLPromocionesModel>();
        public List<CBLPromocionesModel> lsCBLPromocionesModelCliente = new List<CBLPromocionesModel>();

        public List<LigasUrlsPromocionesModel> lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();
        public List<LigasMultiplePromocionesModel> lsLigasMultiplePromocionesModel = new List<LigasMultiplePromocionesModel>();
        
        public List<FranquiciasCBLPromocionesModel> lsFranquiciasCBLPromocionesModel = new List<FranquiciasCBLPromocionesModel>();

        public List<EventosGenerarLigasModel> lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();
        public List<EventosPromocionesModel> lsEventosPromocionesModel = new List<EventosPromocionesModel>();

        public void CargarPromociones()
        {
            lsPromociones = new List<Promociones>();

            lsPromociones = promocionesRepository.CargarPromociones();
        }

        #region Franquicias
        public List<FranquiciasCBLPromocionesModel> CargarPromocionesFranquicia(Guid UidFraqnuicia)
        {
            return lsFranquiciasCBLPromocionesModel = promocionesRepository.CargarPromocionesFranquicia(UidFraqnuicia);
        }
        public bool EliminarPromocionesFranquicias(Guid UidCliente)
        {
            bool result = false;
            if (promocionesRepository.EliminarPromocionesFranquicias(
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
            if (promocionesRepository.RegistrarPromocionesFranquicias(
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
            return lsFranquiciasCBLPromocionesModel = promocionesRepository.CargarPromocionesFranquicias(UidFraqnuicia);
        }
        #endregion

        #region Empresas
        public List<CBLPromocionesModel> CargarPromociones(Guid UidCliente)
        {
            return lsCBLPromocionesModel = promocionesRepository.CargarPromociones(UidCliente);
        }

        public bool RegistrarPromociones(Guid UidCliente, Guid UidPromocion, decimal DcmComicion)
        {
            bool result = false;
            if (promocionesRepository.RegistrarPromociones(
                new CBLPromocionesModel
                {
                    UidCliente = UidCliente,
                    UidPromocion = UidPromocion,
                    DcmComicion = DcmComicion 
                }))
            {
                result = true;
            }
            return result;
        }

        public bool EliminarPromociones(Guid UidCliente)
        {
            bool result = false;
            if (promocionesRepository.EliminarPromociones(
                new CBLPromocionesModel
                {
                    UidCliente = UidCliente
                }))
            {
                result = true;
            }
            return result;
        }
        #endregion

        #region Clientes
        public List<CBLPromocionesModel> CargarPromocionesClientes(Guid UidCliente)
        {
            return lsCBLPromocionesModelCliente = promocionesRepository.CargarPromocionesClientes(UidCliente);
        }

        #region Eventos
        public void CargarPromocionesEvento(Guid UidCliente, Guid UidEvento)
        {
            lsEventosGenerarLigasModel = new List<EventosGenerarLigasModel>();

            lsEventosGenerarLigasModel = promocionesRepository.CargarPromocionesEvento(UidCliente, UidEvento);
        }
        public List<EventosPromocionesModel> ObtenerPromocionesEvento(Guid UidCliente)
        {
            return lsEventosPromocionesModel = promocionesRepository.ObtenerPromocionesEvento(UidCliente);
        }
        #endregion
        #endregion

        #region PromocionesValidas
        public void CargarPromocionesValidas(Guid UidLigaAsociado)
        {
            lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            lsLigasUrlsPromocionesModel = promocionesRepository.CargarPromocionesValidas(UidLigaAsociado);
        }
        public void ValidarPromociones(Guid UidLigaAsociado)
        {
            lsLigasUrlsPromocionesModel = new List<LigasUrlsPromocionesModel>();

            lsLigasUrlsPromocionesModel = promocionesRepository.ValidarPromociones(UidLigaAsociado);
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
            List<LigasMultiplePromocionesModel> lsLigasMultiplePromocionesModelCopia = lsLigasMultiplePromocionesModel.Where(x=> x.IdUsuario == IdUsuario).ToList();

            foreach (var item in lsLigasMultiplePromocionesModelCopia)
            {
                lsLigasMultiplePromocionesModel.RemoveAt(lsLigasMultiplePromocionesModel.FindIndex(x => x.IdUsuario == item.IdUsuario));
            }
        }
        #endregion
    }
}
