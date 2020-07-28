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
    public class ColegiaturasServices
    {
        private ColegiaturasRepository _colegiaturasRepository = new ColegiaturasRepository();
        public ColegiaturasRepository colegiaturasRepository
        {
            get { return _colegiaturasRepository; }
            set { _colegiaturasRepository = value; }
        }

        public List<ColegiaturasGridViewModel> lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();

        #region Metodos Cliente
        public List<ColegiaturasGridViewModel> CargarColegiaturas(Guid UidCliente)
        {
            lsColegiaturasGridViewModel = new List<ColegiaturasGridViewModel>();
            return lsColegiaturasGridViewModel = colegiaturasRepository.CargarColegiaturas(UidCliente);
        }
        public void ObtenerColegiatura(Guid UidColegiatura)
        {
            colegiaturasRepository.colegiaturasGridViewModel = new ColegiaturasGridViewModel();
            colegiaturasRepository.colegiaturasGridViewModel = lsColegiaturasGridViewModel.Find(x => x.UidColegiatura == UidColegiatura);
        }
        //public void ObtenerEvento(Guid UidEvento)
        //{
        //    eventosRepository.eventosGridViewModel = new EventosGridViewModel();
        //    eventosRepository.eventosGridViewModel = lsEventosGridViewModel.Find(x => x.UidEvento == UidEvento);
        //}
        public bool RegistrarColegiatura(Guid UidColegiatura, string VchIdentificador, decimal DcmImporte, int IntCantPagos, Guid UidPeriodicidad, DateTime DtFHInicio, bool BitFHLimite, DateTime DtFHLimite, bool BitFHVencimiento, DateTime DtFHVencimiento, bool BitRecargo, string VchTipoRecargo, decimal DcmRecargo, Guid UidCliente)
        {
            bool result = false;
            if (colegiaturasRepository.RegistrarColegiatura(new ColegiaturasGridViewModel
            {
                UidColegiatura = UidColegiatura,
                VchIdentificador = VchIdentificador,
                DcmImporte = DcmImporte,
                IntCantPagos = IntCantPagos,
                UidPeriodicidad = UidPeriodicidad,
                DtFHInicio = DtFHInicio,
                BitFHLimite = BitFHLimite,
                DtFHLimite = DtFHLimite,
                BitFHVencimiento = BitFHVencimiento,
                DtFHVencimiento = DtFHVencimiento,
                BitRecargo = BitRecargo,
                VchTipoRecargo = VchTipoRecargo,
                DcmRecargo = DcmRecargo,
                UidCliente = UidCliente
            }))
            {
                result = true;
            }
            return result;
        }
        public bool ActualizarColegiatura(Guid UidColegiatura, string VchIdentificador, decimal DcmImporte, int IntCantPagos, Guid UidPeriodicidad, DateTime DtFHInicio, bool BitFHLimite, DateTime DtFHLimite, bool BitFHVencimiento, DateTime DtFHVencimiento, bool BitRecargo, string VchTipoRecargo, decimal DcmRecargo)
        {
            bool result = false;
            if (colegiaturasRepository.ActualizarColegiatura(new ColegiaturasGridViewModel
            {
                UidColegiatura = UidColegiatura,
                VchIdentificador = VchIdentificador,
                DcmImporte = DcmImporte,
                IntCantPagos = IntCantPagos,
                UidPeriodicidad = UidPeriodicidad,
                DtFHInicio = DtFHInicio,
                BitFHLimite = BitFHLimite,
                DtFHLimite = DtFHLimite,
                BitFHVencimiento = BitFHVencimiento,
                DtFHVencimiento = DtFHVencimiento,
                BitRecargo = BitRecargo,
                VchTipoRecargo = VchTipoRecargo,
                DcmRecargo = DcmRecargo
            }))
            {
                result = true;
            }
            return result;
        }

        public bool RegistrarColegiaturaFechas(Guid UidColegiatura, DateTime DtFHInicio, DateTime DtFHLimite, DateTime DtFHVencimiento)
        {
            bool result = false;

            if (colegiaturasRepository.RegistrarColegiaturaFechas(UidColegiatura, DtFHInicio, DtFHLimite, DtFHVencimiento))
            {
                result = true;
            }
            return result;
        }
        public bool EliminarColegiaturaFechas(Guid UidColegiatura)
        {
            bool result = false;
            if (colegiaturasRepository.EliminarColegiaturaFechas(UidColegiatura))
            {
                result = true;
            }
            return result;
        }
        public List<ColegiaturasFechasGridViewModel> ObtenerFechaColegiatura(Guid UidColegiatura)
        {
            return colegiaturasRepository.ObtenerFechaColegiatura(UidColegiatura);
        }

        //public bool EliminarHistorialPagoLigas(Guid UidCliente)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarEvento(UidCliente))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public void ObtenerDatosEvento(Guid UidEvento)
        //{
        //    eventosRepository.ObtenerDatosEvento(UidEvento);
        //}
        //public bool RegistrarPromocionesEvento(Guid UidEvento, Guid UidPromociones)
        //{
        //    bool result = false;
        //    if (eventosRepository.RegistrarPromocionesEvento(UidEvento, UidPromociones))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool EliminarPromocionesEvento(Guid UidEvento)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarPromocionesEvento(UidEvento))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public string ObtenerUrlLiga(string IdReferencia)
        //{
        //    return eventosRepository.ObtenerUrlLiga(IdReferencia);
        //}
        //public string ObtenerUidAdminCliente(Guid UidCliente)
        //{
        //    return eventosRepository.ObtenerUidAdminCliente(UidCliente);
        //}

        //public bool ValidarPagoEvento(string IdReferencia)
        //{
        //    return eventosRepository.ValidarPagoEvento(IdReferencia);
        //}
        //public bool EliminarLigaEvento(string IdReferencia)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarLigaEvento(IdReferencia))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool InsertCorreoLigaEvento(string Correo, string IdReferencia)
        //{
        //    bool result = false;
        //    if (eventosRepository.InsertCorreoLigaEvento(Correo, IdReferencia))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}

        //public void BuscarEventos(Guid UidPropietario, string VchNombreEvento, string DtFHInicioDesde, string DtFHInicioHasta, string DtFHFinDesde, string DtFHFinHasta, decimal DcmImporteMayor, decimal DcmImporteMenor, Guid UidEstatus)
        //{
        //    lsEventosGridViewModel = eventosRepository.BuscarEventos(UidPropietario, VchNombreEvento, DtFHInicioDesde, DtFHInicioHasta, DtFHFinDesde, DtFHFinHasta, DcmImporteMayor, DcmImporteMenor, UidEstatus);
        //}

        //public bool RegistrarUsuariosEvento(Guid UidEvento, Guid UidUsuario)
        //{
        //    bool result = false;
        //    if (eventosRepository.RegistrarUsuariosEvento(UidEvento, UidUsuario))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        //public bool EliminarUsuariosEvento(Guid UidEvento)
        //{
        //    bool result = false;
        //    if (eventosRepository.EliminarUsuariosEvento(UidEvento))
        //    {
        //        result = true;
        //    }
        //    return result;
        //}
        #endregion
    }
}
