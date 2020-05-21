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
    public class EventosServices
    {
        private EventosRepository _eventosRepository = new EventosRepository();
        public EventosRepository eventosRepository
        {
            get { return _eventosRepository; }
            set { _eventosRepository = value; }
        }

        public List<EventosGridViewModel> lsEventosGridViewModel = new List<EventosGridViewModel>();

        #region Metodos Franquicia

        #endregion

        #region Metodos Cliente
        public List<EventosGridViewModel> CargarEventos(Guid UidCliente)
        {
            lsEventosGridViewModel = new List<EventosGridViewModel>();
            return lsEventosGridViewModel = eventosRepository.CargarEventos(UidCliente);
        }
        public void ObtenerEvento(Guid UidEvento)
        {
            eventosRepository.eventosGridViewModel = new EventosGridViewModel();
            eventosRepository.eventosGridViewModel = lsEventosGridViewModel.Find(x => x.UidEvento == UidEvento);
        }
        public bool RegistrarEvento(Guid UidEvento, string VchNombreEvento, string VchDescripcion, DateTime DtRegistro, DateTime DtFHInicio, DateTime DtFHFin, bool BitTipoImporte, decimal DcmImporte, string VchConcepto, bool BitDatosUsuario, string VchUrlEvento, Guid UidPropietario)
        {
            bool result = false;
            if (eventosRepository.RegistrarEvento(
                new EventosGridViewModel
                {
                    UidEvento = UidEvento,
                    VchNombreEvento = VchNombreEvento,
                    VchDescripcion = VchDescripcion,
                    DtRegistro = DtRegistro,
                    DtFHInicio = DtFHInicio,
                    DtFHFin = DtFHFin,
                    BitTipoImporte = BitTipoImporte,
                    DcmImporte = DcmImporte,
                    VchConcepto = VchConcepto,
                    BitDatosUsuario = BitDatosUsuario,
                    VchUrlEvento = VchUrlEvento,
                    UidPropietario = UidPropietario
                }
                ))
            {
                result = true;
            }
            return result;
        }

        public bool ActualizarEvento(Guid UidEvento, string VchNombreEvento, string VchDescripcion, DateTime DtFHInicio, DateTime DtFHFin, bool BitTipoImporte, decimal DcmImporte, string VchConcepto, bool BitDatosUsuario, Guid UidEstatus)
        {
            bool result = false;
            if (eventosRepository.ActualizarEvento(
                new EventosGridViewModel
                {
                    UidEvento = UidEvento,
                    VchNombreEvento = VchNombreEvento,
                    VchDescripcion = VchDescripcion,
                    DtFHInicio = DtFHInicio,
                    DtFHFin = DtFHFin,
                    BitTipoImporte = BitTipoImporte,
                    DcmImporte = DcmImporte,
                    VchConcepto = VchConcepto,
                    BitDatosUsuario = BitDatosUsuario,
                    UidEstatus = UidEstatus
                }))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarHistorialPagoLigas(Guid UidCliente)
        {
            bool result = false;
            if (eventosRepository.EliminarEvento(UidCliente))
            {
                result = true;
            }
            return result;
        }
        public void ObtenerDatosEvento(Guid UidEvento)
        {
            eventosRepository.ObtenerDatosEvento(UidEvento);
        }
        public bool RegistrarPromocionesEvento(Guid UidEvento, Guid UidPromociones)
        {
            bool result = false;
            if (eventosRepository.RegistrarPromocionesEvento(UidEvento, UidPromociones))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarPromocionesEvento(Guid UidEvento)
        {
            bool result = false;
            if (eventosRepository.EliminarPromocionesEvento(UidEvento))
            {
                result = true;
            }
            return result;
        }
        public string ObtenerUrlLiga(string IdReferencia)
        {
            return eventosRepository.ObtenerUrlLiga(IdReferencia);
        }
        public string ObtenerUidAdminCliente(Guid UidCliente)
        {
            return eventosRepository.ObtenerUidAdminCliente(UidCliente);
        }

        public bool ValidarPagoEvento(string IdReferencia)
        {
            return eventosRepository.ValidarPagoEvento(IdReferencia);
        }
        public bool EliminarLigaEvento(string IdReferencia)
        {
            bool result = false;
            if (eventosRepository.EliminarLigaEvento(IdReferencia))
            {
                result = true;
            }
            return result;
        }
        public bool InsertCorreoLigaEvento(string Correo, string IdReferencia)
        {
            bool result = false;
            if (eventosRepository.InsertCorreoLigaEvento(Correo, IdReferencia))
            {
                result = true;
            }
            return result;
        }
        #endregion
    }
}
