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
    public class LigasUrlsServices
    {
        private LigasUrlsRepository _ligasUrlsRepository = new LigasUrlsRepository();
        public LigasUrlsRepository ligasUrlsRepository
        {
            get { return _ligasUrlsRepository; }
            set { _ligasUrlsRepository = value; }
        }

        public List<LigasUrlsListViewModel> lsLigasUrlsListViewModel = new List<LigasUrlsListViewModel>();
        public List<LigasUrlsGridViewModel> lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

        public List<LigasUsuariosFinalGridViewModel> lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

        public List<LigasUrlsListViewModel> TotalEnvioLigas(Guid UidCliente)
        {
            lsLigasUrlsListViewModel = new List<LigasUrlsListViewModel>();

            return lsLigasUrlsListViewModel = ligasUrlsRepository.TotalEnvioLigas(UidCliente);
        }

        #region Cliente
        public List<LigasUrlsGridViewModel> ConsultarEstatusLiga(Guid UidCliente)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.ConsultarEstatusLiga(UidCliente);
        }
        public List<LigasUrlsGridViewModel> BuscarLigas(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.BuscarLigas(UidCliente, Identificador, Nombre, ApePaterno, ApeMaterno, Asunto, Concepto, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, VencimientoDesde, VencimientoHasta, Estatus);
        }
        public void ContruirLiga(Guid UidLigaUrl)
        {
            ligasUrlsRepository.ContruirLiga(UidLigaUrl);
        }

        #endregion

        #region ClientePayCard
        public void ObtenerDatosUrl(string IdReferencia)
        {
            ligasUrlsRepository.ObtenerDatosUrl(IdReferencia);
        }
        #endregion

        #region UsuariosFinal
        public List<LigasUsuariosFinalGridViewModel> ConsultarLigaUsuarioFinal(Guid UidCliente)
        {
            lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            return lsLigasUsuariosFinalGridViewModel = ligasUrlsRepository.ConsultarLigaUsuarioFinal(UidCliente);
        }
        public string ObtenerPromocionesUsuarioFinal(Guid UidLigaUrl)
        {
            return ligasUrlsRepository.ObtenerPromocionesUsuarioFinal(UidLigaUrl);
        }
        public string ObtenerUrlLigaUsuarioFinal(Guid UidLigaUrl)
        {
            return ligasUrlsRepository.ObtenerUrlLigaUsuarioFinal(UidLigaUrl);
        }
        public List<LigasUsuariosFinalGridViewModel> BuscarLigasUsuarioFinal(Guid UidUsuario, string Identificador, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            return lsLigasUsuariosFinalGridViewModel = ligasUrlsRepository.BuscarLigasUsuarioFinal(UidUsuario, Identificador, Asunto, Concepto, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, VencimientoDesde, VencimientoHasta, Estatus);
        }
        #endregion

        #region LigasUrlFranquicias
        public List<LigasUrlsGridViewModel> ConsultarEstatusLigaFranquicia(Guid UidFranquicia)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.ConsultarEstatusLigaFranquicia(UidFranquicia);
        }
        public List<LigasUrlsGridViewModel> BuscarLigasFranquicia(Guid UidFranquicia, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.BuscarLigasFranquicia(UidFranquicia, Identificador, Nombre, ApePaterno, ApeMaterno, Asunto, Concepto, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, VencimientoDesde, VencimientoHasta, Estatus);
        }
        #endregion

        #region Metodos Escuela

        #region ReporteLigasPadres
        public List<LigasUsuariosFinalGridViewModel> ConsultarLigaPadres(Guid UidCliente)
        {
            lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            return lsLigasUsuariosFinalGridViewModel = ligasUrlsRepository.ConsultarLigaPadres(UidCliente);
        }
        public List<LigasUsuariosFinalGridViewModel> BuscarLigasPadres(Guid UidUsuario, string Identificador, string Asunto, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta, string VencimientoDesde, string VencimientoHasta, string Estatus)
        {
            lsLigasUsuariosFinalGridViewModel = new List<LigasUsuariosFinalGridViewModel>();

            return lsLigasUsuariosFinalGridViewModel = ligasUrlsRepository.BuscarLigasPadres(UidUsuario, Identificador, Asunto, Concepto, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta, VencimientoDesde, VencimientoHasta, Estatus);
        }
        #endregion

        #region ReporteLigasEscuela
        public List<LigasUrlsGridViewModel> ConsultarEstatusLigaEscuela(Guid UidCliente)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.ConsultarEstatusLigaEscuela(UidCliente);
        }
        public List<LigasUrlsGridViewModel> BuscarLigaEscuela(Guid UidCliente, string Identificador, string Nombre, string ApePaterno, string ApeMaterno, string Concepto, decimal ImporteMayor, decimal ImporteMenor, string RegistroDesde, string RegistroHasta)
        {
            lsLigasUrlsGridViewModel = new List<LigasUrlsGridViewModel>();

            return lsLigasUrlsGridViewModel = ligasUrlsRepository.BuscarLigaEscuela(UidCliente, Identificador, Nombre, ApePaterno, ApeMaterno, Concepto, ImporteMayor, ImporteMenor, RegistroDesde, RegistroHasta);
        }
        #endregion

        #endregion
    }
}
